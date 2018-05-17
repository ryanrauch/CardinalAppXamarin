using CardinalAppXamarin.Services.Interfaces;
using Xamarin.Forms.GoogleMaps;

namespace CardinalAppXamarin.Controls
{
    public class CustomGoogleMap : Map
    {
        private readonly IGeolocatorService _geolocatorService;

        public CustomGoogleMap(IGeolocatorService geolocatorService)
        {
            _geolocatorService = geolocatorService;

            //var location = new Position(30.4, -97.7);
            var location = _geolocatorService.LastRecordedPosition;
            var cf = CameraUpdateFactory.NewPositionZoom(location, 13.0);
            this.InitialCameraUpdate = cf;
        }
        
    }
}
