using System.Threading.Tasks;
using CardinalAppXamarin.ViewModels.Base;
using CardinalAppXamarin.Views.Base;

namespace CardinalAppXamarin.Services.Interfaces
{
    public interface INavigationService
    {
        void Initialize();
        Task NavigatePopAsync();
        Task NavigatePushAsync<T>(T page) where T : ViewPageBase<ViewModelBase>;
    }
}