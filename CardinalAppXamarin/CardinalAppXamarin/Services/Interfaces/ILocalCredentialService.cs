
namespace CardinalAppXamarin.Services.Interfaces
{
    public interface ILocalCredentialService
    {
        string Password { get; }
        string UserName { get; }

        void DeleteCredentials();
        void SaveCredentials(string userName, string password);
    }
}
