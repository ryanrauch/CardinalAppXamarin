using CardinalLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace CardinalAppXamarin.Extensions
{
    public static class PolygonExtensions
    {
        public static PolygonTag ExtractPolygonTag(this Polygon poly)
        {
            if(poly.Tag is PolygonTag pt)
            {
                return pt;
            }
            return null;
        }

        public static bool PolygonTagEquals(this Polygon left, Polygon right)
        {
            if(left.Tag is PolygonTag lt)
            {
                if(right.Tag is PolygonTag rt)
                {
                    return lt.PolygonTagType.Equals(rt.PolygonTagType)
                           && lt.Tag.Equals(rt.Tag);
                }
            }
            return false;
        }

        public static bool PolygonTagEquals(this Polygon poly, PolygonTagType tagType, string str)
        {
            return poly.PolygonTagTypeEquals(tagType)
                   && poly.PolygonTagStringEquals(str);
        }

        public static bool PolygonTagTypeEquals(this Polygon poly, PolygonTagType tagType)
        {
            if(poly.Tag is PolygonTag pt)
            {
                return pt.PolygonTagType.Equals(tagType);
            }
            return false;
        }

        public static bool PolygonTagStringEquals(this Polygon poly, string str)
        {
            if(poly.Tag is PolygonTag pt)
            {
                return pt.Tag.Equals(str);
            }
            return false;
        }
    }
}
