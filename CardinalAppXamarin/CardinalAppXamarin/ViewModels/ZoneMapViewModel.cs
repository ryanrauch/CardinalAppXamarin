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
            MainMapInitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(cp, 12.0); // zoom can be within: [2,21]
        }

        private ZoneContract _zoneContract;

        //private String _headerText { get; set; }
        public String HeaderText
        {
            get
            {
                if (_zoneContract != null)
                {
                    return _zoneContract.Description;
                }
                return String.Empty;
            }
        }

        public MoveToRegionRequest MoveRequest => new MoveToRegionRequest();

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
                if(ZoneUsers.Count == 1)
                {
                    return "1 Friend";
                }
                else
                {
                    return String.Format("{0} Friends", ZoneUsers.Count);
                }
            }
        }

        public override void Initialize(object param)
        {
            base.Initialize(param);
            if (param is ZoneContract zp)
            {
                _zoneContract = zp;
            }
        }

        public override async Task OnAppearingAsync()
        {
            if (_zoneContract == null)
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
            double minLat = _zoneContract.ZoneShapes.Min(z => z.Latitude),
                   minLon = _zoneContract.ZoneShapes.Min(z => z.Longitude),
                   maxLat = _zoneContract.ZoneShapes.Max(z => z.Latitude),
                   maxLon = _zoneContract.ZoneShapes.Max(z => z.Longitude);
            Position cp = new Position(minLat + ((maxLat - minLat) / 2),
                                       minLon + ((maxLon - minLon) / 2));
            MapSpan ms = MapSpan.FromPositions(Polygons.First().Positions);
            //MapSpan ms = MapSpan.FromCenterAndRadius(cp, Distance.FromKilometers(1.5));
            MoveRequest.MoveToRegion(ms);
            //MainMapInitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(cp, 8.0); // zoom can be within: [2,21]
        }

        private void SetPolygons()
        {
            var zc = _zoneContract;

            Polygons.Clear();

            if(zc == null)
            {
                return;
            }

            Polygon poly = new Polygon()
            {
                //FillColor = Color.FromHex(zc.ARGBFill),
                //StrokeColor = Color.FromHex(zc.ARGBFill),
                FillColor = Color.FromHex(Constants.CardinalRedARGB),
                StrokeColor = Color.FromHex(Constants.CardinalRedARGB),
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
