using CardinalAppXamarin.Services.Interfaces;
using CardinalLibrary.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardinalAppXamarin.Services
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IRequestService _requestService;

        public UserInfoService(
            IRequestService requestService)
        {
            _requestService = requestService;
            _initialized = false;
        }

        private bool _initialized { get; set; }
        private DateTime _lastUpdated { get; set; }
        private List<UserInfoContract> _userInfoContracts { get; set; }

        public async Task<List<UserInfoContract>> GetAllUserInfoAsync()
        {
            await CheckInitialization();
            return _userInfoContracts;
        }

        public async Task<UserInfoContract> GetUserInfoAsync(string userId)
        {
            await CheckInitialization();
            return _userInfoContracts.First(u => u.Id.Equals(userId));
        }

        private async Task CheckInitialization()
        {
            if (!_initialized
               || DateTime.Now - _lastUpdated > Constants.CardinalStaleDataTimeSpan)
            {
                await InitializeDataAsync();
            }
        }

        public async Task InitializeDataAsync()
        {
            _userInfoContracts = await _requestService.GetAsync<List<UserInfoContract>>("api/UserInfo");
            _initialized = true;
            _lastUpdated = DateTime.Now;
        }
    }
}
