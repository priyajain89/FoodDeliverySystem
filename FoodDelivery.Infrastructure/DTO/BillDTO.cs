using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.Infrastructure.DTO
{
    public class BillDto
    {
        public double DistanceKm { get; set; }
        public double EstimatedTimeMinutes { get; set; }
        public double DeliveryCharge { get; set; }
        public List<OrderedItemDto> Items { get; set; }
        public double ItemsTotal { get; set; }     // Sum of all item totals
        public double GrandTotal { get; set; }     // ItemsTotal + DeliveryCharge
    }


    public class OrderedItemDto
    {
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Total { get; set; } // Quantity × Price
    }
}
