using CardinalAppXamarin.Services.Interfaces;
using System;
using System.Linq;
using Xamarin.Auth;

namespace CardinalAppXamarin.Services
{
    public class XamarinAuthLocalCredentialService : ILocalCredentialService
    {
        public string UserName
        {
            get
            {
                var account = AccountStore.Create()
                                          .FindAccountsForService(Constants.ApplicationName)
                                          .FirstOrDefault();
                return (account != null) ? account.Username : null;
            }
        }
        public string Password
        {
            get
            {
                var account = AccountStore.Create()
                                          .FindAccountsForService(Constants.ApplicationName)
                                          .FirstOrDefault();
                return (account != null) ? account.Properties[Constants.AccountStorePasswordKey] : null;
            }
        }

        public void SaveCredentials(string userName, string password)
        {
            if (!String.IsNullOrWhiteSpace(userName) && !String.IsNullOrWhiteSpace(password))
            {
                Account account = new Account
                {
                    Username = userName
                };
                account.Properties.Add(Constants.AccountStorePasswordKey, password);
                AccountStore.Create()
                            .Save(account, Constants.ApplicationName);
            }
        }

        public void DeleteCredentials()
        {
            var account = AccountStore.Create()
                                      .FindAccountsForService(Constants.ApplicationName)
                                      .FirstOrDefault();
            if (account != null)
            {
                AccountStore.Create()
                            .Delete(account, Constants.ApplicationName);
            }
        }
    }
}
