using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services
{
    public class GeocodingService
    {
        private readonly HttpClient _httpClient;

        public GeocodingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(double lat, double lon)> GetCoordinates(string address)
        {
            var url =
                $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(address)}&format=json&limit=1";

            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("MiConsorcioApp/1.0");

            var response = await _httpClient.GetStringAsync(url);

            var data = JsonSerializer.Deserialize<List<NominatimResponse>>(response);

            if (data == null || data.Count == 0)
                throw new Exception("Dirección no encontrada");

            double lat = double.Parse(
                data[0].lat.Trim().Replace(",", "."),
                CultureInfo.InvariantCulture
            );

            double lon = double.Parse(
                data[0].lon.Trim().Replace(",", "."),
                CultureInfo.InvariantCulture
            );

            return (lat, lon);
        }


        public class NominatimResponse
        {
            public string lat { get; set; }
            public string lon { get; set; }
        }
    }
}