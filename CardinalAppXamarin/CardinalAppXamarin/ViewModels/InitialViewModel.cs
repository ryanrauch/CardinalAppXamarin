using CardinalAppXamarin.Services.Interfaces;
using CardinalAppXamarin.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CardinalAppXamarin.ViewModels
{
    public class InitialViewModel : ViewModelBase
    {
        private readonly IRequestService _requestService;
        private readonly ILocalCredentialService _localCredentialService;
        private readonly IAppVersionService _appVersionService;
        private readonly INavigationService _navigationService;

        public InitialViewModel(
            IRequestService requestService,
            ILocalCredentialService localCredentialService,
            IAppVersionService appVersionService,
            INavigationService navigationService)
        {
            _requestService = requestService;
            _localCredentialService = localCredentialService;
            _appVersionService = appVersionService;
            _navigationService = navigationService;
        }

        private string _message { get; set; }
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                RaisePropertyChanged(() => Message);
            }
        }

        public override async Task OnAppearing()
        {
            await base.OnAppearing();
            Message = "Checking Stored Credentials";
            string username = _localCredentialService.UserName;
            string password = _localCredentialService.Password;
            if (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password))
            {
                var result = await _requestService.PostAuthenticationRequestAsync(username, password, true);
                if (result)
                {
                    Message = "Checking Application Version";
                    var checkVersion = await CheckAppVersion();
                    if (checkVersion)
                    {
                        _navigationService.NavigateToMain();
                    }
                    else
                    {
                        Message = String.Format("Application Version {0} is out of date. Please update to the newest version.",
                                                _appVersionService.Version);
                    }
                }
                else
                {
                    Message = "Stored Credentials Failed.";
                    _navigationService.NavigateToLogin();
                }
            }
            else
            {
                _navigationService.NavigateToLogin();
            }
        }
        public async Task<bool> CheckAppVersion()
        {
            string version = _appVersionService.Version;

            return true;
        }
    }
}
