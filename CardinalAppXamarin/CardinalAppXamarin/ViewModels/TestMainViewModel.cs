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
            MenuModels.Add(new HexagonButtonViewModel()
            {
                HexBackgroundColor = menuColor,
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
                HexBackgroundColor = menuColor,
                HexPointyTop = true,
                HexRadius = 40d,
                HexTextColor = Color.White,
                HexFAText = FontAwesomeSolidConstants.AddressBook
            }); MenuModels.Add(new HexagonButtonViewModel()
            {
                HexBackgroundColor = menuColor,
                HexPointyTop = true,
                HexRadius = 40d,
                HexTextColor = Color.White,
                HexFAText = FontAwesomeSolidConstants.Cog
            });

            Random r = new Random();
            Models = new ObservableCollection<HexagonButtonViewModel>();
            for(int i = 0; i < 15; ++i)
            {
                //Models.Add(new HexagonButtonViewModel()
                //{
                //    HexText = i.ToString() + " " + r.Next(999999).ToString(),
                //    HexBackgroundColor = Color.FromRgba(0x00, 0x00, 0x00, 0x50),
                //    HexBorderColor = menuColor,
                //    HexBorderSize = 4f,
                //    HexPointyTop = true,
                //    HexRadius = 40d,
                //    HexTextColor = Color.White,
                //    HexFAText = FontAwesomeSolidConstants.GlassMartini
                //});
                Models.Add(new HexagonButtonViewModel()
                {
                    HexText = i.ToString() + " " + r.Next(999999).ToString(),
                    HexBackgroundColor = Color.FromRgba(0xFF, 0xFF, 0xFF, 0xFF),
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
