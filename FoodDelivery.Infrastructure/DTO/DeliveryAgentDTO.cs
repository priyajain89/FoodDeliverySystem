using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.Infrastructure.DTO
{
    public class DeliveryAgentDTO
    {
        public int UserId { get; set; }
        public IFormFile? DocumentUrl { get; set; }
        public string? Address { get; set; }

    }
        public class DeliveryAgentResponseDto
        {
            public int AgentId { get; set; }
            public int UserId { get; set; }
            public string? DocumentUrl { get; set; }
            public string? Address { get; set; }
        }
    public class DeliveryAgentGetDto
    {
        public int UserId { get; set; }
        public int AgentId { get; set; }
        public string? Address { get; set; }
        public string? DocumentUrl { get; set; }

        // Add these fields from the User entity
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }

}


