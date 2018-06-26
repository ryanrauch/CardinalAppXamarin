using CardinalAppXamarin.Services.Interfaces;
using CardinalAppXamarin.Views.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CardinalAppXamarin.Services
{
    public class SinglePageNavigationService : INavigationService
    {
        protected Application CurrentApplication
        {
            get { return Application.Current; }
        }

        public void Initialize()
        {
            CurrentApplication.MainPage = new InitialView();
        }

        public Task NavigatePopAsync()
        {
            throw new NotImplementedException();
        }

        public Task NavigatePushAsync<T>(T page) where T : Page
        {
            throw new NotImplementedException();
        }

        public Task NavigatePushAsync<T>(T page, object param) where T : Page
        {
            throw new NotImplementedException();
        }

        public void NavigateToLogin()
        {
            CurrentApplication.MainPage = new LoginView();
        }

        public void NavigateToMain()
        {
            CurrentApplication.MainPage = new ZoneHexagonView();
        }
    }
}
