using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardinalAppXamarin.ViewModels;
using CardinalLibrary.DataContracts;

namespace CardinalAppXamarin.Services.Interfaces
{
    public interface IFriendRequestService
    {
        Task<List<FriendRequestContract>> GetAllFriendRequestsAsync();
        Task<FriendRequestContract> GetFriendRequestAsync(string target, string initiator);
        Task PostFriendRequestAsync(FriendRequestContract contract);
        Task InitializeDataAsync();
    }
}