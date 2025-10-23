using FoodDelivery.Domain.Models;
using FoodDelivery.Infrastructure.DTO;
using FoodDelivery.Infrastructure.Repository;
using FoodDelivery.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [Authorize(Roles = "Restaurant")]
        [HttpGet("getAllrestaurants")]

        public async Task<IActionResult> GetAllRestaurants()

        {

            var restaurants = await _repo.GetAllRestaurantsAsync();

            return Ok(restaurants);

        }

        [Authorize(Roles = "Restaurant")]
        [HttpGet("profile")]
        public async Task<IActionResult> GetRestaurantProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            int userId = int.Parse(userIdClaim.Value);
            var restaurant = await _repo.GetRestaurantByUserIdAsync(userId);

            if (restaurant == null)
                return NotFound("Restaurant profile not found.");

            return Ok(restaurant);
        }
        [Authorize(Roles = "Restaurant")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateRestaurant([FromBody] RestaurantIDDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            int userId = int.Parse(userIdClaim.Value);

            // Ensure the user is updating their own restaurant
            if (dto.UserId != userId)
                return Forbid("You can only update your own restaurant profile.");

            var success = await _repo.UpdateRestaurantAsync(dto);
            if (!success)
                return NotFound("Restaurant not found or update failed.");

            return Ok("Restaurant profile updated successfully.");
        }




    }

}




