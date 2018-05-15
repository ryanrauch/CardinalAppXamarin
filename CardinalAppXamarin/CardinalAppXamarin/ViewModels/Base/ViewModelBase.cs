using CardinalAppXamarin.Services.Interfaces;
using System.Threading.Tasks;

namespace CardinalAppXamarin.ViewModels.Base
{
    public abstract class ViewModelBase : ExtendedBindableObject
    {
        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public ViewModelBase() { }

        public abstract Task OnAppearingAsync();
    }
}
