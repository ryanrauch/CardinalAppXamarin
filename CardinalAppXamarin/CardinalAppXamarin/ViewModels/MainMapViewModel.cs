using CardinalAppXamarin.Services.Interfaces;
using CardinalAppXamarin.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardinalAppXamarin.ViewModels
{
    public class MainMapViewModel : ViewModelBase
    {
        private readonly IHexagonal _hexagonal;
        private readonly IHeatGradientService _heatGradientService;

        public MainMapViewModel(
            IHexagonal hexagonal,
            IHeatGradientService heatGradientService)
        {
            _hexagonal = hexagonal;
            _heatGradientService = heatGradientService;
            Header = "test";
        }

        public String Header { get; set; }
    }
}
