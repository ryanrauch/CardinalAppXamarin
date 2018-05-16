﻿using System;
using System.Collections.Generic;
using Xamarin.Forms.GoogleMaps;

namespace CardinalAppXamarin.Services.Interfaces
{
    public interface IHexagonal
    {
        IList<int> Layers { get; }
        Position CenterLocation { get; }
        Position ExactLocation { get; }
        Polygon HexagonalPolygon(Position center);
        Polygon HexagonalPolygon(Position center, int column, int row);
        void SetCenter(Position center);
        void SetLayer(int layer);
        void Initialize(double latitude, double longitude, int layer);
        String AllLayersDelimited();
        int CalculateLayerFromCameraPositionZoom(double zoom);
        int CalculateLayerFromMapSpan(double radiusKilometers);
        Polygon PolygonFromDelimited(String delimited);
    }
}
