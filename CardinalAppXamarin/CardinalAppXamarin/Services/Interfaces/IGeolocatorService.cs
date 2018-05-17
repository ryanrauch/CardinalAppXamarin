using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace CardinalAppXamarin.Services.Interfaces
{
    public interface IGeolocatorService
    {
        Position LastRecordedPosition { get; }
        Task<Position> GetCurrentPosition();
        Task<bool> IsLocationAvailable();
    }
}