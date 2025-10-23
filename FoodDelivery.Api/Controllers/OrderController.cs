using FoodDelivery.Infrastructure.DTO;

using FoodDelivery.Infrastructure.Repository;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers

{

    [Route("api/[controller]")]

    [ApiController]

    public class OrderController : ControllerBase

    {

        private readonly IOrderRepository _orderRepository;

        private readonly IBillService _assignmentService;

        private readonly IRestaurantRepository _restRepository;

        public OrderController(IOrderRepository orderRepository, IBillService assignmentService, IRestaurantRepository restRepository)

        {

            _orderRepository = orderRepository;

            _assignmentService = assignmentService;

            _restRepository = restRepository;

        }

        [HttpPost("create-from-cart")]

        public async Task<IActionResult> CreateOrderFromCart([FromBody] CreateOrderFromCartDto dto)

        {

            try

            {

                var customerIdClaim = User.FindFirst("id")?.Value;

                if (string.IsNullOrEmpty(customerIdClaim))

                    return Unauthorized("CustomerId not found in token.");

                var customerId = int.Parse(customerIdClaim);

                var orderId = await _orderRepository.CreateOrderFromCartAsync(customerId, dto);

                return Ok(new { OrderId = orderId, Message = "Order created. Please select address." });

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

            return Ok(new { Message = "Address assigned to order." });

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

                await _orderRepository.UpdateOrderAsync(order);

                return Ok(new

                {

                    Message = "Agent assigned to order.",

                    AgentId = agent.UserId,

                    AgentName = agent.User?.Name ?? "Unknown"

                });

            }

            catch (Exception ex)

            {

                return BadRequest(new { Message = ex.Message });

            }

        }

        [HttpGet("restaurant-orders")]
        [Authorize(Roles = "Restaurant")]
       
        public async Task<IActionResult> GetOrdersForRestaurant()
        {
            try
            {
                var userIdClaim = User.FindFirst("id")?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized("User ID not found in token.");

                int userId = int.Parse(userIdClaim);

                // Get RestaurantId using RestaurantRepository
                var restaurant = await _restRepository.GetRestaurantByUserIdAsync(userId);
                if (restaurant == null)
                    return NotFound("Restaurant not found for this user.");

                int restaurantId = restaurant.RestaurantId;

                var orders = await _orderRepository.GetOrdersForRestaurantAsync(restaurantId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("update-status")]
        [Authorize(Roles = "Restaurant")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusDto dto)
        {
            try
            {
                var success = await _orderRepository.UpdateOrderStatusAsync(dto);
                if (!success)
                    return NotFound("Order not found.");

                return Ok(new { Message = "Order status updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

    }

}
