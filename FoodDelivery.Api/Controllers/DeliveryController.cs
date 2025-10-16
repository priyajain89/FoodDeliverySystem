using Azure;
using FoodDelivery.Domain.Data;
using FoodDelivery.Domain.Models;
using FoodDelivery.Infrastructure.DTO;
using FoodDelivery.Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FoodDelivery.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {


        private readonly IOrderRepository _orderRepo;
        private readonly IDeliveryagentRepository _repo;
        private readonly IGeocodingService _geocodingService;

        public DeliveryController(IOrderRepository orderRepo, IDeliveryagentRepository repo, IGeocodingService geocodingService)
        {
            _repo = repo;
            _geocodingService = geocodingService;
            _orderRepo = orderRepo;
        }

        [HttpPost("submit")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> SubmitDetails([FromForm] DeliveryAgentDTO dto)
        {

            var fullAddress = $"{dto.Address}";

            // Get coordinates from geocoding service
            var geoResult = await _geocodingService.GetCoordinatesAsync(fullAddress);

            var agent = new DeliveryAgent
            {
                UserId = dto.UserId,
                Latitude = geoResult?.Latitude,
                Longitude = geoResult?.Longitude,
                Address = dto.Address
            };


            var result = await _repo.SubmitAgentDetailsAsync(agent, dto.DocumentUrl);

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


        [HttpGet("orders/agentId")]
        public async Task<IActionResult> GetOrdersForAgent()
        {

            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("id");

            if (idClaim == null || !int.TryParse(idClaim.Value, out int agentId))
            {
                return Unauthorized("Agent ID not found in token.");
            }

            var orders = await _orderRepo.GetOrdersForAgentAsync(agentId);
            return Ok(orders);
        }


        [HttpPost("mark-delivered/{orderId}")]
        public async Task<IActionResult> MarkOrderAsDelivered(int orderId)
        {
            var order = await _orderRepo.GetOrderByIdAsync(orderId);
            if (order == null) return NotFound("Order not found.");

            order.Status = "Delivered";
            await _orderRepo.UpdateOrderAsync(order);

            if (order.AgentId.HasValue)
            {
                var updated = await _repo.MarkAgentAvailableAsync(order.AgentId.Value);
                if (!updated)
                {
                    return BadRequest("Failed to update agent availability.");
                }
            }

            return Ok("Order marked as delivered and agent availability updated.");
        }
    }
}