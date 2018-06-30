using CardinalAppXamarin.Services.Interfaces;
using CardinalAppXamarin.ViewModels.Base;
using CardinalLibrary;
using CardinalLibrary.DataContracts;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.GoogleMaps.Bindings;

namespace CardinalAppXamarin.ViewModels
{
    public class ZoneMapViewModel : ViewModelBase
    {
        private readonly ILayerService _layerService;
        private readonly IGeolocatorService _geolocatorService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        public ZoneMapViewModel(
            ILayerService layerService,
            IGeolocatorService geolocatorService,
            INavigationService navigationService,
            IDialogService dialogService)
        {
            _layerService = layerService;
            _geolocatorService = geolocatorService;
            _navigationService = navigationService;
            _dialogService = dialogService;
            IsBusy = true;
            var cp = _geolocatorService.LastRecordedPosition;
            MapRegion = MapSpan.FromCenterAndRadius(cp, Distance.FromKilometers(1.2));
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

        public bool BackButtonVisible => true;
        public ICommand BackButtonCommand => new Command(() => _navigationService.NavigateToMain());

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
        private MapSpan _region { get; set; }
        public MapSpan MapRegion
        {
            get { return _region; }
            set
            {
                _region = value;
                RaisePropertyChanged(() => MapRegion);
            }
        }

        public bool MapAnimated => false;

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
                RaiseHBProperties();
            }
        }

        /* Start of hard-bound 5-hexagon buttons */
        private void RaiseHBProperties()
        {
            RaisePropertyChanged(() => HB1Visibility);
            RaisePropertyChanged(() => HB2Visibility);
            RaisePropertyChanged(() => HB3Visibility);
            RaisePropertyChanged(() => HB4Visibility);
            RaisePropertyChanged(() => HB5Visibility);
            RaisePropertyChanged(() => HBPlusVisibility);
            RaisePropertyChanged(() => HB1Text);
            RaisePropertyChanged(() => HB2Text);
            RaisePropertyChanged(() => HB3Text);
            RaisePropertyChanged(() => HB4Text);
            RaisePropertyChanged(() => HB5Text);
        }

        public Boolean HB1Visibility
        {
            get { return ZoneUsers.Count >= 4; }
        }
        public Boolean HB2Visibility
        {
            get { return ZoneUsers.Count >= 2; }
        }
        public Boolean HB3Visibility
        {
            get { return ZoneUsers.Count > 0; }
        }
        public Boolean HB4Visibility
        {
            get { return ZoneUsers.Count >= 3; }
        }
        public Boolean HB5Visibility
        {
            get { return ZoneUsers.Count == 5; }
        }
        public Boolean HBPlusVisibility
        {
            get { return ZoneUsers.Count > 5; }
        }
        public String HB1Text
        {
            get
            {
                if (HB1Visibility)
                {
                    return ZoneUsers[0].Name;
                }
                return String.Empty;
            }
        }
        public String HB2Text
        {
            get
            {
                if (HB2Visibility)
                {
                    if(ZoneUsers.Count > 4)
                    {
                        return ZoneUsers[1].Name;
                    }
                    else
                    {
                        return ZoneUsers[0].Name;
                    }
                }
                return String.Empty;
            }
        }
        public String HB3Text
        {
            get
            {
                if (HB3Visibility)
                {
                    if (ZoneUsers.Count == 1)
                    {
                        return ZoneUsers[0].Name;
                    }
                    else if(ZoneUsers.Count < 4)
                    {
                        return ZoneUsers[1].Name;
                    }
                    else
                    {
                        return ZoneUsers[2].Name;
                    }
                }
                return String.Empty;
            }
        }
        public String HB4Text
        {
            get
            {
                if (HB4Visibility)
                {
                    if (ZoneUsers.Count > 4)
                    {
                        return ZoneUsers[2].Name;
                    }
                    else
                    {
                        return ZoneUsers[3].Name;
                    }
                }
                return String.Empty;
            }
        }
        public String HB5Text
        {
            get
            {
                if (HB5Visibility)
                {
                    return ZoneUsers[4].Name;
                }
                return String.Empty;
            }
        }

        public ICommand HB1Command => new Command(async () => await SelectFriendButton(0));
        public ICommand HB2Command => new Command(async () => await SelectFriendButton(1));
        public ICommand HB3Command => new Command(async () => await SelectFriendButton(2));
        public ICommand HB4Command => new Command(async () => await SelectFriendButton(3));
        public ICommand HB5Command => new Command(async () => await SelectFriendButton(4));
        public ICommand SelectMoreFriendsCommand => new Command(() => _navigationService.NavigateToMain());

        private async Task SelectFriendButton(int i)
        {
            await _dialogService.DisplayAlertAsync("index: " + i.ToString(),
                                                   ZoneUsers[i].Name,
                                                   "OK");
        }

        public String ZoneUsersCountText
        {
            get
            {
                if (IsBusy)
                {
                    return "Loading...";
                }
                int zc = ZoneUsers.Count;
                if (zc == 0)
                {
                    return "No Friends";
                }
                else if (zc == 1)
                {
                    return "1 Friend";
                }
                else
                {
                    return String.Format("{0} Friends", zc);
                }
            }
        }
        /* End of hard-bound 5-hexagon buttons */


        /* Start of Aggregate Data */
        public String ZoneDescription
        {
            get { return SubtitleText; }
        }

        public int ZoneAggregateUsers => 532;
        public int ZoneAggregateFemale => 427;
        public int ZoneAggregateMale => 105;
        public float ZoneAggregateFemalePercent => (float)ZoneAggregateFemale / ZoneAggregateUsers;
        public float ZoneAggregateMalePercent => (float)ZoneAggregateMale / ZoneAggregateUsers;

        public String ZoneAggregateTitle
        {
            get
            {
                if (IsBusy)
                {
                    return "Loading...";
                }
                int zc = ZoneAggregateUsers;
                if (zc == 0)
                {
                    return String.Format("{0} is Empty", ZoneDescription);
                }
                else if (zc == 1)
                {
                    return String.Format("1 person in {0}", ZoneDescription);
                }
                else
                {
                    return String.Format("{0} people in {1}", zc, ZoneDescription);
                }
            }
        }
        /* End of Aggregate Data */

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
            else
            {
                ZoneUsers = new ObservableCollection<UserInfoBriefViewCellModel>();
            }
            IsBusy = false;
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
            MapRegion = MapSpan.FromBounds(b);
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
