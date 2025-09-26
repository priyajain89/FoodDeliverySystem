using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.Infrastructure.DTO
{
    public class AddressAddDto
    {
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PinCode { get; set; }
        public string? Landmark { get; set; }
        public bool? IsDefault { get; set; }
    }
    public class AddressViewDto
    {
        public int AddressId { get; set; }
        public int? UserId { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PinCode { get; set; }
        public string? Landmark { get; set; }
        public bool? IsDefault { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }

}

