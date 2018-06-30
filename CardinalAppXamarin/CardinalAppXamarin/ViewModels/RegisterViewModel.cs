using CardinalAppXamarin.Extensions;
using CardinalAppXamarin.Services.Interfaces;
using CardinalAppXamarin.Validation;
using CardinalAppXamarin.ViewModels.Base;
using CardinalLibrary;
using CardinalLibrary.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CardinalAppXamarin.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly IRequestService _requestService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        public RegisterViewModel(
            IRequestService requestService,
            INavigationService navigationService,
            IDialogService dialogService)
        {
            _requestService = requestService;
            _navigationService = navigationService;
            _dialogService = dialogService;
            AddValidations();
        }

        public String TitleText => "Cardinal";
        public String SubtitleText => "Registration";
        public bool BackButtonVisible => false;
        public ICommand BackButtonCommand => null;

        private String _resultMessage { get; set; } = String.Empty;
        public String ResultMessage
        {
            get { return _resultMessage; }
            set
            {
                _resultMessage = value;
                RaisePropertyChanged(() => ResultMessage);
            }
        }

        private bool _isEnabled { get; set; }
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                RaisePropertyChanged(() => IsEnabled);
            }
        }

        private ValidatableObject<string> _email { get; set; }
        public ValidatableObject<string> Email
        {
            get { return _email; }
            set
            {
                _email = value;
                RaisePropertyChanged(() => Email);
            }
        }

        private ValidatableObject<string> _password { get; set; }
        public ValidatableObject<string> Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged(() => Password);
            }
        }

        private ValidatableObject<string> _confirmPassword { get; set; }
        public ValidatableObject<string> ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                _confirmPassword = value;
                RaisePropertyChanged(() => ConfirmPassword);
            }
        }

        private ValidatableObject<string> _displayName { get; set; }
        public ValidatableObject<string> DisplayName
        {
            get { return _displayName; }
            set
            {
                _displayName = value;
                RaisePropertyChanged(() => DisplayName);
            }
        }

        private ValidatableObject<string> _firstName { get; set; }
        public ValidatableObject<string> FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                RaisePropertyChanged(() => FirstName);
            }
        }

        private ValidatableObject<string> _lastName { get; set; }
        public ValidatableObject<string> LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                RaisePropertyChanged(() => LastName);
            }
        }

        private ValidatableObject<DateTime> _dateOfBirth { get; set; }
        public ValidatableObject<DateTime> DateOfBirth
        {
            get { return _dateOfBirth; }
            set
            {
                _dateOfBirth = value;
                RaisePropertyChanged(() => DateOfBirth);
            }
        }

        private ValidatableObject<string> _phoneNumber { get; set; }
        public ValidatableObject<string> PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                RaisePropertyChanged(() => PhoneNumber);
            }
        }

        private AccountGender _selectedGender { get; set; }
        public AccountGender SelectedGender
        {
            get { return _selectedGender; }
            set
            {
                _selectedGender = value;
                RaisePropertyChanged(() => SelectedGender);
            }
        }

        public List<String> GenderList
        {
            get
            {
                return Enum.GetNames(typeof(AccountGender)).Select(g => g.SplitCamelCase()).ToList();
            }
        }

        public ICommand RegisterCommand => new Command(async () => await RegisterAsync());
        public ICommand ValidateCommand => new Command(() => Enable());
        public ICommand CancelRegisterCommand => new Command(() => _navigationService.NavigateToLogin());

        private void AddValidations()
        {
            _email = new ValidatableObject<string>();
            _password = new ValidatableObject<string>();
            _confirmPassword = new ValidatableObject<string>();
            _displayName = new ValidatableObject<string>();
            _firstName = new ValidatableObject<string>();
            _lastName = new ValidatableObject<string>();
            _dateOfBirth = new ValidatableObject<DateTime>();
            _phoneNumber = new ValidatableObject<string>();

            DateOfBirth.Value = DateTime.Now.Date;

            _email.Validations.Add(new IsEmailAddressRule<string> { ValidationMessage = "Valid email address is required. [name@domain.com]" });
            _password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Password is required." });
            _confirmPassword.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Confirmation of Password is required." });
            _displayName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "User Name is required." });
            _firstName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "First Name is required." });
            _lastName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Last Name is required." });
            _dateOfBirth.Validations.Add(new IsOverRequiredAgeRule { RequiredYears = 18, ValidationMessage = "Date of Birth, at least 18 years ago, is required." });
            _phoneNumber.Validations.Add(new IsPhoneNumberRule<string> { ValidationMessage = "Ten-digit phone number required. [Only numbers]" });
        }

        private void Enable()
        {
            IsEnabled = Validate();
        }

        private bool Validate()
        {
            _confirmPassword.Errors.Clear();
            if(!_email.Validate()
               || !_password.Validate()
               || !_confirmPassword.Validate()
               || !_displayName.Validate()
               || !_firstName.Validate()
               || !_lastName.Validate()
               || !_dateOfBirth.Validate()
               || !_phoneNumber.Validate())
            {
                return false;
            }
            if(!_password.Value.Equals(_confirmPassword.Value))
            {
                _confirmPassword.Errors.Add("Password must match Confirmation Password.");
                return false;
            }
            return true;
        }

        private async Task RegisterAsync()
        {
            var data = new UserInfoContract();
            try
            {
                var result = await _requestService.PostAsync<UserInfoContract, bool>("api/Registration", data);
                if (result)
                {
                    _navigationService.NavigateToLogin();
                }
                else
                {
                    ResultMessage = "Registration process failed. Please verify information provided.";
                    //await _dialogService.DisplayAlertAsync("Registration Failed", "Registration process failed. Please verify information provided.", "OK");
                }
            }
            catch(Exception ex)
            {
                //await _dialogService.DisplayAlertAsync(ex.Message, ex.StackTrace, "OK");
                ResultMessage = ex.Message + "\n" + ex.StackTrace;
            }
        }

        public override Task OnAppearingAsync()
        {
            return Task.CompletedTask;
        }
    }
}
