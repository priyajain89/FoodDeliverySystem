using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.Infrastructure.DTO
{
    public class VerifyUserDto
    {
        public int UserId { get; set; }
        public string Role { get; set; } = null!;
    }

    public class PendingRestaurantDto
    {
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public bool IsVerified { get; set; }
        public string Role { get; set; } = null!;
        public List<RestaurantDto> SubmittedRestaurants { get; set; } = new();
    }
}
