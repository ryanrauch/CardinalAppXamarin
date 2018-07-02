using CardinalAppXamarin.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CardinalAppXamarin.ViewModels
{
    public class HexagonButtonViewModel : ViewModelBase
    {
        public HexagonButtonViewModel()
        {
            //Random r = new Random();
            //HexText = r.Next(999999).ToString();
        }

        public double HexRadius => 40d;
        public bool HexPointyTop => true;
        public Color HexTextColor => Color.White;
        public Color HexBackgroundColor => Color.Crimson;

        private string _hexText { get; set; }
        public string HexText
        {
            get { return _hexText; }
            set
            {
                _hexText = value;
                RaisePropertyChanged(() => HexText);
            }
        }

        public override Task OnAppearingAsync()
        {
            return Task.CompletedTask;
        }
    }
}
