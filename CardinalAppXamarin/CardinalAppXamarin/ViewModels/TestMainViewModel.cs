using CardinalAppXamarin.ViewModels.Base;
using CardinalLibrary;
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

        private ObservableCollection<HexagonButtonViewModel> _menuModels { get; set; }
        public ObservableCollection<HexagonButtonViewModel> MenuModels
        {
            get { return _menuModels; }
            set
            {
                _menuModels = value;
                RaisePropertyChanged(() => MenuModels);
            }
        }

        private ObservableCollection<HexagonButtonViewModel> _listModels { get; set; }
        public ObservableCollection<HexagonButtonViewModel> ListModels
        {
            get { return _listModels; }
            set
            {
                _listModels = value;
                RaisePropertyChanged(() => ListModels);
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
            MenuModels = new ObservableCollection<HexagonButtonViewModel>();
            Color menuColor = Color.FromRgba(0xCC, 0x00, 0x00, 0xFF);
            Color menuSelectedColor = Color.FromRgba(0x93, 0x00, 0x00, 0xFF);
            MenuModels.Add(new HexagonButtonViewModel()
            {
                HexBackgroundColor = menuSelectedColor,
                HexPointyTop = true,
                HexRadius = 40d,
                HexTextColor = Color.White,
                HexFAText = FontAwesomeSolidConstants.Cube
            });
            MenuModels.Add(new HexagonButtonViewModel()
            {
                HexBackgroundColor = menuColor,
                HexPointyTop = true,
                HexRadius = 40d,
                HexTextColor = Color.White,
                HexFAText = FontAwesomeSolidConstants.MapMarkedAlt
            });
            MenuModels.Add(new HexagonButtonViewModel()
            {
                HexBackgroundColor = menuSelectedColor,
                HexPointyTop = true,
                HexRadius = 40d,
                HexTextColor = Color.White,
                HexFAText = FontAwesomeSolidConstants.AddressCard
            }); MenuModels.Add(new HexagonButtonViewModel()
            {
                HexBackgroundColor = menuSelectedColor,
                HexPointyTop = true,
                HexRadius = 40d,
                HexTextColor = Color.White,
                HexFAText = FontAwesomeSolidConstants.Cog
            });

            Random r = new Random();
            Models = new ObservableCollection<HexagonButtonViewModel>();
            ListModels = new ObservableCollection<HexagonButtonViewModel>();
            for(int i = 0; i < 5; ++i)
            {
                ListModels.Add(new HexagonButtonViewModel()
                {
                    HexBackgroundColor = Color.FromRgba(0xF5, 0xF5, 0xF6, 0xFF),
                    HexBorderColor = menuColor,
                    HexBorderSize = 4f,
                    HexPointyTop = true,
                    HexRadius = 40d,
                    HexTextColor = menuColor,
                    HexFAText = FontAwesomeSolidConstants.GlobeAmericas
                });
                Models.Add(new HexagonButtonViewModel()
                {
                    HexText = "Rainey", //i.ToString() + " " + r.Next(999999).ToString(),
                    HexBackgroundColor = Color.FromRgba(0xF5, 0xF5, 0xF6, 0xFF),
                    HexBorderColor = menuColor,
                    HexBorderSize = 4d,
                    HexPointyTop = true,
                    HexRadius = 40d,
                    HexTextColor = menuColor,
                    HexFAText = FontAwesomeSolidConstants.GlassMartini
                });
            }
            return Task.CompletedTask;
        }
    }
}
