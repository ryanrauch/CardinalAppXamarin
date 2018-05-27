using CardinalAppXamarin.Services.Interfaces;
using CardinalLibrary.DataContracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CardinalAppXamarin.Services
{
    public class ZoneService : IZoneService
    {
        private readonly IRequestService _requestService;

        public ZoneService(
            IRequestService requestService)
        {
            _requestService = requestService;
        }

        private bool _initialized { get; set; } = false;
        private List<ZoneContract> _zoneContracts { get; set; } = new List<ZoneContract>();

        public async Task InitializeData()
        {
            _zoneContracts = await _requestService.GetAsync<List<ZoneContract>>("api/Zone");
            _initialized = true;
        }

        public async Task<List<ZoneContract>> GetAllZoneContractsAsync()
        {
            if(!_initialized)
            {
                await InitializeData();
            }
            return _zoneContracts;
        }

        public async Task<ZoneContract> GetZoneContractAsync(string zoneId)
        {
            if(!_initialized)
            {
                await InitializeData();
            }
            return _zoneContracts.Find(z => z.ZoneID.Equals(zoneId));
        }

        public async Task<List<ZoneShapeContract>> GetZoneShapesAsync(string zoneId)
        {
            if(!_initialized)
            {
                await InitializeData();
            }
            var targetZone = await GetZoneContractAsync(zoneId);
            return new List<ZoneShapeContract>(targetZone.ZoneShapes);
        }
    }
}
