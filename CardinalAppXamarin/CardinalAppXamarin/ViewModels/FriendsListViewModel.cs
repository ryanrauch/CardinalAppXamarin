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
using CardinalAppXamarin.Models;

namespace CardinalAppXamarin.ViewModels
{
    public class FriendsListViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IFriendRequestService _friendRequestService;
        private readonly IRequestService _requestService;
        private readonly IUserInfoService _userInfoService;
        //private readonly ILayerService _layerService;
        //private readonly IZoneService _zoneService;
        private readonly IPhoneContactService _phoneContactService;

        public FriendsListViewModel(
            INavigationService navigationService,
            IFriendRequestService friendRequestService,
            IRequestService requestService,
            //ILayerService layerService,
            //IZoneService zoneService,
            IUserInfoService userInfoService,
            IPhoneContactService phoneContactService)
        {
            _navigationService = navigationService;
            _friendRequestService = friendRequestService;
            _requestService = requestService;
            _userInfoService = userInfoService;
            //_layerService = layerService;
            //_zoneService = zoneService;
            _phoneContactService = phoneContactService;
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

        private ObservableCollection<FriendViewCellModel> _contactSearchRequestFriends { get; set; } = new ObservableCollection<FriendViewCellModel>();
        public ObservableCollection<FriendViewCellModel> ContactSearchRequestFriends
        {
            get { return _contactSearchRequestFriends; }
            set
            {
                _contactSearchRequestFriends = value;
                RaisePropertyChanged(() => ContactSearchRequestFriends);
            }
        }
        private bool _includeImportedContacts => ContactSearchRequestFriends.Count > 0;

        public ICommand ImportContactsCommand => new Command(async () => await ImportContactsTask());
        private async Task ImportContactsTask()
        {
            var contacts = await _phoneContactService.GetAllContactsAsync();
            ContactSearchRequestFriends.Clear();
            foreach(var c in contacts)
            {
                if(String.IsNullOrEmpty(c.PhoneNumber))
                {
                    continue;
                }
                string formattedPhone = string.Empty;
                foreach(char pc in c.PhoneNumber)
                {
                    if(Char.IsDigit(pc))
                    {
                        formattedPhone += pc;
                    }
                }
                if (formattedPhone.Length != 10)
                {
                    continue;
                }
                var res = await _requestService.GetAsync<PhoneContactSearchContract>("api/PhoneContactSearch/" + formattedPhone);
                if(res.Found 
                    && !String.IsNullOrEmpty(res.UserName)
                    && !String.IsNullOrEmpty(res.UserId)
                    && !ContactSearchRequestFriends.Any(cs=>cs.UserId.Equals(res.UserId))
                    && !MutualFriends.Any(cs=>cs.UserId.Equals(res.UserId))
                    && !PendingFriends.Any(cs=>cs.UserId.Equals(res.UserId))
                    && !InitiatedRequestFriends.Any(cs=>cs.UserId.Equals(res.UserId)))
                {
                    var uic = new UserInfoContract()
                    {
                        Id = res.UserId,
                        UserName = res.UserName,
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        PhoneNumber = formattedPhone
                    };
                    var fvcm = new FriendViewCellModel(uic,
                                                       DateTime.Now,
                                                       FriendRequestType.Normal,
                                                       FriendStatus.FoundInContactSearch);
                    ContactSearchRequestFriends.Add(fvcm);
                }
            }
        }


        public ICommand ContactSearchButtonCommand => new Command<FriendViewCellModel>(async (vm) => await ContactSearchButtonTask(vm));
        private async Task ContactSearchButtonTask(FriendViewCellModel fvcm)
        {
            var frc = new FriendRequestContract()
            {
                InitiatorId = _userSelf.Id,
                TargetId = fvcm.UserId,
                TimeStamp = DateTime.Now,
                Type = FriendRequestType.Normal
            };
            await _friendRequestService.PostFriendRequestAsync(frc);
            ContactSearchRequestFriends.Remove(fvcm);
            fvcm.Status = FriendStatus.PendingRequest;
            PendingFriends.Add(fvcm);
            FilterGroupsBySearch();
        }

        public ICommand RequestSentButtonCommand => new Command<FriendViewCellModel>(async (vm) => await RequestSentButtonTask(vm));
        private async Task RequestSentButtonTask(FriendViewCellModel fvcm)
        {
            await _friendRequestService.DeleteFriendRequestAsync(fvcm.UserId);
            InitiatedRequestFriends.Remove(fvcm);
            FilterGroupsBySearch();
        }

        public ICommand PendingRequestButtonCommand => new Command<FriendViewCellModel>(async (vm) => await PendingRequestButtonTask(vm));
        private async Task PendingRequestButtonTask(FriendViewCellModel fvcm)
        {
            await Task.Delay(10);
            var contract = new FriendRequestContract()
            {
                InitiatorId = _userSelf.Id,
                TargetId = fvcm.UserId,
                TimeStamp = DateTime.Now,
                Type = FriendRequestType.Normal
            };
            await _friendRequestService.PostFriendRequestAsync(contract);
            PendingFriends.Remove(fvcm);
            fvcm.Status = FriendStatus.Mutual;
            MutualFriends.Add(fvcm);
            FilterGroupsBySearch();
        }

        public ICommand MutualFriendButtonCommand => new Command<FriendViewCellModel>(async (vm) => await MutualFriendButtonTask(vm));
        private async Task MutualFriendButtonTask(FriendViewCellModel fvcm)
        {
            await _friendRequestService.DeleteFriendRequestAsync(fvcm.UserId);
            MutualFriends.Remove(fvcm);
            fvcm.Status = FriendStatus.PendingRequest;
            PendingFriends.Add(fvcm);
            FilterGroupsBySearch();
        }

        //public ICommand UserProfileButtonCommand => new Command<FriendViewCellModel>(async (vm) => await UserProfileButtonTask(vm));
        //private async Task UserProfileButtonTask(FriendViewCellModel fvcm)
        public ICommand UserProfileButtonCommand => new Command<FriendViewCellModel>((vm) => UserProfileButtonTask(vm));
        private void UserProfileButtonTask(FriendViewCellModel fvcm)
        {
            UserFrameVisibility = true;
            // TODO: had issues trying to resolve the zone that a current
            //       user was in. need to refactor the zone-resolution services
            //if (fvcm != null)
            //{
            //    string zid = _layerService.FindZoneContainingUser(fvcm.UserId);
            //    ZoneContract zc = null;
            //    if (!String.IsNullOrEmpty(zid))
            //    {
            //        zc = await _zoneService.GetZoneContractAsync(zid);
            //    }
            //    if (zc != null)
            //    {
            //        fvcm.ZoneDescription = zc.Description;
            //    }
            //}
            SelectedFriendViewModel = fvcm;
        }

        public ICommand UserProfileViewCloseCommand => new Command(UserProfileViewCloseTask);
        private void UserProfileViewCloseTask()
        {
            UserFrameVisibility = false;
            SelectedFriendViewModel = null;
        }

        private FriendViewCellModel _selectedFriendViewModel { get; set; } = null;
        public FriendViewCellModel SelectedFriendViewModel
        {
            get { return _selectedFriendViewModel; }
            set
            {
                _selectedFriendViewModel = value;
                RaisePropertyChanged(() => SelectedFriendViewModel);
            }
        }

        private bool _userFrameVisibility { get; set; } = false;
        public bool UserFrameVisibility
        {
            get { return _userFrameVisibility; }
            set
            {
                _userFrameVisibility = value;
                RaisePropertyChanged(() => UserFrameVisibility);
            }
        }

        private UserInfoContract _userSelf { get; set; }

        private async Task SortDataAsync()
        {
            _userSelf = await _requestService.GetAsync<UserInfoContract>("api/UserInfoSelf");

            MutualFriends.Clear();
            PendingFriends.Clear();
            InitiatedRequestFriends.Clear();

            var entire = await _friendRequestService.GetAllFriendRequestsAsync();
            var initiated = entire.Where(f => f.InitiatorId.Equals(_userSelf.Id)
                                           && !f.TargetId.Equals(_userSelf.Id)).ToList();
            var targeted = entire.Where(f => f.TargetId.Equals(_userSelf.Id)
                                          && !f.InitiatorId.Equals(_userSelf.Id)).ToList();
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
            GroupedFriendModel csg = new GroupedFriendModel() { LongName = "Your Contacts", ShortName = "C" };
            foreach (var mv in ContactSearchRequestFriends.OrderBy(m => m.FirstAndLastName))
            {
                csg.Add(mv);
            }
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
            Grouped.Add(csg);
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
            GroupedFriendModel csg = new GroupedFriendModel() { LongName = "Your Contacts", ShortName = "C" };
            GroupedFriendModel pg = new GroupedFriendModel() { LongName = "Pending Friend Requests", ShortName = "P" };
            GroupedFriendModel mg = new GroupedFriendModel() { LongName = "Mutual Friends", ShortName = "M" };
            GroupedFriendModel ig = new GroupedFriendModel() { LongName = "Waiting for Response", ShortName = "W" };
            if (String.IsNullOrEmpty(SearchEntry))
            {
                if (_includeImportedContacts)
                {
                    foreach (var mv in ContactSearchRequestFriends.OrderBy(m => m.FirstAndLastName))
                    {
                        csg.Add(mv);
                    }
                }
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
                if (_includeImportedContacts)
                {
                    foreach (var mv in ContactSearchRequestFriends
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
                        csg.Add(mv);
                    }
                }
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
            if (_includeImportedContacts)
            {
                Grouped.Add(csg);
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
            //await _zoneService.InitializeData();
            //await _layerService.InitializeData();
            IsBusy = false;
        } 
    }
}
