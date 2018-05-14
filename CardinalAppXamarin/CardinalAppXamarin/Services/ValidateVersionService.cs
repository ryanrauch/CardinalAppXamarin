using CardinalAppXamarin.Services.Interfaces;
using CardinalLibrary.DataContracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CardinalAppXamarin.Services
{
    public class ValidateVersionService : IValidateVersionService
    {
        private readonly IRequestService _requestService;

        public ValidateVersionService(IRequestService requestService)
        {
            _requestService = requestService;
        }

        public async Task<bool> ValidateVersionNumber(string app)
        {
            var option = await _requestService.GetAsync<ApplicationOptionContract>("api/ApplicationOption");
            if(option == null)
            {
                return false;
            }
            String versionDb = String.Format("{0}.{1}.{2}",
                                             option.Version,
                                             option.VersionMajor,
                                             option.VersionMinor);
            //TODO: implement actual integer logic to check >= versionDb
            return versionDb.Equals(app);
        }
    }
}
