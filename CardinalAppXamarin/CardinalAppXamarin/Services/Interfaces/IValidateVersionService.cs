using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CardinalAppXamarin.Services.Interfaces
{
    public interface IValidateVersionService
    {
        Task<bool> ValidateVersionNumber(string app);
    }
}
