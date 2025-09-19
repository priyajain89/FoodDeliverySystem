using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.Infrastructure.DTO
{
    public class GeocodeRequestDto
    {
        public string Address { get; set; } = string.Empty;
    }

    public class GeocodeResponseDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
