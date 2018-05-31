using System;

namespace FlyoverKit.iOS
{
    public static class FlyoverExtensions
    {
        public static bool NearlyEquals(this Flyover lhs, Flyover rhs)
        {
            if (rhs == null
               || !lhs.Coordinate.IsValid()
               || !rhs.Coordinate.IsValid())
            {
                return false;
            }
            double factor = 1000.0;
            return Math.Round(lhs.Coordinate.Latitude * factor) == Math.Round(rhs.Coordinate.Latitude * factor)
                && Math.Round(lhs.Coordinate.Longitude * factor) == Math.Round(rhs.Coordinate.Longitude * factor);
        }
    }
}
