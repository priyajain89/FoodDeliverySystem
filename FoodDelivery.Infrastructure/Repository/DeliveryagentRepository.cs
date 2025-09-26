using FoodDelivery.Domain.Data;
using FoodDelivery.Domain.Models;
using FoodDelivery.Infrastructure.DTO;

namespace FoodDelivery.Infrastructure.Repository
{
    public class DeliveryagentRepository : IDeliveryagentRepository
    {
        private readonly AppDbContext _context;

        public DeliveryagentRepository(AppDbContext context)
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

        public async Task<bool> UpdateDeliveryAgentAsync(DeliveryAgentResponseDto dto)
        {
            var agent = await _context.DeliveryAgents.FindAsync(dto.AgentId);
            if (agent == null) return false;

            agent.UserId = dto.UserId;
            agent.DocumentUrl = dto.DocumentUrl;
            agent.Address = dto.Address;

            _context.DeliveryAgents.Update(agent);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}

