using CardinalAppXamarin.Models;
using CardinalAppXamarin.ViewModels.Base;
using CardinalLibrary.DataContracts;
using System;
using System.Threading.Tasks;

namespace CardinalAppXamarin.ViewModels
{
    public class UserInfoBriefViewCellModel : ViewModelBase
    {
        private readonly UserInfoContract _userInfoContract;
        private readonly CurrentLayerContract _currentLayerContract;

        public UserInfoBriefViewCellModel(
            UserInfoContract userInfoContract,
            CurrentLayerContract currentLayerContract)
        {
            _userInfoContract = userInfoContract;
            _currentLayerContract = currentLayerContract;
        }

        public string Name =>  _userInfoContract.UserName;
        public string LastUpdated => DateTime.Now.Subtract(_currentLayerContract.TimeStamp).ToString();

        public override Task OnAppearingAsync()
        {
            return Task.CompletedTask;
        }
    }
}
