using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardinalLibrary.DataContracts;

namespace CardinalAppXamarin.Services.Interfaces
{
    public interface ILayerService
    {
        DateTime LastUpdated { get; }

        Task<string> FindPolygonTagContainingUser(string userId);
        Task<string> FindPolygonTagContainingUser(string userId, int layer);
        Task InitializeData();
        Task<int> NumberOfUsersInsidePolygonTag(string layerDelimited);
        Task<IList<UserInfoContract>> UsersInsidePolygonTag(string layerDelimited);
    }
}