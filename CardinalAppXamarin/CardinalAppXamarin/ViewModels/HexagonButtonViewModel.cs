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
        }

        private double _hexRadius { get; set; } = 40d;
        public double HexRadius
        {
            get { return _hexRadius; }
            set
            {
                _hexRadius = value;
                RaisePropertyChanged(() => HexRadius);
            }
        }

        private bool _hexPointyTop { get; set; } = true;
        public bool HexPointyTop
        {
            get { return _hexPointyTop; }
            set
            {
                _hexPointyTop = value;
                RaisePropertyChanged(() => HexPointyTop);
            }
        }

        private Color _hexTextColor { get; set; } = Color.White;
        public Color HexTextColor
        {
            get { return _hexTextColor; }
            set
            {
                _hexTextColor = value;
                RaisePropertyChanged(() => HexTextColor);
            }
        }

        private Color _hexBackgroundColor { get; set; } = Color.Crimson;
        public Color HexBackgroundColor
        {
            get { return _hexBackgroundColor; }
            set
            {
                _hexBackgroundColor = value;
                RaisePropertyChanged(() => HexBackgroundColor);
            }
        }

        private string _hexText { get; set; } = String.Empty;
        public string HexText
        {
            get { return _hexText; }
            set
            {
                _hexText = value;
                RaisePropertyChanged(() => HexText);
            }
        }

        private string _hexFAText { get; set; } = String.Empty;
        public string HexFAText
        {
            get { return _hexFAText; }
            set
            {
                _hexFAText = value;
                RaisePropertyChanged(() => HexFAText);
            }
        }

        public override Task OnAppearingAsync()
        {
            return Task.CompletedTask;
        }
    }
}
