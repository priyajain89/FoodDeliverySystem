using FoodDelivery.Domain.Data;
using FoodDelivery.Domain.Models;

namespace FoodDelivery.Infrastructure.Repository
{
    public class Deliveryagent : IDeliveryagent
    {
        private readonly AppDbContext _context;

        public Deliveryagent(AppDbContext context)
        {
            _context = context;
        }

       
            public async Task<DeliveryAgent> SubmitAgentDetailsAsync(DeliveryAgent agent)
            {
                var user = await _context.Users.FindAsync(agent.UserId);
                if (user == null || user.Role?.ToLower() != "deliveryagent")
                    throw new InvalidOperationException("Invalid delivery agent user.");

                _context.DeliveryAgents.Add(agent);
                await _context.SaveChangesAsync();
                return agent;
            }
        }
    }

