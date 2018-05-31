using CardinalAppXamarin.Services.Interfaces;
using CardinalAppXamarin.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CardinalAppXamarin.ViewModels
{
    public class MainZoneViewModel : ViewModelBase
    {
        private readonly ILayerService _layerService;
        private readonly IZoneService _zoneService;
        private readonly INavigationService _navigationService;

        public MainZoneViewModel(
            ILayerService layerService,
            IZoneService zoneService,
            INavigationService navigationService)
        {
            _layerService = layerService;
            _zoneService = zoneService;
            _navigationService = navigationService;
            IsBusy = true;
        }

        private UserInfoBriefViewCellModel _emptyUserInfo = new UserInfoBriefViewCellModel("No Friends in this zone.");

        private ObservableCollection<ZoneViewModel> _zonesList { get; set; } = new ObservableCollection<ZoneViewModel>();
        public ObservableCollection<ZoneViewModel> ZonesList
        {
            get { return _zonesList; }
            set
            {
                _zonesList = value;
                RaisePropertyChanged(() => ZonesList);
            }
        }

        private ZoneViewModel _selectedZone { get; set; }
        public ZoneViewModel SelectedZone
        {
            get { return _selectedZone; }
            set
            {
                _selectedZone = value;
                RaisePropertyChanged(() => SelectedZone);
            }
        }

        public override async Task OnAppearingAsync()
        {
            await _layerService.InitializeData();
            var zones = await _zoneService.GetAllZoneContractsAsync();
            ZonesList.Clear();
            foreach(var zone in zones)
            {
                var zoneUsers = _layerService.UsersInsideZone(zone.ZoneID);
                ZonesList.Add(new ZoneViewModel(zone, zoneUsers));
            }
            IsBusy = false;
        }
    }
}
