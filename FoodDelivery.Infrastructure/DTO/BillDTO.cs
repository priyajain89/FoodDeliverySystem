using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.Infrastructure.DTO
{
    public class BillDto
    {
        public string AgentName { get; set; } = string.Empty;
        public string RestaurantName { get; set; } = string.Empty;
        public double DistanceKm { get; set; }
        public double EstimatedTimeMinutes { get; set; }
        public double DeliveryCharge { get; set; }
        public List<OrderedItemDto> Items { get; set; }
        public double ItemsTotal { get; set; }    
        public double GrandTotal { get; set; }     
    }
    public class OrderedItemDto
    {
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Total { get; set; } 
    }
}
