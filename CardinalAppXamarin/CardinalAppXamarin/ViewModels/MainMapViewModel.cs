﻿using CardinalAppXamarin.Services.Interfaces;
using CardinalAppXamarin.ViewModels.Base;
using CardinalLibrary.DataContracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.GoogleMaps.Bindings;

namespace CardinalAppXamarin.ViewModels
{
    public class MainMapViewModel : ViewModelBase
    {
        private readonly IRequestService _requestService;
        private readonly ILayerService _layerService;
        private readonly IGeolocatorService _geolocatorService;
        private readonly IHexagonal _hexagonal;
        private readonly IHeatGradientService _heatGradientService;

        public MainMapViewModel(
            IRequestService requestService,
            ILayerService layerService,
            IGeolocatorService geolocatorService,
            IHexagonal hexagonal,
            IHeatGradientService heatGradientService)
        {
            _requestService = requestService;
            _layerService = layerService;
            _geolocatorService = geolocatorService;
            _hexagonal = hexagonal;
            _heatGradientService = heatGradientService;
            Message = "test label";
        }

        public MapStyle CustomMapStyle => null;// MapStyle.FromJson(Constants.GoogleMapStyleSilverBlueWater);

        private string _message { get; set; }
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                RaisePropertyChanged(() => Message);
            }
        }

        private Polygon _selectedPolygon { get; set; }
        public Polygon SelectedPolygon
        {
            get { return _selectedPolygon; }
            set
            {
                _selectedPolygon = value;
                RaisePropertyChanged(() => SelectedPolygon);
            }
        }

        private ObservableCollection<Polygon> _polygons { get; set; } = new ObservableCollection<Polygon>();
        public ObservableCollection<Polygon> Polygons
        {
            get { return _polygons; }
            set
            {
                _polygons = value;
                RaisePropertyChanged(() => Polygons);
            }
        }

        public Command<MapClickedEventArgs> MapClickedCommand => new Command<MapClickedEventArgs>(
            args =>
            {
                var position = args.Point;
                var polygon = new Polygon();
                polygon.Positions.Add(position);
                polygon.Positions.Add(new Position(position.Latitude - 0.02d, position.Longitude - 0.01d));
                polygon.Positions.Add(new Position(position.Latitude - 0.02d, position.Longitude + 0.01d));
                polygon.Positions.Add(position);

                polygon.IsClickable = true;
                polygon.StrokeColor = Color.Green;
                polygon.StrokeWidth = 3f;
                polygon.FillColor = Color.FromRgba(255, 0, 0, 64);
                polygon.Tag = "POLYGON"; // Can set any object

                Polygons.Add(polygon);
            });

        public MoveCameraRequest MoveCameraRequest { get; } = new MoveCameraRequest();

        private MapSpan _visibleRegion;
        public MapSpan VisibleRegion
        {
            get { return _visibleRegion; }
            set
            {
                _visibleRegion = value;
                RaisePropertyChanged(() => VisibleRegion);
            }
        }

        public MoveToRegionRequest MoveRequest { get; } = new MoveToRegionRequest();

        public override async Task OnAppearingAsync()
        {
            //CustomMapStyle = MapStyle.FromJson(Constants.GoogleMapStyleSilverBlueWater);
            await _layerService.InitializeData();
            var currentPosition = await _geolocatorService.GetCurrentPosition();
            MoveRequest.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                    currentPosition,
                    Distance.FromKilometers(1)));
            int layer = _hexagonal.CalculateLayerFromMapSpan(VisibleRegion.Radius.Kilometers);
            _hexagonal.Initialize(currentPosition.Latitude, currentPosition.Longitude, layer);
            for(int row = -5; row < 6; ++row)
            {
                for(int col = -5; col < 6; ++col)
                {
                    var poly = _hexagonal.HexagonalPolygon(_hexagonal.CenterLocation, col, row);
                    int heatCount = _layerService.NumberOfUsersInsidePolygonTag(poly.Tag.ToString());
                    poly.FillColor = _heatGradientService.SteppedColor(heatCount);
                    poly.IsClickable = true;
                    poly.Clicked += Polygon_Clicked;
                    if (row == 0 && col == 0)
                    {
                        poly.StrokeColor = Color.Coral;
                        poly.StrokeWidth = 5;
                    }
                    else
                    {
                        poly.StrokeColor = _heatGradientService.SteppedColor(heatCount + 1);
                        poly.StrokeWidth = 1;
                    }
                    Polygons.Add(poly);
                }
            }
        }

        private void Polygon_Clicked(object sender, EventArgs e)
        {
            if(sender is Polygon)
            {
                var poly = sender as Polygon;
                var friendCount = _layerService.NumberOfUsersInsidePolygonTag(poly.Tag.ToString());
                Message = poly.Tag.ToString() + ":" + friendCount.ToString();
            }
        }
    }
}
