using FoodDelivery.Infrastructure.DTO;
using FoodDelivery.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.Infrastructure.Repository
{
    public interface IGeocodingService
    {
        Task<GeocodeResponseDto?> GetCoordinatesAsync(string address);
    }
}
