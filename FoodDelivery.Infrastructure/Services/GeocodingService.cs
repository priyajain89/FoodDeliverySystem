using FoodDelivery.Infrastructure.DTO;
using FoodDelivery.Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FoodDelivery.Infrastructure.Services
{
    public class GeocodingService : IGeocodingService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        //private readonly string _apiKey = "eyJvcmciOiI1YjNjZTM1OTc4NTExMTAwMDFjZjYyNDgiLCJpZCI6ImQyMmYxZmE5MTljMzQ3ZTliMmI1NDJiNDkzMDg2NWU0IiwiaCI6Im11cm11cjY0In0=";

        public GeocodingService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<GeocodeResponseDto?> GetCoordinatesAsync(string address)
        {

            var _apiKey = _configuration.GetValue<string>("MyAPIKEY:apikey");
            var url = $"https://api.openrouteservice.org/geocode/search?api_key={_apiKey}&text={Uri.EscapeDataString(address)}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var coordinates = doc.RootElement
                .GetProperty("features")[0]
                .GetProperty("geometry")
                .GetProperty("coordinates");

            return new GeocodeResponseDto
            {
                Longitude = coordinates[0].GetDouble(),
                Latitude = coordinates[1].GetDouble()
            };
        }
    }
}
