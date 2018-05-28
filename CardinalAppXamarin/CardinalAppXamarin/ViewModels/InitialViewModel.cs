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
        private readonly IDialogService _dialogService;

        public InitialViewModel(
            IRequestService requestService,
            ILocalCredentialService localCredentialService,
            IAppVersionService appVersionService,
            INavigationService navigationService,
            IDialogService dialogService)
        {
            _requestService = requestService;
            _localCredentialService = localCredentialService;
            _appVersionService = appVersionService;
            _navigationService = navigationService;
            _dialogService = dialogService;
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

        public override async Task OnAppearingAsync()
        {
            Message = "Checking Stored Credentials";
            string username = _localCredentialService.UserName;
            string password = _localCredentialService.Password;
            if (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password))
            {
                var parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("username", username));
                parameters.Add(new KeyValuePair<string, string>("password", password));
                parameters.Add(new KeyValuePair<string, string>("persistent", "True"));
                var result = await _requestService.PostAsync<IEnumerable<KeyValuePair<string,string>>,bool>("api/Token", parameters, false);
                //var result = await _requestService.PostAuthenticationRequestAsync(username, password, true);
                if (result)
                {
                    Message = "Checking Application Version";
                    var checkVersion = CheckAppVersion();
                    if (checkVersion)
                    {
                        _navigationService.NavigateToMain();
                    }
                    else
                    {
                        await _dialogService.DisplayAlertAsync(String.Format("Application Version {0} is out of date.", _appVersionService.Version), "Please update to the newest version.", "OK");
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

        public bool CheckAppVersion()
        {
            string version = _appVersionService.Version;

            return true;
        }
    }
}
