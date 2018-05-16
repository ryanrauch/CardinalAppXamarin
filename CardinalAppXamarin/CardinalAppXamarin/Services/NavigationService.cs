using CardinalAppXamarin.Services.Interfaces;
using CardinalAppXamarin.ViewModels.Base;
using CardinalAppXamarin.Views;
using CardinalAppXamarin.Views.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CardinalAppXamarin.Services
{
    public class NavigationService : INavigationService
    {
        protected Application CurrentApplication
        {
            get { return Application.Current; }
        }

        public void Initialize()
        {
            CurrentApplication.MainPage = new NavigationPage(new InitialView());
        }

        public void NavigateToMain()
        {
            CurrentApplication.MainPage = new NavigationPage(new MainMapView());
        }
        public void NavigateToLogin()
        {
            CurrentApplication.MainPage = new NavigationPage(new LoginView());
        }
        public async Task NavigatePushAsync<T>(T page) where T : Page
        {
            await CurrentApplication.MainPage.Navigation.PushAsync(page);
        }

        public async Task NavigatePopAsync()
        {
            //if (CurrentApplication.MainPage is MainPage)
            //{
            //    var mainPage = CurrentApplication.MainPage as MainPage;
            //    await mainPage.Detail.Navigation.PopAsync();
            //}
            //else if (CurrentApplication.MainPage != null)
            //{
            await CurrentApplication.MainPage.Navigation.PopAsync();
            //}
        }
    }
}
