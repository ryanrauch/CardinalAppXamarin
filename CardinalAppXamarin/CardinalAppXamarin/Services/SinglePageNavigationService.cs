using CardinalAppXamarin.Services.Interfaces;
using CardinalAppXamarin.ViewModels.Base;
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
            CurrentApplication.MainPage = page;
            return Task.CompletedTask;
        }

        public Task NavigatePushAsync<T>(T page, object param) where T : Page
        {
            (page.BindingContext as ViewModelBase).Initialize(param);
            return NavigatePushAsync(page);
        }

        public void NavigateToFriendsList()
        {
            CurrentApplication.MainPage = new FriendsListView();
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
