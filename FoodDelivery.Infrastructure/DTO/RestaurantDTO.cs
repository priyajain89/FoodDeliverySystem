using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.Infrastructure.DTO
{
    public class RestaurantDto
    {
        // User Info
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public bool IsVerified { get; set; }
        public string Role { get; set; } = null!;

        // Restaurant Info
        public List<RestaurantResponseDto> SubmittedRestaurants { get; set; } = new();
    }


    public class RestaurantResponseDto
    {
        
        public int UserId { get; set; }
        public string? Address { get; set; }
        public string? FssaiId { get; set; }
        public int? PinCode { get; set; }
        public string? FssaiImage { get; set; }
        public string? TradelicenseImage { get; set; }
        public string? TradeId { get; set; }
        
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
