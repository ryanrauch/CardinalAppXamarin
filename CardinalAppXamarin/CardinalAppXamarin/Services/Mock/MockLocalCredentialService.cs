using CardinalAppXamarin.Services.Interfaces;

namespace CardinalAppXamarin.Services.Mock
{
    public class MockLocalCredentialService : ILocalCredentialService
    {
        public string Password => "#lsf3FX241";

        public string UserName => "rauch.ryan@gmail.com";

        public void DeleteCredentials()
        {

        }

        public void SaveCredentials(string userName, string password)
        {

        }
    }
}
