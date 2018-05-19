﻿using CardinalAppXamarin.Models;
using CardinalAppXamarin.Services.Interfaces;
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

            _layerLast = 0;
            _currentPositionTag = String.Empty;
            _currentPosition = _geolocatorService.LastRecordedPosition;
            MainMapInitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(_currentPosition, 12.0); // zoom can be within: [2,21]
        }

        public MapStyle CustomMapStyle => null;// MapStyle.FromJson(Constants.GoogleMapStyleSilverBlueWater);

        private bool _polygonUsersVisible { get; set; } = false;
        public bool PolygonUsersVisible
        {
            get { return _polygonUsersVisible; }
            set
            {
                _polygonUsersVisible = value;
                RaisePropertyChanged(() => PolygonUsersVisible);
            }
        }

        private ObservableCollection<UserInfoBriefViewCellModel> _selectedUsers { get; set; } = new ObservableCollection<UserInfoBriefViewCellModel>();
        public ObservableCollection<UserInfoBriefViewCellModel> SelectedUsers
        {
            get { return _selectedUsers; }
            set
            {
                _selectedUsers = value;
                RaisePropertyChanged(() => SelectedUsers);
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

        //public MoveCameraRequest MoveCameraRequest { get; } = new MoveCameraRequest();

        private MapSpan _visibleRegion;
        public MapSpan VisibleRegion
        {
            get { return _visibleRegion; }
            set
            {
                _visibleRegion = value;
                SelectedPolygon = null;
                RefreshPolygonUsers();
                RaisePropertyChanged(() => VisibleRegion);
            }
        }

        private CameraUpdate _mainMapInitialCameraUpdate { get; set; }
        public CameraUpdate MainMapInitialCameraUpdate
        {
            get { return _mainMapInitialCameraUpdate; }
            set
            {
                _mainMapInitialCameraUpdate = value;
                RaisePropertyChanged(() => MainMapInitialCameraUpdate);
            }
        }

        //private Position _moveRequestFinalPosition { get; set; }
        public MoveToRegionRequest MoveRequest { get; } = new MoveToRegionRequest();

        public override async Task OnAppearingAsync()
        {
            await _layerService.InitializeData();
            RefreshPolygons();
        }

        private string _currentPositionTag { get; set; }
        private int _layerLast { get; set; }

        private Position _currentPositionValue { get; set; }
        private Position _currentPosition
        {
            get { return _currentPositionValue; }
            set
            {
                _currentPositionValue = value;
                RefreshCurrentPositionTag();
            }
        }

        private void RefreshCurrentPositionTag()
        {
            _hexagonal.Initialize(_currentPosition.Latitude, _currentPosition.Longitude, _hexagonal.Layers.Min());
            var centerPoly = _hexagonal.HexagonalPolygon(_hexagonal.CenterLocation);
            _currentPositionTag = centerPoly.Tag.ToString();
        }

        private void RefreshPolygons()
        {
            if(_visibleRegion == null)
            {
                return;
            }
            double radius = _visibleRegion.Radius.Kilometers;
            //TODO:  set correct radius after user changes VisibleRegion
            //       right now is only calculated from initial view.
            //       and set to one smaller for iphone display purposes.
            radius /= 3;
            int layer = _hexagonal.CalculateLayerFromMapSpan(radius);
            if(layer != _layerLast)
            {
                Polygons.Clear();
                _layerLast = layer;
            }
            _hexagonal.Initialize(_visibleRegion.Center.Latitude, _visibleRegion.Center.Longitude, layer);
            for (int row = -2; row < 3; ++row)
            {
                for (int col = -2; col < 3; ++col)
                {
                    var poly = _hexagonal.HexagonalPolygon(_hexagonal.CenterLocation, col, row);
                    if(Polygons.Any(p => p.Tag.ToString().Equals(poly.Tag.ToString())))
                    {
                        continue;
                    }
                    int heatCount = _layerService.NumberOfUsersInsidePolygonTag(poly.Tag.ToString());
                    poly.FillColor = _heatGradientService.SteppedColor(heatCount);
                    poly.IsClickable = true;
                    poly.Clicked += Polygon_Clicked;
                    if (poly.Tag.ToString().Equals(_currentPositionTag))
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
            //RaisePropertyChanged(() => Polygons);
        }

        private void Polygon_Clicked(object sender, EventArgs e)
        {
            if (sender is Polygon)
            {
                SelectedPolygon = (Polygon)sender;
                RefreshPolygonUsers();
            }
        }

        private void RefreshPolygonUsers()
        {
            if(SelectedPolygon != null)
            {
                var friends = _layerService.UsersInsidePolygonTagBrief(SelectedPolygon.Tag.ToString());
                SelectedUsers = new ObservableCollection<UserInfoBriefViewCellModel>(friends);
                PolygonUsersVisible = SelectedUsers.Count > 0;
            }
            else
            {
                SelectedUsers.Clear();
                PolygonUsersVisible = false;
            }
        }

        private void MapClicked(MapClickedEventArgs args)
        {
            //var position = args.Point;
            //var polygon = new Polygon();
            //polygon.Positions.Add(position);
            //polygon.Positions.Add(new Position(position.Latitude - 0.02d, position.Longitude - 0.01d));
            //polygon.Positions.Add(new Position(position.Latitude - 0.02d, position.Longitude + 0.01d));
            //polygon.Positions.Add(position);

            //polygon.IsClickable = true;
            //polygon.StrokeColor = Color.Green;
            //polygon.StrokeWidth = 3f;
            //polygon.FillColor = Color.FromRgba(255, 0, 0, 64);
            //polygon.Tag = "POLYGON"; // Can set any object
        }

        public Command<MapClickedEventArgs> MapClickedCommand => new Command<MapClickedEventArgs>(MapClicked);
    }
}
