﻿using CardinalAppXamarin.Services.Interfaces;
using CardinalAppXamarin.Validation;
using CardinalAppXamarin.ViewModels.Base;
using CardinalAppXamarin.Views;
using CardinalAppXamarin.Views.Pages;
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
        private readonly IDialogService _dialogService;
        private readonly ILocalCredentialService _localCredentialService;

        public LoginViewModel(
            IRequestService requestService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILocalCredentialService localCredentialService)
        {
            _requestService = requestService;
            _navigationService = navigationService;
            _dialogService = dialogService;
            _localCredentialService = localCredentialService;
            UserName = new ValidatableObject<string>();
            Password = new ValidatableObject<string>();
            AddValidations();
        }

        public String TitleText => "Cardinal";
        public String SubtitleText => String.Empty;
        public bool BackButtonVisible => false;
        public ICommand BackButtonCommand => null;

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

        private String _loginResultMessage { get; set; } = String.Empty;
        public String LoginResultMessage
        {
            get { return _loginResultMessage; }
            set
            {
                _loginResultMessage = value;
                RaisePropertyChanged(() => LoginResultMessage);
            }
        }

        private bool _isValid;
        public bool IsValid
        {
            get
            {
                return _isValid;
            }
            set
            {
                _isValid = value;
                RaisePropertyChanged(() => IsValid);
            }
        }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
                RaisePropertyChanged(() => IsEnabled);
            }
        }

        public ICommand SignInCommand => new Command(async () => await SignInAsync());
        public ICommand RegisterCommand => new Command(async () => await Register());
        public ICommand ValidateCommand => new Command(() => Enable());

        private async Task SignInAsync()
        {
            IsBusy = true;
            LoginResultMessage = String.Empty;
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("username", UserName.Value));
            parameters.Add(new KeyValuePair<string, string>("password", Password.Value));
            parameters.Add(new KeyValuePair<string, string>("persistent", Persistent.ToString()));
            var result = await _requestService.PostAsync<IEnumerable<KeyValuePair<string, string>>, bool>("api/Token", parameters, false);
            //bool result = await _requestService.PostAuthenticationRequestAsync(UserName.Value, 
            //                                                                   Password.Value, 
            //                                                                   Persistent);
            if(result)
            {
                if(Persistent)
                {
                    _localCredentialService.SaveCredentials(UserName.Value, Password.Value);
                }
                _navigationService.NavigateToMain();
            }
            else
            {
                //await _dialogService.DisplayAlertAsync("Log-in Failed", "Invalid Username or Credentials", "OK");
                LoginResultMessage = "Invalid Username or Credentials";
            }
            IsBusy = false;
        }

        private void Enable()
        {
            //IsEnabled = !string.IsNullOrEmpty(UserName.Value) 
            //            && !string.IsNullOrEmpty(Password.Value);
            IsEnabled = Validate();
        }

        private async Task Register()
        {
            await _navigationService.NavigatePushAsync(new RegisterView());
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
            //should clear out username/password values here??
            //_userName = new ValidatableObject<string>();
            //_password = new ValidatableObject<string>();
            //AddValidations();
            //if (!String.IsNullOrEmpty(UserName.Value))
            //{
            //    UserName.Value = _localCredentialService.UserName;
            //    Password.Value = String.Empty;
            //}
            return Task.CompletedTask;
        }
    }
}
