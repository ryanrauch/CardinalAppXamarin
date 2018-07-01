using CardinalAppXamarin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CardinalAppXamarin.Services.Interfaces
{
    public interface IPhoneContactService
    {
        Task<IEnumerable<PhoneContact>> GetAllContactsAsync();
    }
}
