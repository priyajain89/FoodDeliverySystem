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


    public class DeliveryOrderSummaryDto
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerAddress { get; set; } = string.Empty;
        public string RestaurantName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
    }




}
