using CardinalAppXamarin.ViewModels.Base;
using CardinalLibrary;
using CardinalLibrary.DataContracts;
using System;
using System.Threading.Tasks;

namespace CardinalAppXamarin.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        public ProfileViewModel()
        {
        }
        public string DisplayName => UserInfo?.UserName;
        public string FirstName => UserInfo?.FirstName;
        public string LastName => UserInfo?.LastName;
        public string Email => UserInfo?.Email;
        public string PhoneNumber => UserInfo?.PhoneNumber;
        public DateTime DateOfBirth => UserInfo == null ? DateTime.Now.Date : UserInfo.DateOfBirth;
        public AccountGender Gender => UserInfo == null ? AccountGender.Male : UserInfo.Gender;

        private UserInfoContract _userInfo { get; set; }
        public UserInfoContract UserInfo
        {
            get { return _userInfo; }
            set
            {
                _userInfo = value;
                RaisePropertyChanged(() => UserInfo);
            }
        }

        public override Task OnAppearingAsync()
        {
            return Task.CompletedTask;
        }
    }
}
