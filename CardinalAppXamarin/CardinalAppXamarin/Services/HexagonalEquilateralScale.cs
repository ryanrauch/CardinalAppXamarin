﻿using CardinalAppXamarin.Services.Interfaces;
using CardinalLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace CardinalAppXamarin.Services
{
    public class HexagonalEquilateralScale : IHexagonal
    {
        private const double ZERORADIUS = 0.005; // 0.005 will create a hexagon with width of 1km (roughly)
        private const double ZEROHALFRADIUS = 0.0025;
        private readonly double ZEROWIDTH = ZERORADIUS + ZEROHALFRADIUS;
        private readonly double ZEROHEIGHT = Math.Sqrt(3) * ZERORADIUS;
        private readonly double ZEROHALFHEIGHT = Math.Sqrt(3) * ZEROHALFRADIUS;

        private int _layer { get; set; }
        private double _flatRadius => _layer * ZERORADIUS;
        private double _flatHalfRadius => _flatRadius / 2;
        private double _flatWidth => _layer * ZEROWIDTH;
        private double _flatHeight => _layer * ZEROHEIGHT;
        private double _flatHalfHeight => _flatHeight / 2;

        private double _latitude { get; set; }
        private double _longitude { get; set; }

        private bool _initialized { get; set; }

        private Position? _centerLocation { get; set; }
        public Position CenterLocation
        {
            get
            {
                if (!_centerLocation.HasValue)
                {
                    double lon = Math.Floor(_longitude / _flatWidth);
                    double lat = Math.Floor(_latitude / _flatHeight);
                    if (lon % 2 == 0)
                    {
                        _centerLocation = new Position(lat * _flatHeight, lon * _flatWidth);
                    }
                    else
                    {
                        _centerLocation = new Position(lat * _flatHeight - _flatHalfHeight, lon * _flatWidth);
                    }
                }
                return _centerLocation.Value;
            }
        }

        Position IHexagonal.ExactLocation => new Position(_latitude, _longitude);

        public IList<int> Layers => new List<int> { 1, 3, 9, 27, 81, 243, 729, 2187 };

        public Polygon HexagonalPolygon(Position center)
        {
            CheckInitialization();
            double lat_top = center.Latitude + _flatHalfHeight;
            double lat_bottom = center.Latitude - _flatHalfHeight;
            double lon_left = center.Longitude - _flatHalfRadius;
            double lon_right = center.Longitude + _flatHalfRadius;
            // positions start with top-left, rotating clockwise
            Polygon poly = new Polygon();
            poly.Positions.Add(new Position(lat_top, lon_left));
            poly.Positions.Add(new Position(lat_top, lon_right));
            poly.Positions.Add(new Position(center.Latitude, center.Longitude + _flatRadius));
            poly.Positions.Add(new Position(lat_bottom, lon_right));
            poly.Positions.Add(new Position(lat_bottom, lon_left));
            poly.Positions.Add(new Position(center.Latitude, center.Longitude - _flatRadius));
            //poly.Tag = CreateTagFromCoordinates(center, _layer);
            poly.Tag = CreatePolygonTag(center, _layer);
            return poly;
        }
        public PolygonTag CreatePolygonTag(Position position, int layer)
        {
            return new PolygonTag()
            {
                PolygonTagType = PolygonTagType.Hexagon,
                Tag = CreateTagFromCoordinates(position, layer)
            };
        }
        public String CreateTagFromCoordinates(Position position, int layer)
        {
            double width = ZEROWIDTH * layer;
            double height = ZEROHEIGHT * layer;
            double lat = 0;
            double lon = position.Longitude / width;
            if (lon % 2 != 0)
                lat = (position.Latitude + height / 2) / height;
            else
                lat = position.Latitude / height;
            String tag = String.Format("{1}{0}{2}{0}{3}",
                                       Constants.BoundingBoxDelim,
                                       layer,
                                       lat,
                                       lon);
            return tag;
        }

        public Polygon HexagonalPolygon(Position center, int column, int row)
        {
            if (column % 2 == 0)
                return HexagonalPolygon(new Position(center.Latitude + row * _flatHeight,
                                                        center.Longitude + column * _flatWidth));
            // if column number is odd, shift down half-height
            return HexagonalPolygon(new Position(center.Latitude + (row * _flatHeight) - _flatHalfHeight,
                                                    center.Longitude + column * _flatWidth));
        }

        public HexagonalEquilateralScale()
        {
            _latitude = 0;
            _longitude = 0;
            _layer = Layers[0];
        }

        public void SetCenter(Position center)
        {
            _latitude = center.Latitude;
            _longitude = center.Longitude;
            _centerLocation = null;
        }

        public void SetLayer(int layer)
        {
            if (!Layers.Contains(layer))
            {
                throw new ArgumentOutOfRangeException(layer.ToString() + " was not contained within Layers");
            }
            _centerLocation = null;
            _layer = layer;
        }

        public void CheckInitialization()
        {
            if (!_initialized)
                throw new InvalidOperationException("HexagonalEquilateralScale has not been initialized properly.");
        }

        public void Initialize(double latitude, double longitude, int layer)
        {
            SetCenter(new Position(latitude, longitude));
            SetLayer(layer);
            _initialized = true;
        }

        public String AllLayersDelimited()
        {
            CheckInitialization();
            String result = String.Empty;
            foreach (int i in Layers)
            {
                SetLayer(i);
                result += CreateTagFromCoordinates(CenterLocation, i) + Constants.LayerDelimChar;
            }
            return result.TrimEnd(Constants.LayerDelimChar);
        }

        public int CalculateLayerFromMapSpan(double radiusKilometers)
        {
            if(radiusKilometers < 1)
            {
                return 1;
            }
            int layer = Convert.ToInt32(Math.Pow(3,Math.Floor(radiusKilometers/3)));
            int layerMax = Layers.Max();
            if(layer > layerMax)
            {
                layer = layerMax;
            }
            return layer;
        }

        public int CalculateLayerFromCameraPositionZoom(double zoom)
        {
            if (zoom > 13)
                return 1;
            else if (zoom > 12.25)
                return 3;
            else if (zoom > 11)
                return 9;
            else if (zoom > 9.25)
                return 27;
            else if (zoom > 8)
                return 81;
            else if (zoom > 6)
                return 243;
            else if (zoom > 4.5)
                return 729;
            return 2187;
        }

        public Polygon PolygonFromDelimited(String delimited)
        {
            var split = delimited.Split(Constants.BoundingBoxDelim);
            if (split.Length != 3)
            {
                throw new ArgumentException(delimited + " could not be split into layer, latitude, and longitude.");
            }
            int layer = Convert.ToInt32(split[0]);
            double lat = Convert.ToDouble(split[1]);
            double lon = Convert.ToDouble(split[2]);

            SetLayer(layer);
            if (lon % 2 == 0)
            {
                return HexagonalPolygon(new Position(lat * _flatHeight,
                                                     lon * _flatWidth));
            }
            else
            {
                // if column number is odd, shift down half-height
                return HexagonalPolygon(new Position(lat * _flatHeight - _flatHalfHeight,
                                                     lon * _flatWidth));
            }
        }
    }
}
