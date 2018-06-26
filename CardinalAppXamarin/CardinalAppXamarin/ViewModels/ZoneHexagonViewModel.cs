using CardinalAppXamarin.Services.Interfaces;
using CardinalAppXamarin.ViewModels.Base;
using CardinalAppXamarin.Views.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CardinalAppXamarin.ViewModels
{
    public class ZoneHexagonViewModel : ViewModelBase
    {
        private readonly IZoneService _zoneService;
        private readonly INavigationService _navigationService;

        public ZoneHexagonViewModel(
            IZoneService zoneService,
            INavigationService navigationService)
        {
            _zoneService = zoneService;
            _navigationService = navigationService;
            IsBusy = true;
        }

        public String TitleText => "Cardinal";
        public String SubtitleText => String.Empty;
        public bool BackButtonVisible => false;
        public ICommand BackButtonCommand => null;

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

        public ICommand SelectZoneCommand => new Command<ZoneViewModel>(async (ZoneViewModel z) => await SelectZoneAsync(z));

        public override async Task OnAppearingAsync()
        {
            await _zoneService.InitializeData();
            ZonesList.Clear();
            var zones = await _zoneService.GetAllZoneContractsAsync();
            foreach(var zone in zones)
            {
                ZonesList.Add(new ZoneViewModel(zone));
            }
            IsBusy = false;
        }

        private async Task SelectZoneAsync(ZoneViewModel zvm)
        {
            if(zvm != null)
            {
                await _navigationService.NavigatePushAsync<ZoneMapView>(new ZoneMapView(), zvm.ZoneContractInfo);
            }
        }
    }
}
