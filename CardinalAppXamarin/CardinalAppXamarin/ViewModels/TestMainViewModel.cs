using CardinalAppXamarin.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CardinalAppXamarin.ViewModels
{
    public class TestMainViewModel : ViewModelBase
    {
        private ObservableCollection<HexagonButtonViewModel> _models { get; set; }
        public ObservableCollection<HexagonButtonViewModel> Models
        {
            get { return _models; }
            set
            {
                _models = value;
                RaisePropertyChanged(() => Models);
            }
        }

        private String _tappedText { get; set; } = "999123";
        public String TappedText
        {
            get { return _tappedText; }
            set
            {
                _tappedText = value;
                RaisePropertyChanged(() => TappedText);
            }
        }

        public ICommand TapCommand => new Command<HexagonButtonViewModel>(TapCommandHandler);
        private void TapCommandHandler(HexagonButtonViewModel vm)
        {
            TappedText = vm.HexText;
        }

        public override Task OnAppearingAsync()
        {
            Random r = new Random();
            Models = new ObservableCollection<HexagonButtonViewModel>();
            for(int i = 0; i < 15; ++i)
            {
                Char fa = (Char)r.Next(0xf13d,0xf410);
                Models.Add(new HexagonButtonViewModel()
                {
                    HexText = i.ToString() + " " + r.Next(999999).ToString(),
                    HexBackgroundColor = Color.Crimson,
                    HexPointyTop = true,
                    HexRadius = 40d,
                    HexTextColor = Color.White,
                    //HexFAText = fa.ToString()
                    HexFAText = "\uf279"
                });
            }
            return Task.CompletedTask;
        }
    }
}
