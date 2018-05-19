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

        public string Name => _userInfoContract.UserName;
        public string LastUpdated
        {
            get
            {
                TimeSpan span = DateTime.Now.ToUniversalTime().Subtract(_currentLayerContract.TimeStamp);
                int hours = (int)Math.Floor(Math.Abs(span.TotalHours));
                if (hours == 1)
                {
                    return String.Format("{0}hr ago", hours);
                }
                else if (hours > 1)
                {
                    return String.Format("{0}hrs ago", hours);
                }
                else
                {
                    return String.Format("{0}mins ago", (int)Math.Floor(Math.Abs(span.TotalMinutes)));
                }
            }
        }

        public override Task OnAppearingAsync()
        {
            return Task.CompletedTask;
        }
    }
}
