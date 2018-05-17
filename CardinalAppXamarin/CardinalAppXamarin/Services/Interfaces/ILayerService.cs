using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardinalAppXamarin.Models;
using CardinalAppXamarin.ViewModels;
using CardinalLibrary.DataContracts;

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
    }
}