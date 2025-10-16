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

        [Consumes("multipart/form-data")]

        public async Task<IActionResult> SubmitDetails([FromForm] RestaurantResponseDto dto)

        {

            var fullAddress = $"{dto.Address}, {dto.PinCode}";

            var geoResult = await _geocodingService.GetCoordinatesAsync(fullAddress);

            var restaurant = new Restaurant

            {

                UserId = dto.UserId,

                Address = dto.Address,

                FssaiId = dto.FssaiId,

                PinCode = dto.PinCode,

                TradeId = dto.TradeId,

                Latitude = geoResult?.Latitude,

                Longitude = geoResult?.Longitude

            };


            var newresult = await _repo.SubmitRestaurantDetailsAsync(restaurant, dto.FssaiImage, dto.TradelicenseImage);

            if (newresult == null)

                return BadRequest("Invalid restaurant user.");


            return Ok(newresult);


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




