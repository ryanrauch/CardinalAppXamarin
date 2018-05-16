using CardinalAppXamarin.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace CardinalAppXamarin.Services
{
    public class DialogService : IDialogService
    {
        public async Task DisplayAlertAsync(String title, String message, String buttonText)
        {
            await App.Current.MainPage.DisplayAlert(title, message, buttonText);
        }
    }
}
