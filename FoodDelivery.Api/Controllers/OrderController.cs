using FoodDelivery.Infrastructure.DTO;
using FoodDelivery.Infrastructure.Enums;
using FoodDelivery.Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FoodDelivery.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBillService _assignmentService;

        public OrderController(IOrderRepository orderRepository, IBillService assignmentService)
        {
            _orderRepository = orderRepository;
            _assignmentService = assignmentService;
        }

        [HttpPost("create-from-cart")]
        [Authorize]
        public async Task<IActionResult> CreateOrderFromCart([FromBody] CreateOrderFromCartDto dto)
        {
            try
            {
                var customerIdClaim = User.FindFirst("id")?.Value;
                if (string.IsNullOrEmpty(customerIdClaim))
                    return Unauthorized("CustomerId not found in token.");

                var customerId = int.Parse(customerIdClaim);

                var orderId = await _orderRepository.CreateOrderFromCartAsync(customerId, dto);
                return Ok(new
                {
                    OrderId = orderId,
                    Status = OrderEnums.OrderStatus.Pending.ToString(),
                    Message = "Order created. Please select address."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("assign-address")]
        public async Task<IActionResult> AssignAddress([FromBody] AssignAddressToOrderDto dto)
        {
            var success = await _orderRepository.AssignAddressToOrderAsync(dto);
            if (!success)
                return NotFound("Order not found.");

            return Ok(new
            {
                Status = OrderEnums.OrderStatus.ReadyForPickup.ToString(),
                Message = "Address assigned to order."
            });
        }

        [HttpPut("assign-agent/{orderId}")]
        public async Task<IActionResult> AssignAgentToOrder(int orderId)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(orderId);
                if (order == null)
                    return NotFound("Order not found.");

                var agent = await _assignmentService.AssignNearestAgentAsync(order);
                if (agent == null)
                    return NotFound("No available delivery agents.");

                // You can optionally update status here if needed
                // order.Status = OrderEnums.OrderStatus.OutForDelivery.ToString();
                await _orderRepository.UpdateOrderAsync(order);

                return Ok(new
                {
                    Message = "Agent assigned to order.",
                    AgentId = agent.AgentId,
                    AgentName = agent.User?.Name ?? "Unknown"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}