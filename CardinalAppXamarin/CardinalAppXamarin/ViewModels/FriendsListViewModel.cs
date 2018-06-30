using CardinalAppXamarin.Services.Interfaces;
using CardinalAppXamarin.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using CardinalLibrary.DataContracts;
using CardinalLibrary;

namespace CardinalAppXamarin.ViewModels
{
    public class FriendsListViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IFriendRequestService _friendRequestService;
        private readonly IRequestService _requestService;
        private readonly IUserInfoService _userInfoService;

        public FriendsListViewModel(
            INavigationService navigationService,
            IFriendRequestService friendRequestService,
            IRequestService requestService,
            IUserInfoService userInfoService)
        {
            _navigationService = navigationService;
            _friendRequestService = friendRequestService;
            _requestService = requestService;
            _userInfoService = userInfoService;
            IsBusy = true;
        }

        public String TitleText => "Cardinal";
        public String SubtitleText => "Friends List";
        public bool BackButtonVisible => true;
        public ICommand BackButtonCommand => new Command(() => _navigationService.NavigateToMain());

        private ObservableCollection<FriendViewCellModel> _mutualFriends { get; set; } = new ObservableCollection<FriendViewCellModel>();
        public ObservableCollection<FriendViewCellModel> MutualFriends
        {
            get { return _mutualFriends; }
            set
            {
                _mutualFriends = value;
                RaisePropertyChanged(() => MutualFriends);
            }
        }

        private ObservableCollection<FriendViewCellModel> _pendingFriends { get; set; } = new ObservableCollection<FriendViewCellModel>();
        public ObservableCollection<FriendViewCellModel> PendingFriends
        {
            get { return _pendingFriends; }
            set
            {
                _pendingFriends = value;
                RaisePropertyChanged(() => PendingFriends);
            }
        }

        private ObservableCollection<FriendViewCellModel> _initiatedRequestFriends { get; set; } = new ObservableCollection<FriendViewCellModel>();
        public ObservableCollection<FriendViewCellModel> InitiatedRequestFriends
        {
            get { return _initiatedRequestFriends; }
            set
            {
                _initiatedRequestFriends = value;
                RaisePropertyChanged(() => InitiatedRequestFriends);
            }
        }

        public ICommand ImportContactsCommand => null;

        public ICommand RequestSentButtonCommand => new Command<FriendViewCellModel>(async (vm) => await RequestSentButtonTask(vm));
        private async Task RequestSentButtonTask(FriendViewCellModel fvcm)
        {
            await Task.Delay(10);
        }

        public ICommand PendingRequestButtonCommand => new Command<FriendViewCellModel>(async (vm) => await PendingRequestButtonTask(vm));
        private async Task PendingRequestButtonTask(FriendViewCellModel fvcm)
        {
            await Task.Delay(10);
        }

        public ICommand MutualFriendButtonCommand => new Command<FriendViewCellModel>(async (vm) => await MutualFriendButtonTask(vm));
        private async Task MutualFriendButtonTask(FriendViewCellModel fvcm)
        {
            await Task.Delay(10);
        }

        private UserInfoContract _userSelf { get; set; }

        private async Task SortDataAsync()
        {
            _userSelf = await _requestService.GetAsync<UserInfoContract>("api/UserInfoSelf");

            MutualFriends.Clear();
            PendingFriends.Clear();
            InitiatedRequestFriends.Clear();

            var entire = await _friendRequestService.GetAllFriendRequestsAsync();
            var initiated = entire.Where(f => f.InitiatorId.Equals(_userSelf.Id)).ToList();
            var targeted = entire.Where(f => f.TargetId.Equals(_userSelf.Id)).ToList();
            // first scan through inbound requests,
            // if there is also an initiated request, becomes mutual,
            // otherwise, it is a pending request
            foreach (var req in targeted)
            {
                var initiatorInfo = await _userInfoService.GetUserInfoAsync(req.InitiatorId);
                if (initiated.Any(f => f.TargetId.Equals(req.InitiatorId)))
                {
                    MutualFriends.Add(new FriendViewCellModel(initiatorInfo, 
                                                              req.TimeStamp, 
                                                              req.Type ?? FriendRequestType.Normal, 
                                                              FriendStatus.Mutual));
                }
                else
                {
                    PendingFriends.Add(new FriendViewCellModel(initiatorInfo,
                                                              req.TimeStamp,
                                                              req.Type ?? FriendRequestType.Normal,
                                                              FriendStatus.PendingRequest));
                }
            }
            // scan through the initiated requests,
            // ignoring requests that have a matching target record,
            // these should have already been placed into mutual
            foreach(var req in initiated)
            {
                if(!targeted.Any(f => f.InitiatorId.Equals(req.TargetId)))
                {
                    var targetInfo = await _userInfoService.GetUserInfoAsync(req.TargetId);
                    InitiatedRequestFriends.Add(new FriendViewCellModel(targetInfo,
                                                                        req.TimeStamp,
                                                                        req.Type ?? FriendRequestType.Normal,
                                                                        FriendStatus.Initiated));
                }
            }
            Grouped.Clear();
            GroupedFriendModel pg = new GroupedFriendModel() { LongName = "Pending Friend Requests", ShortName = "P" };
            foreach (var pv in PendingFriends.OrderBy(m => m.FirstAndLastName))
            {
                pg.Add(pv);
            }
            GroupedFriendModel mg = new GroupedFriendModel() { LongName = "Mutual Friends", ShortName = "M" };
            foreach(var mv in MutualFriends.OrderBy(m => m.FirstAndLastName))
            {
                mg.Add(mv);
            }
            GroupedFriendModel ig = new GroupedFriendModel() { LongName = "Waiting for Response", ShortName = "W" };
            foreach (var iv in InitiatedRequestFriends.OrderBy(m => m.FirstAndLastName))
            {
                ig.Add(iv);
            }
            Grouped.Add(pg);
            Grouped.Add(mg);
            Grouped.Add(ig);
        }

