using System.Threading.Tasks;
using CardinalAppXamarin.ViewModels.Base;
using CardinalAppXamarin.Views.Base;
using Xamarin.Forms;

namespace CardinalAppXamarin.Services.Interfaces
{
    public interface INavigationService
    {
        void Initialize();
        void NavigateToMain();
        void NavigateToLogin();
        void NavigateToZones();
        Task NavigatePopAsync();
        Task NavigatePushAsync<T>(T page) where T : Page;
    }
}