using CardinalAppXamarin.ViewModels.Base;
using CardinalLibrary;
using CardinalLibrary.DataContracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace CardinalAppXamarin.ViewModels
{
    public class ZoneViewModel : ViewModelBase
    {
        public ZoneViewModel()
        { }

        public ZoneViewModel(
            ZoneContract zoneContract,
            List<UserInfoBriefViewCellModel> zoneUsers)
        {
            Initialize(zoneContract, zoneUsers);
        }

        public void Initialize(ZoneContract zoneContract, List<UserInfoBriefViewCellModel> zoneUsers)
        {
            ZoneContractInfo = zoneContract;
            if (zoneUsers != null && zoneUsers.Count > 0)
            {
                ZoneUsers = new ObservableCollection<UserInfoBriefViewCellModel>(zoneUsers);
            }
        }

        public string ZoneDescription => _zoneContractInfo?.Description;
        public double RatioFriends => GetZoneUsersRatio();
        //public double RatioAggregate => 0.33;
        public double Distance => 1.3;
        public string DistanceText => string.Format("{0} miles", Distance);
        public Position ZoneCenter => new Position(30.400992, -97.722821);

        public string FriendsText
        {
            get
            {
                if(ZoneUsers.Count == 0)
                {
                    return "No Friends";
                }
                else if (ZoneUsers.Count == 1)
                {
                    return "1 Friend";
                }
                else
                {
                    return string.Format("{0} Friends", ZoneUsers.Count);
                }
            }
        }


        private ZoneContract _zoneContractInfo { get; set; }
        public ZoneContract ZoneContractInfo
        {
            get { return _zoneContractInfo; }
            set
            {
                _zoneContractInfo = value;
                RaisePropertyChanged(() => ZoneContractInfo);
            }
        }

        private ObservableCollection<UserInfoBriefViewCellModel> _zoneUsers { get; set; } = new ObservableCollection<UserInfoBriefViewCellModel>();
        public ObservableCollection<UserInfoBriefViewCellModel> ZoneUsers
        {
            get { return _zoneUsers; }
            set
            {
                _zoneUsers = value;
                RaisePropertyChanged(() => ZoneUsers);
            }
        }

        private double GetZoneUsersRatio()
        {
            double total = ZoneUsers.Count;
            if (total > 0)
            {
                double f = ZoneUsers.Where(z => z.Gender.Equals(AccountGender.Female)).Count();
                return f / total;
            }
            return 0;
        }

        public override Task OnAppearingAsync()
        {
            return Task.CompletedTask;
        }
    }
}
