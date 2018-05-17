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
            MainMapInitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(_currentPosition, 12.0);
        }

        public MapStyle CustomMapStyle => null;// MapStyle.FromJson(Constants.GoogleMapStyleSilverBlueWater);

        public bool PolygonUsersVisible => SelectedPolygon != null && SelectedUsers.Count > 0;

        private ObservableCollection<UserInfoContract> _selectedUsers { get; set; } = new ObservableCollection<UserInfoContract>();
        public ObservableCollection<UserInfoContract> SelectedUsers
        {
            get { return _selectedUsers; }
            set
            {
                _selectedUsers = value;
                RaisePropertyChanged(() => SelectedUsers);
                RaisePropertyChanged(() => PolygonUsersVisible);
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
                //RaisePropertyChanged(() => PolygonUsersVisible);
                RefreshPolygonUsers();//.ConfigureAwait(true);
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

        //public Command<MapClickedEventArgs> MapClickedCommand => new Command<MapClickedEventArgs>(
        //    args =>
        //    {
        //        var position = args.Point;
        //        var polygon = new Polygon();
        //        polygon.Positions.Add(position);
        //        polygon.Positions.Add(new Position(position.Latitude - 0.02d, position.Longitude - 0.01d));
        //        polygon.Positions.Add(new Position(position.Latitude - 0.02d, position.Longitude + 0.01d));
        //        polygon.Positions.Add(position);

        //        polygon.IsClickable = true;
        //        polygon.StrokeColor = Color.Green;
        //        polygon.StrokeWidth = 3f;
        //        polygon.FillColor = Color.FromRgba(255, 0, 0, 64);
        //        polygon.Tag = "POLYGON"; // Can set any object

        //        Polygons.Add(polygon);
        //    });

        public MoveCameraRequest MoveCameraRequest { get; } = new MoveCameraRequest();

        private MapSpan _visibleRegion;
        public MapSpan VisibleRegion
        {
            get { return _visibleRegion; }
            set
            {
                _visibleRegion = value;
                RaisePropertyChanged(() => VisibleRegion);
                //if (!MainMapVisible
                //    && _visibleRegion != null
                //    && _moveRequestFinalPosition != null
                //    && AreLocationsSameWithTolerance(_visibleRegion.Center, _moveRequestFinalPosition, 0.0001))
                //{
                //    MainMapVisible = true;
                //}
                //if (_visibleRegion != null)
                //{
                //    RefreshPolygons();//.ConfigureAwait(false);
                //}
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

        private Position _moveRequestFinalPosition { get; set; }
        public MoveToRegionRequest MoveRequest { get; } = new MoveToRegionRequest();

        //private bool _mainMapVisible { get; set; } = true; //= false;
        //public bool MainMapVisible
        //{
        //    get { return _mainMapVisible; }
        //    set
        //    {
        //        _mainMapVisible = value;
        //        RaisePropertyChanged(() => MainMapVisible);
        //    }
        //}

        private bool AreLocationsSameWithTolerance(Position attempt, Position exact, double tolerance)
        {
            bool lat = attempt.Latitude.Equals(exact.Latitude)
                       || (attempt.Latitude <= exact.Latitude + tolerance
                           && attempt.Latitude >= exact.Latitude - tolerance);
            bool lon = attempt.Longitude.Equals(exact.Longitude)
                       || (attempt.Longitude <= exact.Longitude + tolerance
                           && attempt.Longitude >= exact.Longitude - tolerance);
            return lat && lon;
        }

        public override async Task OnAppearingAsync()
        {
            await _layerService.InitializeData();
            //_currentPosition = await _geolocatorService.GetCurrentPosition();
            //_moveRequestFinalPosition = _currentPosition;
            //MoveRequest.MoveToRegion(
            //    MapSpan.FromCenterAndRadius(
            //        _currentPosition,
            //        Distance.FromKilometers(1)));
            await RefreshPolygons();
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

        private async Task RefreshPolygons()
        {
            if(_visibleRegion == null)
            {
                return;
            }
            int layer = _hexagonal.CalculateLayerFromMapSpan(_visibleRegion.Radius.Kilometers);
            if(layer != _layerLast)
            {
                Polygons.Clear();
                _layerLast = layer;
            }
            _hexagonal.Initialize(_visibleRegion.Center.Latitude, _visibleRegion.Center.Longitude, layer);
            //ObservableCollection<Polygon> freshPolygons = new ObservableCollection<Polygon>();
            for (int row = -5; row < 6; ++row)
            {
                for (int col = -5; col < 6; ++col)
                {
                    var poly = _hexagonal.HexagonalPolygon(_hexagonal.CenterLocation, col, row);
                    if(Polygons.Any(p => p.Tag.ToString().Equals(poly.Tag.ToString())))
                    {
                        continue;
                    }
                    int heatCount = await _layerService.NumberOfUsersInsidePolygonTag(poly.Tag.ToString());
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
            RaisePropertyChanged(() => Polygons);
        }

        private async Task RefreshPolygonUsers()
        {
            if(SelectedPolygon != null)
            {
                var friends = await _layerService.UsersInsidePolygonTag(SelectedPolygon.Tag.ToString());
                SelectedUsers = new ObservableCollection<UserInfoContract>(friends);
            }
            else
            {
                SelectedUsers.Clear();
            }
        }

        private void Polygon_Clicked(object sender, EventArgs e)
        {
            if(sender is Polygon)
            {
                var poly = sender as Polygon;
                SelectedPolygon = poly;
            }
        }
    }
}
