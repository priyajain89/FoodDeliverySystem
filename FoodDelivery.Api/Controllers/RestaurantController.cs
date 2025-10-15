using FoodDelivery.Domain.Models;
using FoodDelivery.Infrastructure.DTO;
using FoodDelivery.Infrastructure.Repository;
using FoodDelivery.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {

        private readonly IRestaurantRepository _repo;
        private readonly IGeocodingService _geocodingService;

        public RestaurantController(IRestaurantRepository repo, IGeocodingService geocodingService)
        {
            _repo = repo;
            _geocodingService = geocodingService;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitDetails([FromBody] RestaurantResponseDto dto)
        {
            var fullAddress = $"{dto.Address}, {dto.PinCode}";

            var geoResult = await _geocodingService.GetCoordinatesAsync(fullAddress);

            var restaurant = new Restaurant
            {
                UserId = dto.UserId,
                Address = dto.Address,
                FssaiId = dto.FssaiId,
                PinCode = dto.PinCode,
                FssaiImage = dto.FssaiImage,
                TradelicenseImage = dto.TradelicenseImage,
                TradeId = dto.TradeId,
                Latitude = geoResult?.Latitude,
                Longitude = geoResult?.Longitude
            };


            var result = await _repo.SubmitRestaurantDetailsAsync(restaurant);

            if (result == null)
            {
                return BadRequest("Invalid restaurant user.");
            }

            return Ok(result);



        }


        [HttpGet("getAllrestaurants")]
        public async Task<IActionResult> GetAllRestaurants()
        {
            var restaurants = await _repo.GetAllRestaurantsAsync();
            return Ok(restaurants);
        }

        [HttpPut("restaurant/update")]
        public async Task<IActionResult> UpdateRestaurant([FromBody] RestaurantIDDto dto)
        {
            var result = await _repo.UpdateRestaurantAsync(dto);
            if (!result) return NotFound("Restaurant not found.");
            return Ok("Restaurant updated successfully.");
        }



    }
}






