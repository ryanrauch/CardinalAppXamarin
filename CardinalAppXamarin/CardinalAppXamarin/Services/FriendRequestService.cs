using CardinalAppXamarin.Services.Interfaces;
using CardinalAppXamarin.ViewModels;
using CardinalLibrary.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardinalAppXamarin.Services
{
    public class FriendRequestService : IFriendRequestService
    {
        private readonly IRequestService _requestService;

        public FriendRequestService(
            IRequestService requestService)
        {
            _requestService = requestService;
            _initialized = false;
        }

        private bool _initialized { get; set; }
        private DateTime _lastUpdated { get; set; }
        private List<FriendRequestContract> _friendRequestContracts { get; set; }

        public async Task InitializeDataAsync()
        {
            _friendRequestContracts = await _requestService.GetAsync<List<FriendRequestContract>>("api/FriendRequest");
            _lastUpdated = DateTime.Now;
            _initialized = true;
        }

        private async Task CheckInitialization()
        {
            if(!_initialized 
               || DateTime.Now - _lastUpdated > Constants.CardinalStaleDataTimeSpan)
            {
                await InitializeDataAsync();
            }
        }

        public async Task<List<FriendRequestContract>> GetAllFriendRequestsAsync()
        {
            await CheckInitialization();
            return _friendRequestContracts;
        }

        public async Task<FriendRequestContract> GetFriendRequestAsync(string target, string initator)
        {
            await CheckInitialization();
            var local = _friendRequestContracts.First(f => f.TargetId.Equals(target)
                                                         && f.InitiatorId.Equals(initator));
            if(local != null)
            {
                return local;
            }
            //TODO: Re-send a request for specific item
            //      then return null only if that does not exist
            return null;
        }

        public async Task PostFriendRequestAsync(FriendRequestContract contract)
        {
            await _requestService.PostAsync("api/FriendRequest", contract);
        }

        public async Task DeleteFriendRequestAsync(string target)
        {
            await _requestService.DeleteAsync<bool>("api/FriendRequest/" + target);
        }
    }
}
