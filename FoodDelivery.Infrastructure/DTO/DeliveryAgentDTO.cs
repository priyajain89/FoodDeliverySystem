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
}


