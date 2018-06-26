using CardinalAppXamarin.ViewModels.Base;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CardinalAppXamarin.ViewModels
{
    public class HeaderViewModel : ViewModelBase
    {
        public HeaderViewModel()
        {

        }

        public string TitleText => "Cardinal";
        public string SubtitleText => "Rockrose";
        public bool BackButtonVisible => false;

        public ICommand BackButtonCommand => null;

        public override Task OnAppearingAsync()
        {
            return Task.CompletedTask;
        }
    }
}
