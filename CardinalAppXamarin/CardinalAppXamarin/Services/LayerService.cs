﻿using CardinalAppXamarin.Services.Interfaces;
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

        public LayerService(
            IRequestService requestService,
            IHexagonal hexagonal)
        {
            _requestService = requestService;
            _hexagonal = hexagonal;
            _lastUpdated = DateTime.MinValue;
        }

        private DateTime _lastUpdated { get; set; }
        public DateTime LastUpdated => _lastUpdated;

        private List<CurrentLayerContract> _currentLayerContracts { get; set; }
        private List<UserInfoContract> _userInfoContracts { get; set; }

        public async Task InitializeData()
        {
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
            return FindPolygonTagContainingUser(userId, _hexagonal.Layers[0]);
        }

        public string FindPolygonTagContainingUser(string userId, int layer)
        {
            var current = _currentLayerContracts.FirstOrDefault(c => c.UserId.Equals(userId));
            if (current == null)
            {
                return String.Empty;
            }
            var tag = SplitLayersDelimited(current.LayersDelimited).FirstOrDefault(s => DelimitedBelongsToLayer(s, layer));
            if(tag == null)
            {
                return String.Empty;
            }
            return tag;
        }

        public int NumberOfUsersInsidePolygonTag(string layerDelimited)
        {
            return _currentLayerContracts.Where(c => SplitLayersDelimited(c.LayersDelimited).Contains(layerDelimited)).Count();
        }

        public IList<UserInfoContract> UsersInsidePolygonTag(string layerDelimited)
        {
            var users = from u in _userInfoContracts
                        join c in _currentLayerContracts on u.Id equals c.UserId
                        select u;
            return users.ToList();
        }
    }
}