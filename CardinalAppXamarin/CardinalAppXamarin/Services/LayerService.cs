using CardinalAppXamarin.Models;
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
    public class LayerService : ILayerService
    {
        private readonly IRequestService _requestService;
        private readonly IHexagonal _hexagonal;
        private readonly IMockDataUpdateService _mockDataUpdateService;
        private readonly IZoneService _zoneService;

        public LayerService(
            IRequestService requestService,
            IHexagonal hexagonal,
            IMockDataUpdateService mockDataUpdateService,
            IZoneService zoneService)
        {
            _requestService = requestService;
            _hexagonal = hexagonal;
            _mockDataUpdateService = mockDataUpdateService;
            _zoneService = zoneService;

            _lastUpdated = DateTime.MinValue;
        }

        private DateTime _lastUpdated { get; set; }
        public DateTime LastUpdated => _lastUpdated;

        private List<CurrentLayerContract> _currentLayerContracts { get; set; }
        private List<UserInfoContract> _userInfoContracts { get; set; }

        public async Task InitializeData()
        {
            await _mockDataUpdateService.InitializeMockData();
            await _zoneService.InitializeData();
            _currentLayerContracts = await _requestService.GetAsync<List<CurrentLayerContract>>("api/UserLocation");
            _userInfoContracts = await _requestService.GetAsync<List<UserInfoContract>>("api/UserInfo");

            if (_currentLayerContracts.Count > 0)
            {
                _lastUpdated = _currentLayerContracts.Max(c => c.TimeStamp);
            }
            else
            {
                _lastUpdated = DateTime.MinValue;
            }
        }

        private bool DelimitedBelongsToLayer(string delimited, int layer)
        {
            return delimited.StartsWith(layer.ToString() + Constants.BoundingBoxDelim);
        }

        private List<string> SplitLayersDelimited(string delimited)
        {
            return delimited.Split(Constants.LayerDelimChar).ToList();
        }

        public string FindPolygonTagContainingUser(string userId)
        {
            return FindPolygonTagContainingUser(userId, _hexagonal.Layers.Min());
        }

        public string FindPolygonTagContainingUser(string userId, int layer)
        {
            var current = _currentLayerContracts.FirstOrDefault(c => c.UserId.Equals(userId));
            if (current == null)
            {
                return String.Empty;
            }
            var tag = SplitLayersDelimited(current.LayersDelimited)
                      .FirstOrDefault(s => DelimitedBelongsToLayer(s, layer));
            if(tag == null)
            {
                return String.Empty;
            }
            return tag;
        }

        public int NumberOfUsersInsidePolygonTag(string layerDelimited)
        {
            return _currentLayerContracts.Where(c => SplitLayersDelimited(c.LayersDelimited)
                                                                          .Contains(layerDelimited))
                                                     .Count();
        }

        public List<UserInfoBriefViewCellModel> UsersInsidePolygonTagBrief(string layerDelimited)
        {
            var models = from c in _currentLayerContracts
                         join u in _userInfoContracts on c.UserId equals u.Id
                         where SplitLayersDelimited(c.LayersDelimited).Contains(layerDelimited)
                         orderby c.TimeStamp descending
                         select new UserInfoBriefViewCellModel(u, c);
            return new List<UserInfoBriefViewCellModel>(models);
        }

        public string FindZoneContainingUser(string userId)
        {
            var current = _currentLayerContracts.FirstOrDefault(c => c.UserId.Equals(userId)
                                                                     && !String.IsNullOrEmpty(c.CurrentZoneId));
            if (current != null)
            {
                return _zoneService.GetZoneContract(current.CurrentZoneId).ZoneID;
            }
            return null;
        }

        public List<UserInfoBriefViewCellModel> UsersInsizeZone(string zoneId)
        {
            var models = from c in _currentLayerContracts
                         join u in _userInfoContracts on c.UserId equals u.Id
                         where !String.IsNullOrEmpty(c.CurrentZoneId) && c.CurrentZoneId.Equals(zoneId)
                         orderby c.TimeStamp descending
                         select new UserInfoBriefViewCellModel(u, c);
            return new List<UserInfoBriefViewCellModel>(models);
        }
    }
}
