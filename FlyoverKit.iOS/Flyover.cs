using CoreLocation;
using MapKit;
using System;

namespace FlyoverKit.iOS
{
    public class Flyover : IEquatable<Flyover>
    {
        public CLLocationCoordinate2D Coordinate { get; set; }

        public Flyover(CLLocationCoordinate2D locationCoordinate)
        {
            Coordinate = locationCoordinate;
        }

        public Flyover(CLCircularRegion circularRegion)
        {
            Coordinate = circularRegion.Center;
        }

        public Flyover(CLLocation location)
        {
            Coordinate = location.Coordinate;
        }

        public Flyover(FlyoverAwesomePlace place)
        {
            switch (place)
            {
                case FlyoverAwesomePlace.NewYorkStatueOfLiberty:
                    Coordinate = new CLLocationCoordinate2D(40.689249, -74.044500);
                    break;
                case FlyoverAwesomePlace.NewYork:
                    Coordinate = new CLLocationCoordinate2D(40.702749, -74.014120);
                    break;
                case FlyoverAwesomePlace.SanFranciscoGoldenGateBridge:
                    Coordinate = new CLLocationCoordinate2D(37.826040, -122.479448);
                    break;
                default:
                    Coordinate = new CLLocationCoordinate2D(40.689249, -74.044500);
                    break;
            }
        }

        public Flyover(MKMapItem mapItem)
        {
            Coordinate = mapItem.Placemark.Coordinate;
        }

        public Flyover(MKMapView mapView)
        {
            Coordinate = mapView.CenterCoordinate;
        }

        public Flyover(MKMapPoint mapPoint)
        {
            Coordinate = mapPoint.ToCLLocationCoordinate();
        }

        public Flyover(MKCoordinateRegion coordinateRegion)
        {
            Coordinate = coordinateRegion.Center;
        }

        public Flyover(MKMapRect mapRect)
        {
            Coordinate = mapRect.Origin.ToCLLocationCoordinate();
        }

        public Flyover(MKCoordinateSpan coordinateSpan)
        {
            Coordinate = new CLLocationCoordinate2D(coordinateSpan.LatitudeDelta, coordinateSpan.LongitudeDelta);
        }

        public Flyover(MKMapCamera mapCamera)
        {
            Coordinate = mapCamera.CenterCoordinate;
        }

        public Flyover(MKShape shape)
        {
            Coordinate = shape.Coordinate;
        }

        public Flyover(MKPlacemark placemark)
        {
            Coordinate = placemark.Coordinate;
        }

        public static bool operator ==(Flyover lhs, Flyover rhs)
        {
            if (ReferenceEquals(lhs, null))
            {
                return ReferenceEquals(rhs, null);
            }
            if (ReferenceEquals(rhs, null))
            {
                return ReferenceEquals(lhs, null);
            }
            return lhs?.Coordinate.Latitude == rhs?.Coordinate.Latitude
                && lhs?.Coordinate.Longitude == rhs?.Coordinate.Longitude;
        }
        public static bool operator !=(Flyover lhs, Flyover rhs)
        {
            return !(lhs == rhs);
        }

        public bool Equals(Flyover other)
        {
            return Coordinate.Latitude == other?.Coordinate.Latitude
                && Coordinate.Longitude == other?.Coordinate.Longitude;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            var f = (Flyover)obj;
            return Coordinate.Latitude == f.Coordinate.Latitude
                && Coordinate.Longitude == f.Coordinate.Longitude;
        }

        public override int GetHashCode()
        {
            return Coordinate.Latitude.GetHashCode()
                 ^ Coordinate.Longitude.GetHashCode();
        }
    }
}