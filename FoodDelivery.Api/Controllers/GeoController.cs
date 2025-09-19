using FoodDelivery.Infrastructure.DTO;
using FoodDelivery.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FoodDelivery.Infrastructure.Repository;

namespace FoodDelivery.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeocodingController : ControllerBase
    {
        private readonly IGeocodingService _geocodingService;

        public GeocodingController(IGeocodingService geocodingService)
        {
            _geocodingService = geocodingService;
        }

        [HttpPost("get-coordinates")]
        public async Task<IActionResult> GetCoordinates([FromBody] GeocodeRequestDto dto)
        {
            var result = await _geocodingService.GetCoordinatesAsync(dto.Address);
            if (result == null)
                return BadRequest("Unable to geocode address.");

            return Ok(result);
        }
    }

}
