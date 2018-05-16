using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace CardinalAppXamarin.Services.Interfaces
{
    public interface IGeolocatorService
    {
        Task<Position> GetCurrentPosition();
        Task<bool> IsLocationAvailable();
    }
}