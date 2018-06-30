using System.Collections.Generic;
using System.Threading.Tasks;
using CardinalLibrary.DataContracts;

namespace CardinalAppXamarin.Services.Interfaces
{
    public interface IUserInfoService
    {
        Task<UserInfoContract> GetUserInfoAsync(string userId);
        Task<List<UserInfoContract>> GetAllUserInfoAsync();
        Task InitializeDataAsync();
    }
}