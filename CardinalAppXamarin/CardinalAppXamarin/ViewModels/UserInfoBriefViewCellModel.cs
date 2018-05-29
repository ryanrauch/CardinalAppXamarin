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

        public UserInfoBriefViewCellModel(string emptyMessage)
        {
            EmptyMessage = emptyMessage;
        }

        public UserInfoBriefViewCellModel(
            UserInfoContract userInfoContract,
            CurrentLayerContract currentLayerContract)
        {
            _userInfoContract = userInfoContract;
            _currentLayerContract = currentLayerContract;
        }

        private string _emptyMessage { get; set; }
        public string EmptyMessage
        {
            get { return _emptyMessage; }
            set
            {
                _emptyMessage = value;
                RaisePropertyChanged(() => EmptyMessage);
            }
        }

        public string Name
        {
            get
            {
                if(!string.IsNullOrEmpty(EmptyMessage))
                {
                    return EmptyMessage;
                }
                return _userInfoContract.UserName;
            }
        }
        public string LastUpdated
        {
            get
            {
                if(!string.IsNullOrEmpty(EmptyMessage))
                {
                    return string.Empty;
                }
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
