using CardinalAppXamarin.Services.Interfaces;
using CardinalAppXamarin.ViewModels.Base;
using CardinalLibrary.DataContracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace CardinalAppXamarin.ViewModels
{
    public class MainMapViewModel : ViewModelBase
    {
        private readonly IRequestService _requestService;
        private readonly IHexagonal _hexagonal;
        private readonly IHeatGradientService _heatGradientService;

        public MainMapViewModel(
            IRequestService requestService,
            IHexagonal hexagonal,
            IHeatGradientService heatGradientService)
        {
            _requestService = requestService;
            _hexagonal = hexagonal;
            _heatGradientService = heatGradientService;
            Header = "test";
        }

        public String Header { get; set; }

        private ObservableCollection<Polygon> _polygons { get; set; }
        public ObservableCollection<Polygon> Polygons
        {
            get { return _polygons; }
            set
            {
                _polygons = value;
                RaisePropertyChanged(() => Polygons);
            }
        }

        private IList<CurrentLayerContract> _currentLayerContracts { get; set; }
        private IList<UserInfoContract> _userInfoContracts { get; set; }

        public override async Task OnAppearing()
        {
            await base.OnAppearing();
            await RefreshData();
        }

        public async Task RefreshData()
        {
            _currentLayerContracts = await _requestService.GetAsync<List<CurrentLayerContract>>("api/UserLocation");
            _userInfoContracts = await _requestService.GetAsync<List<UserInfoContract>>("api/UserInfo");
        }
    }
}
