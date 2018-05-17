using CardinalAppXamarin.Extensions;
using CardinalAppXamarin.Services.Interfaces;
using Plugin.Geolocator;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace CardinalAppXamarin.Services
{
    public class CrossGeolocatorService : IGeolocatorService
    {
        private readonly IPermissionService _permissionService;
        private readonly IRequestService _requestService;
        private bool _alreadyCheckedPermissions;

        private Position _lastRecordedPosition { get; set; }
        public Position LastRecordedPosition
        {
            get
            {
                if(_lastRecordedPosition == null)
                {
                    _lastRecordedPosition = GetCurrentPosition().Result;
                }
                return _lastRecordedPosition;
            }
        }

        public CrossGeolocatorService(
            IPermissionService permissionService,
            IRequestService requestService)
        {
            _permissionService = permissionService;
            _requestService = requestService;
            _alreadyCheckedPermissions = false;
        }

        public async Task<bool> IsLocationAvailable()
        {
            if(!CrossGeolocator.IsSupported)
            {
                return false;
            }
            if (!_alreadyCheckedPermissions)
            {
                var status = await _permissionService.CheckAndRequestPermissionAsync(Permission.LocationWhenInUse);
                if (status != PermissionStatus.Granted)
                {
                    return false;
                }
                _alreadyCheckedPermissions = true;
            }
            return CrossGeolocator.Current.IsGeolocationAvailable;
        }

        public async Task<Position> GetCurrentPosition()
        {
            var avail = await IsLocationAvailable();
            if (avail)
            {
                var geoPosition = await CrossGeolocator.Current.GetPositionAsync();

                await _requestService.PostAsync("api/UserLocation", geoPosition.GeolocatorToDataContract());
                _lastRecordedPosition = geoPosition.GeolocatorToGoogleMaps();
                return _lastRecordedPosition;
            }
            return new Position();
        }
    }
}
