using CardinalAppXamarin.Extensions;
using CardinalAppXamarin.Models;
using CardinalAppXamarin.Services.Interfaces;
using CardinalAppXamarin.ViewModels.Base;
using CardinalLibrary;
using CardinalLibrary.DataContracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
        private readonly INavigationService _navigationService;
        private readonly IHexagonal _hexagonal;
        private readonly IHeatGradientService _heatGradientService;
        private readonly IZoneService _zoneService;

        public MainMapViewModel(
            IRequestService requestService,
            ILayerService layerService,
            IGeolocatorService geolocatorService,
            INavigationService navigationService,
            IHexagonal hexagonal,
            IHeatGradientService heatGradientService,
            IZoneService zoneService)
        {
            _requestService = requestService;
            _layerService = layerService;
            _geolocatorService = geolocatorService;
            _navigationService = navigationService;
            _hexagonal = hexagonal;
            _heatGradientService = heatGradientService;
            _zoneService = zoneService;

            _layerLast = 0;
            _currentPositionTag = String.Empty;
            _currentPosition = _geolocatorService.LastRecordedPosition;
            //MainMapInitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(_currentPosition, 12.0); // zoom can be within: [2,21]
            CameraPosition cp = new CameraPosition(_currentPosition, 12.0, 0.0, 60.0);
            MainMapInitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(cp);
            _initialized = false;
        }

        public ICommand ZonesCommand => new Command(ZonesClicked);
        public ICommand ProfileCommand => new Command(ProfileClicked);
        public ICommand SettingsCommand => new Command(SettingsClicked);
        public ICommand FriendsCommand => new Command(FriendsClicked);
        public Command<CameraIdledEventArgs> MapCameraIdled => new Command<CameraIdledEventArgs>(async (a) => await CameraIdled(a));
        public Command<MapClickedEventArgs> MapClickedCommand => new Command<MapClickedEventArgs>(MapClicked);

        public ICommand MapTypeCommand => new Command<string>(MapTypeClicked);

        public MapStyle CustomMapStyle => null;// MapStyle.FromJson(Constants.GoogleMapStyleSilverBlueWater);

        private MapType _customMapType { get; set; } = MapType.Street;
        public MapType CustomMapType
        {
            get { return _customMapType; }
            set
            {
                _customMapType = value;
                RaisePropertyChanged(() => CustomMapType);
            }
        }

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

        private bool _initialized { get; set; }

        public override async Task OnAppearingAsync()
        {
            var profile = await _requestService.GetAsync<UserInfoContract>("api/UserInfoSelf");
            UserProfile = new ProfileViewModel() { UserInfo = profile };

            Task[] tasks = new Task[] { _layerService.InitializeData(), _zoneService.InitializeData() };
            //await _layerService.InitializeData();
            //await _zoneService.InitializeData();
            await Task.WhenAll(tasks);
            await RefreshPolygonsAsync();
            _initialized = true;
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

        private bool _settingsVisible { get; set; } = false;

        private void SettingsClicked()
        {
            //TODO: Show settings view
            if(_settingsVisible)
            {
                
            }
            else
            {

            }
            _settingsVisible = !_settingsVisible;
        }

        private void MapTypeClicked(string str)
        {
            switch(str)
            {
                case "Satellite":
                    CustomMapType = MapType.Satellite;
                    break;
                case "Street":
                    CustomMapType = MapType.Street;
                    break;
                case "Terrain":
                    CustomMapType = MapType.Terrain;
                    break;
                case "Hybrid":
                    CustomMapType = MapType.Hybrid;
                    break;
                default:
                    break;
            }
        }

        private void ZonesClicked()
        {
            _navigationService.NavigateToZones();
        }

        private void FriendsClicked()
        {

        }

        private ProfileViewModel _userProfile { get; set; }
        public ProfileViewModel UserProfile
        {
            get { return _userProfile; }
            set
            {
                _userProfile = value;
                RaisePropertyChanged(() => UserProfile);
            }
        }

        private bool _profileVisible { get; set; } = false;
        public bool ProfileVisible
        {
            get { return _profileVisible; }
            set
            {
                _profileVisible = value;
                RaisePropertyChanged(() => ProfileVisible);
            }
        }

        private void ProfileClicked()
        {
            ProfileVisible = !ProfileVisible;
        }

        private double _currentCameraZoom { get; set; }
        private CameraIdledEventArgs _currentCameraIdledEventArgs {get;set;}

        private async Task CameraIdled(CameraIdledEventArgs args)
        {
            _currentCameraIdledEventArgs = args;
            double zoom = args.Position.Zoom;
            //_currentCameraZoom = zoom;
            DebugLabel = zoom.ToString();
            if (_initialized)
            {
                await RefreshPolygonsAsync();
            }
        }

        private string _debugLabel { get; set; } = string.Empty;
        public string DebugLabel
        {
            get
            {
                return _debugLabel;
            }
            set
            {
                _debugLabel = value;
                RaisePropertyChanged(() => DebugLabel);
            }
        }

        private void RefreshCurrentPositionTag()
        {
            _hexagonal.Initialize(_currentPosition.Latitude, _currentPosition.Longitude, _hexagonal.Layers.Min());
            var centerPoly = _hexagonal.HexagonalPolygon(_hexagonal.CenterLocation);
            _currentPositionTag = centerPoly.ExtractPolygonTag().Tag;
        }
        
        private async Task RefreshZonesAsync()
        {
            var zones = await _zoneService.GetAllZoneContractsAsync();
            foreach(var zc in zones)
            {
                Polygon poly = new Polygon()
                {
                    FillColor = Color.FromHex(zc.ARGBFill),
                    StrokeColor = Color.FromHex(zc.ARGBFill),
                    StrokeWidth = 1.0f,
                    Tag = new PolygonTag()
                                        {
                                            PolygonTagType = PolygonTagType.Zone,
                                            Tag = zc.Description
                                        }
                };
                foreach (var shape in zc.ZoneShapes.Where(z => z.Order > 0).OrderBy(z=>z.Order))
                {
                    poly.Positions.Add(new Position(shape.Latitude, shape.Longitude));
                }
                Polygons.Add(poly);
            }
        }

        private async Task RefreshPolygonsAsync()
        {
            if(_visibleRegion == null)
            {
                return;
            }
            int layer = _hexagonal.CalculateLayerFromCameraPositionZoom(_currentCameraIdledEventArgs.Position.Zoom);
            if(layer != _layerLast)
            {
                Polygons.Clear();
                _layerLast = layer;
            }
            _hexagonal.Initialize(_currentCameraIdledEventArgs.Position.Target.Latitude, _currentCameraIdledEventArgs.Position.Target.Longitude, layer);
            for (int row = -2; row < 3; ++row)
            {
                for (int col = -2; col < 3; ++col)
                {
                    var poly = _hexagonal.HexagonalPolygon(_hexagonal.CenterLocation, col, row);
                    if(Polygons.Any(p => p.PolygonTagEquals(poly)))
                    {
                        continue;
                    }
                    int heatCount = _layerService.NumberOfUsersInsidePolygonTag(poly.ExtractPolygonTag().Tag);
                    poly.FillColor = _heatGradientService.SteppedColor(heatCount);
                    poly.IsClickable = true;
                    poly.Clicked += Polygon_Clicked;
                    if (poly.PolygonTagStringEquals(_currentPositionTag))
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
            await RefreshZonesAsync();
        }

        private void Polygon_Clicked(object sender, EventArgs e)
        {
            if (sender is Polygon sp)
            {
                if (SelectedPolygon != null 
                    && SelectedPolygon.PolygonTagEquals(sp))
                {
                    SelectedPolygon = null;
                    PolygonUsersVisible = false;
                }
                else
                {
                    SelectedPolygon = sp;
                    RefreshPolygonUsers();
                }
            }
        }

        private void RefreshPolygonUsers()
        {
            if(SelectedPolygon != null)
            {
                if(SelectedPolygon.PolygonTagTypeEquals(PolygonTagType.Hexagon))
                {
                    var friends = _layerService.UsersInsidePolygonTagBrief(SelectedPolygon.ExtractPolygonTag().Tag);
                    SelectedUsers = new ObservableCollection<UserInfoBriefViewCellModel>(friends);
                }
                else if(SelectedPolygon.PolygonTagTypeEquals(PolygonTagType.Zone))
                {
                    //TODO: read friends inside of Zone from webapi
                    var friends = _layerService.UsersInsideZone(SelectedPolygon.ExtractPolygonTag().Tag);
                    SelectedUsers = new ObservableCollection<UserInfoBriefViewCellModel>(friends);
                }
                else
                {
                    SelectedUsers.Clear();
                }
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
            if(PolygonUsersVisible)
            {
                SelectedPolygon = null;
                PolygonUsersVisible = false;
            }
        }
    }
}
