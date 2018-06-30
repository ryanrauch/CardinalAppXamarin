using CardinalAppXamarin.Services.Interfaces;
using CardinalAppXamarin.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CardinalAppXamarin.ViewModels
{
    public class FriendsListViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public FriendsListViewModel(
            INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public String TitleText => "Cardinal";
        public String SubtitleText => "Friends List";
        public bool BackButtonVisible => true;
        public ICommand BackButtonCommand => new Command(() => _navigationService.NavigateToMain());

        public override Task OnAppearingAsync()
        {
            return Task.CompletedTask;
        }
    }
}
