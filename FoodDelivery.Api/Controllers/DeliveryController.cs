using Azure;
using FoodDelivery.Domain.Models;
using FoodDelivery.Infrastructure.DTO;
using FoodDelivery.Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
            private readonly IDeliveryagentRepository _repo;
            private readonly IGeocodingService _geocodingService;

        public DeliveryController(IDeliveryagentRepository repo, IGeocodingService geocodingService)
            {
                _repo = repo;
            _geocodingService = geocodingService;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitDetails([FromBody] DeliveryAgentDTO dto)
        {
            var fullAddress = $"{dto.Address}";
            var geoResult = await _geocodingService.GetCoordinatesAsync(fullAddress);

            var agent = new DeliveryAgent
            {
                UserId = dto.UserId,
                DocumentUrl = dto.DocumentUrl,
                Latitude = geoResult?.Latitude,
                Longitude = geoResult?.Longitude,
                Address = dto.Address
            };
            var result = await _repo.SubmitAgentDetailsAsync(agent);

            if (result == null)
            {
                return BadRequest("Invalid restaurant user.");
            }

            return Ok(result);
        }

        [HttpPut("delivery-agent/update")]
        public async Task<IActionResult> UpdateDeliveryAgent([FromBody] DeliveryAgentResponseDto dto)
        {
            var result = await _repo.UpdateDeliveryAgentAsync(dto);
            if (!result) return NotFound("Delivery agent not found.");
            return Ok("Delivery agent updated successfully.");
        }
    }
}





