using CardinalAppXamarin.ViewModels.Base;
using CardinalLibrary.DataContracts;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CardinalAppXamarin.ViewModels
{
    public class ZoneViewModel : ViewModelBase
    {
        public ZoneViewModel(
            ZoneContract zoneContract,
            List<UserInfoBriefViewCellModel> zoneUsers)
        {
            ZoneContractInfo = zoneContract;
            ZoneUsers = new ObservableCollection<UserInfoBriefViewCellModel>(zoneUsers);
        }

        public string ZoneDescription => _zoneContractInfo?.Description;

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

        public override Task OnAppearingAsync()
        {
            return Task.CompletedTask;
        }
    }
}
