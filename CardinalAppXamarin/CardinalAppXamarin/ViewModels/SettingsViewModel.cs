using CardinalAppXamarin.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CardinalAppXamarin.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private bool _visible { get; set; }
        public bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                RaisePropertyChanged(() => Visible);
            }
        }
        public SettingsViewModel()
        {
        }

        public override Task OnAppearingAsync()
        {
            return Task.CompletedTask;
        }
    }
}
