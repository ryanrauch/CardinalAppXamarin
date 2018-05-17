using CardinalAppXamarin.Services.Interfaces;
using CardinalLibrary.DataContracts;
using System.Threading.Tasks;

namespace CardinalAppXamarin.Services.Mock
{
    public class MockDataUpdateService : IMockDataUpdateService
    {
        private readonly IRequestService _requestService;

        public MockDataUpdateService(
            IRequestService requestService)
        {
            _requestService = requestService;
        }

        public async Task InitializeMockData()
        {
            MockDataInitializeContract mock = new MockDataInitializeContract()
            {
                Email = "rauch.ryan@gmail.com",
                Latitude = 30.4031,
                Longitude = -97.7345
            };
            await _requestService.PostAsync<MockDataInitializeContract>("api/MockData", mock);
        }
    }
}
