using CoreLocation;
using MapKit;

namespace FlyoverKit.iOS
{
    public static class MKMapPointExtensions
    {
        public static CLLocationCoordinate2D ToCLLocationCoordinate(this MKMapPoint mapPoint)
        {
            return new CLLocationCoordinate2D(mapPoint.X, mapPoint.Y);
        }
    }
}
