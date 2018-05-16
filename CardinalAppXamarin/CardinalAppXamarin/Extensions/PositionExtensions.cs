using Geo = Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using GMaps = Xamarin.Forms.GoogleMaps;
using CardinalLibrary.DataContracts;

namespace CardinalAppXamarin.Extensions
{
    public static class PositionExtensions
    {
        public static Geo.Position GoogleMapsToGeolocator(this GMaps.Position position)
        {
            return new Geo.Position(position.Latitude, position.Longitude);
        }

        public static GMaps.Position GeolocatorToGoogleMaps(this Geo.Position position)
        {
            return new GMaps.Position(position.Latitude, position.Longitude);
        }

        public static CurrentLocationPost GeolocatorToDataContract(this Geo.Position position)
        {
            return new CurrentLocationPost()
            {
                Latitude = position.Latitude,
                Longitude = position.Longitude
            };
        }
    }
}
