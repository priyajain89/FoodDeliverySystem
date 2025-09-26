using FoodDelivery.Domain.Data;
using FoodDelivery.Domain.Models;
using FoodDelivery.Infrastructure.DTO;
using FoodDelivery.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDelivery.Infrastructure.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        private readonly IGeocodingService _geocodingService;

        public OrderRepository(IGeocodingService geocodingService, AppDbContext context)
        {
            _context = context;
            _geocodingService = geocodingService;
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
                Status = OrderEnums.OrderStatus.Pending.ToString(), // Enum used as string
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
            order.Status = OrderEnums.OrderStatus.ReadyForPickup.ToString(); // Enum used as string

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
    }
}
