using CardinalAppXamarin.Services.Interfaces;
using CardinalAppXamarin.ViewModels.Base;
using CardinalAppXamarin.Views.Pages;
using CardinalLibrary.DataContracts;
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
        private readonly ILocalCredentialService _localCredentialService;
        private readonly INavigationService _navigationService;
        private readonly IRequestService _requestService;

        public ZoneHexagonViewModel(
            IZoneService zoneService,
            INavigationService navigationService,
            IRequestService requestService,
            ILocalCredentialService localCredentialService)
        {
            _zoneService = zoneService;
            _requestService = requestService;
            _navigationService = navigationService;
            _localCredentialService = localCredentialService;
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

            var profile = await _requestService.GetAsync<UserInfoContract>("api/UserInfoSelf");
            UserProfile = new ProfileViewModel() { UserInfo = profile };

            IsBusy = false;
        }

        private async Task SelectZoneAsync(ZoneViewModel zvm)
        {
            if(zvm != null)
            {
                await _navigationService.NavigatePushAsync<ZoneMapView>(new ZoneMapView(), zvm.ZoneContractInfo);
            }
        }


        ///////// Currently logged-in user profile
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
        public ICommand CurrentUserProfileCommand => new Command(ProfileClicked);

        public ICommand LogOutCommand => new Command(LogOut);
        private void LogOut()
        {
            //TODO: check that this works?
            _localCredentialService.DeleteCredentials();
            _navigationService.NavigateToLogin();
        }
        ////////////////////////////////
        public ICommand DisplayFriendsListCommand => new Command(()=>_navigationService.NavigateToFriendsList());
    }
}
