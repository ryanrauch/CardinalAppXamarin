using System.Threading.Tasks;

namespace CardinalAppXamarin.Services.Interfaces
{
    public interface IDialogService
    {
        Task DisplayAlertAsync(string title, string message, string buttonText);
    }
}