using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardinalAppXamarin.iOS.DependencyService;
using CardinalAppXamarin.Models;
using CardinalAppXamarin.Services;
using CardinalAppXamarin.Services.Interfaces;
using Contacts;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(PhoneContactService))]
namespace CardinalAppXamarin.iOS.DependencyService
{
    public class PhoneContactService : IPhoneContactService
    {
        public async Task<IEnumerable<PhoneContact>> GetAllContactsAsync()
        {
            var keysToFetch = new[] 
            {
                CNContactKey.GivenName,
                CNContactKey.FamilyName,
                CNContactKey.PhoneNumbers
            };
            NSError error;
            //var containerId = new CNContactStore().DefaultContainerIdentifier;
            // using the container id of null to get all containers.
            // If you want to get contacts for only a single container type, 
            // you can specify that here
            var contactList = new List<CNContact>();

            await Task.Run(() =>
            {
                using (var store = new CNContactStore())
                {
                    var allContainers = store.GetContainers(null, out error);
                    foreach (var container in allContainers)
                    {
                        try
                        {
                            using (var predicate = CNContact.GetPredicateForContactsInContainer(container.Identifier))
                            {
                                var containerResults = store.GetUnifiedContacts(predicate, keysToFetch, out error);
                                contactList.AddRange(containerResults);
                            }
                        }
                        catch
                        {
                            // ignore missed contacts from errors
                        }
                    }
                }
            });
            var contacts = new List<PhoneContact>();

            foreach (var item in contactList)
            {
                var numbers = item.PhoneNumbers;
                if (numbers != null)
                {
                    foreach (var item2 in numbers)
                    {
                        contacts.Add(new PhoneContact
                        {
                            FirstName = item.GivenName,
                            LastName = item.FamilyName,
                            PhoneNumber = item2.Value.StringValue
                        });
                    }
                }
            }
            return contacts;
        }
    }
}