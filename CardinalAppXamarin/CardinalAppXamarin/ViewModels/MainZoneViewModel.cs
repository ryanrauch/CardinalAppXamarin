using CardinalAppXamarin.Services.Interfaces;
using CardinalAppXamarin.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace CardinalAppXamarin.ViewModels
{
    public class MainZoneViewModel : ViewModelBase
    {
        private readonly ILayerService _layerService;
        private readonly IZoneService _zoneService;

        public MainZoneViewModel(
            ILayerService layerService,
            IZoneService zoneService)
        {
            _layerService = layerService;
            _zoneService = zoneService;
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

        public override async Task OnAppearingAsync()
        {
            await _layerService.InitializeData();
            var zones = await _zoneService.GetAllZoneContractsAsync();
            ZonesList.Clear();
            foreach(var zone in zones)
            {
                var zoneUsers = _layerService.UsersInsideZone(zone.ZoneID);
                if(zoneUsers.Count == 0)
                {
                    zoneUsers.Add(_emptyUserInfo);
                }
                ZonesList.Add(new ZoneViewModel(zone, zoneUsers));
            }
            IsBusy = false;
        }
    }
}
