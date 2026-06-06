using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IGeocodingService
    {
        Task<(double lat, double lon)> GetCoordinates(string address);
    }
}
