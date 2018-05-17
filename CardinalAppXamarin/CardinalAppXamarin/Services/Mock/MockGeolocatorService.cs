using CardinalAppXamarin.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace CardinalAppXamarin.Services.Mock
{
    public class MockGeolocatorService : IGeolocatorService
    {
        public async Task<Position> GetCurrentPosition()
        {
            await Task.Delay(10);
            return new Position(30.4031, -97.7345);
        }

        public async Task<bool> IsLocationAvailable()
        {
            await Task.Delay(10);
            return true;
        }
    }
}
