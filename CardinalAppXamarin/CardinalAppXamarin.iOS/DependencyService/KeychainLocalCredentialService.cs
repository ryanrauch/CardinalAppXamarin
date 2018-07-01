using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CardinalAppXamarin.iOS.DependencyService;
using CardinalAppXamarin.Services.Interfaces;
using Foundation;
using Security;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(KeychainLocalCredentialService))]
namespace CardinalAppXamarin.iOS.DependencyService
{
    public class KeychainLocalCredentialService : ILocalCredentialService
    {
        public string Password => throw new NotImplementedException();

        public string UserName => throw new NotImplementedException();

        public void DeleteCredentials()
        {
            throw new NotImplementedException();
        }

        public void SaveCredentials(string userName, string password)
        {
            var s = new SecRecord(SecKind.GenericPassword)
            {
                Label = "Item Label",
                Description = "Item description",
                Account = Constants.ApplicationName,
                Service = "CardinalPassword",
                Comment = "CardinalUsernamePassword",
                ValueData = NSData.FromString(password),
                Generic = NSData.FromString("foo")
            };

            var err = SecKeyChain.Add(s);

            //if (err != SecStatusCode.Success && err != SecStatusCode.DuplicateItem)
            //    DisplayMessage(this, "Error adding record: {0}", err);


            //var record = new SecRecord(SecKind.GenericPassword)
            //{
            //    Service = Constants.ApplicationName,
            //    Account = userName,
            //    ValueData = NSData.FromString(userName, NSStringEncoding.UTF8),
            //    Accessible = SecAccessible.WhenUnlocked,
            //    Generic = NSData.FromString(userName, NSStringEncoding.UTF8)
            //};

            //var statusCode = SecKeyChain.Add(record);
            //if(statusCode.Equals())
        }
    }
}