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

        public String Header { get; set; }

        public MainMapViewModel(IHexagonal hexagonal)
        {
            _hexagonal = hexagonal;
            Header = "test";
        }
    }
}
