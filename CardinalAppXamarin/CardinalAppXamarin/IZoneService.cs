using System.Collections.Generic;
using System.Threading.Tasks;
using CardinalLibrary.DataContracts;

namespace CardinalAppXamarin.Services.Interfaces
{
    public interface IZoneService
    {
        Task<List<ZoneContract>> GetAllZoneContractsAsync();
        Task<ZoneContract> GetZoneContractAsync(string zoneId);
        Task<List<ZoneShapeContract>> GetZoneShapesAsync(string zoneId);
        Task InitializeData();
    }
}