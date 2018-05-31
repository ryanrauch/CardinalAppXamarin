using CardinalAppXamarin.ViewModels.Base;
using System.Threading.Tasks;

namespace CardinalAppXamarin.ViewModels
{
    public class HeaderViewModel : ViewModelBase
    {
        public HeaderViewModel()
        {

        }

        public string TitleText => "Cardinal";

        public string WelcomeText => "Welcome, MockUser.";

        public override Task OnAppearingAsync()
        {
            return Task.CompletedTask;
        }
    }
}
