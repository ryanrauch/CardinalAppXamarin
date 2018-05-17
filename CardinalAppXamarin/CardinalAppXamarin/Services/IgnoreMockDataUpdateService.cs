using CardinalAppXamarin.Services.Interfaces;
using System.Threading.Tasks;

namespace CardinalAppXamarin.Services
{
    public class IgnoreMockDataUpdateService : IMockDataUpdateService
    {
        public Task InitializeMockData()
        {
            return Task.CompletedTask;
        }
    }
}
