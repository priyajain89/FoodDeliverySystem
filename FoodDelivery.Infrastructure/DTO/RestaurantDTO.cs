using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.Infrastructure.DTO
{
    public class RestaurantDto
    {
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public bool IsVerified { get; set; }
        public string Role { get; set; } = null!;
        public List<RestaurantResponseDto> SubmittedRestaurants { get; set; } = new();
    }
    public class RestaurantResponseDto
    {
        public int UserId { get; set; }
        public string? Address { get; set; }
        public string? FssaiId { get; set; }
        public int? PinCode { get; set; }
        public string? TradeId { get; set; }

        public IFormFile? FssaiImage { get; set; } // ✅ File input
        public IFormFile? TradelicenseImage{ get; set; } // ✅ File input
    }
    public class RestaurantIDDto
    {
        public int RestaurantId { get; set; }
        public int UserId { get; set; }
        public string? Address { get; set; }
        public string? FssaiId { get; set; }
        public int? PinCode { get; set; }
        public string? FssaiImage { get; set; }
        public string? TradelicenseImage { get; set; }
        public string? TradeId { get; set; }
       
    }
}
