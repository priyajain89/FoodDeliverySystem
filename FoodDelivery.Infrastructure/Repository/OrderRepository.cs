using FoodDelivery.Domain.Data;

using FoodDelivery.Domain.Models;

using FoodDelivery.Infrastructure.DTO;

using Microsoft.EntityFrameworkCore;

using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

using System.Threading.Tasks;

namespace FoodDelivery.Infrastructure.Repository

{

    public class OrderRepository : IOrderRepository

    {

        private readonly AppDbContext _context;

        private readonly IGeocodingService _geocodingService;

        private readonly IAddressRepository _addressRepo;

        public OrderRepository(IGeocodingService geocodingService, AppDbContext context, IAddressRepository addressRepo)

        {

            _context = context;

            _geocodingService = geocodingService;

            _addressRepo = addressRepo;

        }

        public async Task<int> CreateOrderFromCartAsync(int customerId, CreateOrderFromCartDto dto)

        {

            var cart = await _context.Carts

                .Include(c => c.CartItems)

                    .ThenInclude(ci => ci.Item)

                .FirstOrDefaultAsync(c => c.CartId == dto.CartId && c.UserId == customerId);

            if (cart == null || !cart.CartItems.Any())

                throw new Exception("Cart is empty or not found.");

            var order = new Order

            {

                UserId = customerId,

                RestaurantId = cart.CartItems.First().Item?.RestaurantId,

                OrderDate = DateTime.UtcNow,

                Status = "PendingAddress",

                TotalAmount = cart.CartItems.Sum(i => i.Quantity * (i.Item?.Price ?? 0))

            };

            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            foreach (var item in cart.CartItems)

            {

                _context.OrderItems.Add(new OrderItem

                {

                    OrderId = order.OrderId,

                    ItemId = item.ItemId ?? 0,

                    Quantity = item.Quantity ?? 1,

                    Price = item.Item?.Price ?? 0

                });

            }

            _context.CartItems.RemoveRange(cart.CartItems);

            await _context.SaveChangesAsync();

            return order.OrderId;

        }

        public async Task<bool> AssignAddressToOrderAsync(AssignAddressToOrderDto dto)

        {

            var order = await _context.Orders.FindAsync(dto.OrderId);

            if (order == null) return false;

            order.AddressId = dto.AddressId;

            order.Status = "place order";

            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)

        {

            return await _context.Orders

                .Include(o => o.Restaurant)

                .Include(o => o.Address)

                .Include(o => o.Agent)

                .FirstOrDefaultAsync(o => o.OrderId == orderId);

        }

        public async Task UpdateOrderAsync(Order order)

        {

            _context.Orders.Update(order);

            await _context.SaveChangesAsync();

        }


        public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId)
        {
            return await _context.Orders
                .Include(o => o.Restaurant)
                 .ThenInclude(r => r.User)
                 //.Where(o => o.UserId == customerId && o.Status == "Delivered")
                  .Where(o => o.UserId == customerId )
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<DeliveryOrderSummaryDto>> GetOrdersForAgentAsync(int agentId)

        {

            var orders = await _context.Orders

                .Include(o => o.User) // Customer

                .Include(o => o.Restaurant)

                    .ThenInclude(r => r.User) // Restaurant Owner

                .Where(o => o.AgentId == agentId)

                .OrderByDescending(o => o.OrderDate)

                .ToListAsync();

            var result = new List<DeliveryOrderSummaryDto>();

            foreach (var order in orders)

            {

                string fullAddress = "Unknown";

                if (order.AddressId.HasValue && order.UserId != 0)

                {

                    var addressDto = await _addressRepo.GetByUserIdForCustomerAsync(order.AddressId.Value, order.UserId.Value);

                    if (addressDto != null)

                    {

                        fullAddress = $"{addressDto.AddressLine1}, {addressDto.AddressLine2}, {addressDto.Landmark}, {addressDto.City}, {addressDto.State}, {addressDto.PinCode}";

                    }

                }

                result.Add(new DeliveryOrderSummaryDto

                {

                    OrderId = order.OrderId,

                    CustomerName = order.User?.Name ?? "Unknown",

                    CustomerAddress = fullAddress,

                    RestaurantName = order.Restaurant?.User?.Name ?? "Unknown",

                    Status = order.Status,

                });

            }

            return result;

        }

        public async Task<IEnumerable<RestaurantOrderViewDto>> GetOrdersForRestaurantAsync(int restaurantId)
        {
            var orders = await _context.Orders
                .Include(o => o.User) // Customer
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Item)
                .Include(o => o.Agent)
                    .ThenInclude(a => a.User)
                .Where(o => o.RestaurantId == restaurantId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var result = new List<RestaurantOrderViewDto>();

            foreach (var order in orders)
            {
                var orderedItems = order.OrderItems.Select(oi => new OrderedItemDto
                {
                    ItemName = oi.Item?.Name ?? "Unknown",
                    Quantity = oi.Quantity ?? 0,
                    Price = (double)(oi.Price ?? 0),
                    Total = (oi.Quantity ?? 0) * (double)(oi.Price ?? 0)
                }).ToList();

                result.Add(new RestaurantOrderViewDto
                {
                    OrderId = order.OrderId,
                    CustomerName = order.User?.Name ?? "Unknown",
                    OrderedItems = orderedItems,
                    GrandTotal = (double)(order.TotalAmount ?? 0),
                    OrderDateTime = order.OrderDate ?? DateTime.MinValue,
                    Status = order.Status,
                    DeliveryAgentName = order.Agent?.User?.Name ?? "Not Assigned"
                });
            }

            return result;
        }


        public async Task<bool> UpdateOrderStatusAsync(UpdateOrderStatusDto dto)
        {
            var order = await _context.Orders.FindAsync(dto.OrderId);
            if (order == null) return false;

            order.Status = dto.NewStatus;
            await _context.SaveChangesAsync();
            return true;
        }



    }

}

