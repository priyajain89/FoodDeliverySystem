using FoodDelivery.Domain.Data;
using FoodDelivery.Domain.Models;
using FoodDelivery.Infrastructure.DTO;
using FoodDelivery.Infrastructure.Services;
using Microsoft.AspNetCore.Http;

namespace FoodDelivery.Infrastructure.Repository
{
    public class DeliveryagentRepository : IDeliveryagentRepository
    {
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;

        public DeliveryagentRepository(AppDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

       
        public async Task<DeliveryAgent> SubmitAgentDetailsAsync(DeliveryAgent agent, IFormFile? DocumentUrl)
            {
                var user = await _context.Users.FindAsync(agent.UserId);
                if (user == null || user.Role?.ToLower() != "deliveryagent")
                    throw new InvalidOperationException("Invalid delivery agent user.");

            if (DocumentUrl != null)
                agent.DocumentUrl = await _fileService.SaveFileAsync(DocumentUrl, "DeliveryAgent docs");

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


        public async Task<bool> MarkAgentAvailableAsync(int agentId)
        {
            var agent = await _context.DeliveryAgents.FindAsync(agentId);
            if (agent == null) return false;

            agent.IsAvailable = true;
            _context.DeliveryAgents.Update(agent);
            await _context.SaveChangesAsync();
            return true;
        }


    }
}

