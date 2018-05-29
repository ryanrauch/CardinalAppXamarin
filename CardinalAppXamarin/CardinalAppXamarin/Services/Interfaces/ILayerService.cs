using CardinalAppXamarin.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CardinalAppXamarin.Services.Interfaces
{
    public interface ILayerService
    {
        DateTime LastUpdated { get; }

        string FindPolygonTagContainingUser(string userId);
        string FindPolygonTagContainingUser(string userId, int layer);
        Task InitializeData();
        int NumberOfUsersInsidePolygonTag(string layerDelimited);
        List<UserInfoBriefViewCellModel> UsersInsidePolygonTagBrief(string layerDelimited);

        string FindZoneContainingUser(string userId);
        List<UserInfoBriefViewCellModel> UsersInsideZone(string zoneId);
    }
}