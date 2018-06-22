using System.Threading.Tasks;
using Xamarin.Forms;

namespace CardinalAppXamarin.Services.Interfaces
{
    public interface INavigationService
    {
        void Initialize();
        void NavigateToMain();
        void NavigateToLogin();
        void NavigateToMainZones();
        Task NavigatePopAsync();
        Task NavigatePushAsync<T>(T page) where T : Page;
        Task NavigatePushAsync<T>(T page, object param) where T : Page;
    }
}