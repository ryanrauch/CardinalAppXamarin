using CardinalAppXamarin.Services.Interfaces;
using CardinalAppXamarin.Validation;
using CardinalAppXamarin.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CardinalAppXamarin.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IRequestService _requestService;
        private readonly INavigationService _navigationService;

        public LoginViewModel(
            IRequestService requestService,
            INavigationService navigationService)
        {
            _requestService = requestService;
            _navigationService = navigationService;
            _userName = new ValidatableObject<string>();
            _password = new ValidatableObject<string>();
            AddValidations();
        }

        private bool _persistent;
        public bool Persistent
        {
            get { return _persistent; }
            set
            {
                _persistent = value;
                RaisePropertyChanged(() => Persistent);
            }
        }

        private ValidatableObject<string> _userName;
        public ValidatableObject<string> UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                RaisePropertyChanged(() => UserName);
            }
        }

        private ValidatableObject<string> _password;
        public ValidatableObject<string> Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged(() => Password);
            }
        }

        public ICommand SignInCommand => new Command(async () => await SignInAsync());
        public ICommand RegisterCommand => new Command(Register);

        private async Task SignInAsync()
        {
            IsBusy = true;
            bool result = await _requestService.PostAuthenticationRequestAsync(UserName.Value, 
                                                                               Password.Value, 
                                                                               Persistent);
            if(result)
            {
                //await _navigationService.NavigatePushAsync<MainMapViewBase>(new MainMapView());
            }
            IsBusy = false;
        }

        private void Register()
        {
            //_openUrlService.OpenUrl(GlobalSetting.Instance.RegisterWebsite);
        }

        private void AddValidations()
        {
            _userName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Username is required." });
            _password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Password is required." });
        }
        private bool Validate()
        {
            bool isValidUser = ValidateUserName();
            bool isValidPassword = ValidatePassword();

            return isValidUser && isValidPassword;
        }
        private bool ValidateUserName()
        {
            return _userName.Validate();
        }
        private bool ValidatePassword()
        {
            return _password.Validate();
        }

        public override Task OnAppearingAsync()
        {
            throw new NotImplementedException();
        }
    }
}