        private ObservableCollection<GroupedFriendModel> _grouped { get; set; } = new ObservableCollection<GroupedFriendModel>();
        public ObservableCollection<GroupedFriendModel> Grouped
        {
            get { return _grouped; }
            set
            {
                _grouped = value;
                RaisePropertyChanged(() => Grouped);
            }
        }

        private String _searchEntry { get; set; }
        public String SearchEntry
        {
            get { return _searchEntry; }
            set
            {
                _searchEntry = value;
                RaisePropertyChanged(() => SearchEntry);
            }
        }

        public ICommand SearchCommand => new Command(FilterGroupsBySearch);

        private void FilterGroupsBySearch()
        {
            Grouped.Clear();
            GroupedFriendModel pg = new GroupedFriendModel() { LongName = "Pending Friend Requests", ShortName = "P" };
            GroupedFriendModel mg = new GroupedFriendModel() { LongName = "Mutual Friends", ShortName = "M" };
            GroupedFriendModel ig = new GroupedFriendModel() { LongName = "Waiting for Response", ShortName = "W" };
            if (String.IsNullOrEmpty(SearchEntry))
            {
                foreach (var pv in PendingFriends.OrderBy(m => m.FirstAndLastName))
                {
                    pg.Add(pv);
                }
                foreach (var mv in MutualFriends.OrderBy(m => m.FirstAndLastName))
                {
                    mg.Add(mv);
                }
                foreach (var iv in InitiatedRequestFriends.OrderBy(m => m.FirstAndLastName))
                {
                    ig.Add(iv);
                }
            }
            else
            {
                foreach (var pv in PendingFriends
                                   .Where(p => p.UserName
                                                .StartsWith(SearchEntry, StringComparison.OrdinalIgnoreCase)
                                            || p.FirstName
                                                .StartsWith(SearchEntry, StringComparison.OrdinalIgnoreCase)
                                            || p.LastName
                                                .StartsWith(SearchEntry, StringComparison.OrdinalIgnoreCase)
                                            || p.PhoneNumber
                                                .StartsWith(SearchEntry, StringComparison.OrdinalIgnoreCase))
                                   .OrderBy(m => m.FirstAndLastName))
                {
                    pg.Add(pv);
                }
                foreach (var mv in MutualFriends
                                   .Where(p => p.UserName
                                                .StartsWith(SearchEntry, StringComparison.OrdinalIgnoreCase)
                                            || p.FirstName
                                                .StartsWith(SearchEntry, StringComparison.OrdinalIgnoreCase)
                                            || p.LastName
                                                .StartsWith(SearchEntry, StringComparison.OrdinalIgnoreCase)
                                            || p.PhoneNumber
                                                .StartsWith(SearchEntry, StringComparison.OrdinalIgnoreCase))
                                   .OrderBy(m => m.FirstAndLastName))
                {
                    mg.Add(mv);
                }
                foreach (var iv in InitiatedRequestFriends
                                   .Where(p => p.UserName
                                                .StartsWith(SearchEntry, StringComparison.OrdinalIgnoreCase)
                                            || p.FirstName
                                                .StartsWith(SearchEntry, StringComparison.OrdinalIgnoreCase)
                                            || p.LastName
                                                .StartsWith(SearchEntry, StringComparison.OrdinalIgnoreCase)
                                            || p.PhoneNumber
                                                .StartsWith(SearchEntry, StringComparison.OrdinalIgnoreCase))
                                   .OrderBy(m => m.FirstAndLastName))
                {
                    ig.Add(iv);
                }
            }
            Grouped.Add(pg);
            Grouped.Add(mg);
            Grouped.Add(ig);
        }

        public override async Task OnAppearingAsync()
        {
            await _friendRequestService.InitializeDataAsync();
            await _userInfoService.InitializeDataAsync();
            await SortDataAsync();
            IsBusy = false;
        } 
    }

    public class GroupedFriendModel : ObservableCollection<FriendViewCellModel>
    {
        public string LongName { get; set; }
        public string ShortName { get; set; }
    }
}
