using CardinalAppXamarin.Services.Interfaces;
using CardinalAppXamarin.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CardinalAppXamarin.ViewModels
{
    public class InitialViewModel : ViewModelBase
    {
        private readonly IRequestService _requestService;
        private readonly ILocalCredentialService _localCredentialService;

        public InitialViewModel(
            IRequestService requestService,
            ILocalCredentialService localCredentialService)
        {
            _requestService = requestService;
            _localCredentialService = localCredentialService;
            //InitializeAsync();
        }

        public override async Task OnAppearing()
        {
            await base.OnAppearing();
        }
        public override async Task OnDisappearing()
        {
            await base.OnDisappearing();
        }
    }
}
