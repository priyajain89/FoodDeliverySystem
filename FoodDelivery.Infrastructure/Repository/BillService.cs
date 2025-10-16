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

    public class BillService : IBillService

    {

        private readonly AppDbContext _context;

        public BillService(AppDbContext context)

        {

            _context = context;

        }

        public async Task<BillDto> GenerateBillFromOrderAsync(int orderId)

        {

            var order = await _context.Orders

            .Include(o => o.OrderItems)

            .ThenInclude(oi => oi.Item)

            .Include(o => o.Address)

            .Include(o => o.Restaurant)

                .ThenInclude(r => r.User)

            .Include(o => o.Agent)

                .ThenInclude(a => a.User)

            .FirstOrDefaultAsync(o => o.OrderId == orderId);


            if (order == null || order.Address == null || order.Restaurant == null)

                throw new Exception("Invalid order or missing address/restaurant.");

            double distanceKm = CalculateDistance(

                order.Address.Latitude ?? 0, order.Address.Longitude ?? 0,

                order.Restaurant.Latitude ?? 0, order.Restaurant.Longitude ?? 0);

            double estimatedTimeMinutes = (distanceKm / 30) * 60;

            double deliveryCharge = Math.Round(distanceKm * 3, 2);

            var items = new List<OrderedItemDto>();

            double itemsTotal = 0;

            foreach (var orderItem in order.OrderItems)

            {

                var itemTotal = (double)(orderItem.Quantity ?? 0) * (double)(orderItem.Price ?? 0);

                itemsTotal += itemTotal;

                items.Add(new OrderedItemDto

                {

                    ItemName = orderItem.Item?.Name ?? "Unknown",

                    Quantity = orderItem.Quantity ?? 0,

                    Price = (double)(orderItem.Price ?? 0),

                    Total = itemTotal

                });

            }

            return new BillDto

            {

                AgentName = order.Agent.User?.Name ?? "Unknown",

                RestaurantName = order.Restaurant.User?.Name ?? "Unknown",

                DistanceKm = Math.Round(distanceKm, 2),

                EstimatedTimeMinutes = Math.Round(estimatedTimeMinutes, 2),

                DeliveryCharge = deliveryCharge,

                Items = items,

                ItemsTotal = itemsTotal,

                GrandTotal = itemsTotal + deliveryCharge

            };

        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)

        {

            double R = 6371;

            double dLat = Math.PI / 180 * (lat2 - lat1);

            double dLon = Math.PI / 180 * (lon2 - lon1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +

                       Math.Cos(Math.PI / 180 * lat1) * Math.Cos(Math.PI / 180 * lat2) *

                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;

        }


        public async Task<DeliveryAgent?> AssignNearestAgentAsync(Order order)

        {

            var restaurantLat = order.Restaurant.Latitude ?? 0;

            var restaurantLon = order.Restaurant.Longitude ?? 0;

            var availableAgents = await _context.DeliveryAgents

                .Include(a => a.User)

                .Where(a => a.IsAvailable == true && a.Latitude != null && a.Longitude != null)

                .ToListAsync();

            var nearestAgent = availableAgents

                .Select(agent => new

                {

                    Agent = agent,

                    Distance = CalculateDistance(

                        restaurantLat, restaurantLon,

                        agent.Latitude.Value, agent.Longitude.Value)

                })

                .OrderBy(x => x.Distance)

                .FirstOrDefault()?.Agent;

            if (nearestAgent != null)

            {

                nearestAgent.IsAvailable = false;

                order.AgentId = nearestAgent.AgentId;

                await _context.SaveChangesAsync();

            }

            return nearestAgent;

        }


    }

}

