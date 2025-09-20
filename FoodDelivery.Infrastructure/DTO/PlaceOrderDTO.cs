using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.Infrastructure.DTO
{
    public class PlaceOrderDto
    {
        public int CustomerId { get; set; }
        public int AddressId { get; set; }
        public int CartId { get; set; }
        public string PaymentMethod { get; set; } // e.g., "COD", "Online"
    }
    public class CreateOrderFromCartDto
    {
        public int CartId { get; set; }
    }

    public class AssignAddressToOrderDto
    {
        public int OrderId { get; set; }
        public int AddressId { get; set; }
    }



}
