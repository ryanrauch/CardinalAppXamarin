using CardinalAppXamarin.Services.Interfaces;
using CardinalAppXamarin.ViewModels.Base;
using CardinalLibrary;
using CardinalLibrary.DataContracts;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.GoogleMaps.Bindings;

namespace CardinalAppXamarin.ViewModels
{
    public class ZoneMapViewModel : ViewModelBase
    {
        private readonly ILayerService _layerService;
        private readonly IGeolocatorService _geolocatorService;

        public ZoneMapViewModel(
            ILayerService layerService,
            IGeolocatorService geolocatorService)
        {
            _layerService = layerService;
            _geolocatorService = geolocatorService;
            var cp = _geolocatorService.LastRecordedPosition;
            MainMapInitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(cp, 13.0); // zoom can be within: [2,21]
        }

        public string TitleText => "Cardinal";

        public String SubtitleText
        {
            get
            {
                if (ZoneContract != null)
                {
                    return ZoneContract.Description;
                }
                return String.Empty;
            }
        }

        public bool BackButtonVisible => false;

        private ZoneContract _zoneContract { get; set; }
        public ZoneContract ZoneContract
        {
            get { return _zoneContract; }
            set
            {
                _zoneContract = value;
                RaisePropertyChanged(() => ZoneContract);
                RaisePropertyChanged(() => SubtitleText);
            }
        }

        public MoveCameraRequest MoveCameraRequest { get; } = new MoveCameraRequest();

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

        private ObservableCollection<UserInfoBriefViewCellModel> _zoneUsers { get; set; } = new ObservableCollection<UserInfoBriefViewCellModel>();
        public ObservableCollection<UserInfoBriefViewCellModel> ZoneUsers
        {
            get { return _zoneUsers; }
            set
            {
                _zoneUsers = value;
                RaisePropertyChanged(() => ZoneUsers);
                RaisePropertyChanged(() => ZoneUsersCountText);
            }
        }

        public String ZoneUsersCountText
        {
            get
            {
                int zc = ZoneUsers.Count;
                if(zc == 0)
                {
                    return "No Friends";
                }
                else if(zc == 1)
                {
                    return "1 Friend";
                }
                else
                {
                    return String.Format("{0} Friends", zc);
                }
            }
        }

        public override void Initialize(object param)
        {
            base.Initialize(param);
            if (param is ZoneContract zp)
            {
                ZoneContract = zp;
            }
        }

        public override async Task OnAppearingAsync()
        {
            if (ZoneContract == null)
            {
                return;
            }

            SetMapPosition();
            SetPolygons();

            await _layerService.InitializeData();

            var users = _layerService.UsersInsideZone(_zoneContract.ZoneID);
            if(users != null && users.Count > 0)
            {
                ZoneUsers = new ObservableCollection<UserInfoBriefViewCellModel>(users);
            }
        }

        private void SetMapPosition()
        {
            double minLat = ZoneContract.ZoneShapes.Min(z => z.Latitude),
                   minLon = ZoneContract.ZoneShapes.Min(z => z.Longitude),
                   maxLat = ZoneContract.ZoneShapes.Max(z => z.Latitude),
                   maxLon = ZoneContract.ZoneShapes.Max(z => z.Longitude),
                   halfLat = (maxLat - minLat) / 2,
                   halfLon = (maxLon - minLon) / 2;
            Position cp = new Position(minLat + ((maxLat - minLat) / 2),
                                       minLon + ((maxLon - minLon) / 2));
            Bounds b = new Bounds(new Position(minLat - halfLat, minLon - halfLon),
                                  new Position(maxLat + halfLat, maxLon + halfLon));
            //MoveCameraRequest.MoveCamera(CameraUpdateFactory.NewPositionZoom(cp, 14.0));
            MoveCameraRequest.MoveCamera(CameraUpdateFactory.NewBounds(b, 0));
        }

        private void SetPolygons()
        {
            var zc = ZoneContract;

            Polygons.Clear();

            if(zc == null)
            {
                return;
            }

            Polygon poly = new Polygon()
            {
                //FillColor = Color.FromHex(zc.ARGBFill),
                //StrokeColor = Color.FromHex(zc.ARGBFill),
                FillColor = Color.FromHex(Constants.CardinalRed50ARGB),
                StrokeColor = Color.FromHex(Constants.CardinalRed50ARGB),
                StrokeWidth = 1.0f,
                Tag = new PolygonTag()
                {
                    PolygonTagType = PolygonTagType.Zone,
                    Tag = zc.Description
                }
            };
            foreach (var shape in zc.ZoneShapes.Where(z => z.Order > 0).OrderBy(z => z.Order))
            {
                poly.Positions.Add(new Position(shape.Latitude, shape.Longitude));
                //TODO: add code to display negative space
            }
            Polygons.Add(poly);
        }
    }
}
