using FoodDelivery.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FoodDelivery.Infrastructure.DTO;
using FoodDelivery.Infrastructure.Repository;

namespace FoodDelivery.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {


     
            private readonly IRestaurantRepository _repo;

            public RestaurantController(IRestaurantRepository repo)
            {
                _repo = repo;
            }

            [HttpPost("submit")]
       
        public async Task<IActionResult> SubmitDetails([FromBody] RestaurantResponseDto dto)
        {
            var restaurant = new Restaurant
            {
                UserId = dto.UserId,
                Address = dto.Address,
                FssaiId = dto.FssaiId,
                PinCode = dto.PinCode,
                FssaiImage = dto.FssaiImage,
                TradelicenseImage = dto.TradelicenseImage,
                TradeId = dto.TradeId,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude
            };

            try
            {
                var result = await _repo.SubmitRestaurantDetailsAsync(restaurant);

                var response = new RestaurantResponseDto
                {
                    RestaurantId = result.RestaurantId,
                    UserId = result.UserId ?? 0,
                    Address = result.Address,
                    FssaiId = result.FssaiId,
                    PinCode = result.PinCode,
                    FssaiImage = result.FssaiImage,
                    TradelicenseImage = result.TradelicenseImage,
                    TradeId = result.TradeId,
                    Latitude = result.Latitude,
                    Longitude = result.Longitude
                };

                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}






