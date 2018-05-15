using CardinalAppXamarin.Services.Interfaces;
using CardinalAppXamarin.ViewModels.Base;
using CardinalLibrary.DataContracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
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
            //Header = "test";
        }

        //public String Header { get; set; }

        private ObservableCollection<Polygon> _polygons { get; set; } = new ObservableCollection<Polygon>();
        public ObservableCollection<Polygon> Polygons
        {
            get { return _polygons; }
            set
            {
                _polygons = value;
                RaisePropertyChanged(() => Polygons);
            }
        }
        public Command<MapClickedEventArgs> MapClickedCommand => new Command<MapClickedEventArgs>(
            args =>
            {
                var position = args.Point;
                var polygon = new Polygon();
                polygon.Positions.Add(position);
                polygon.Positions.Add(new Position(position.Latitude - 0.02d, position.Longitude - 0.01d));
                polygon.Positions.Add(new Position(position.Latitude - 0.02d, position.Longitude + 0.01d));
                polygon.Positions.Add(position);

                polygon.IsClickable = true;
                polygon.StrokeColor = Color.Green;
                polygon.StrokeWidth = 3f;
                polygon.FillColor = Color.FromRgba(255, 0, 0, 64);
                polygon.Tag = "POLYGON"; // Can set any object

                Polygons.Add(polygon);
            });

        private List<CurrentLayerContract> _currentLayerContracts { get; set; }
        private List<UserInfoContract> _userInfoContracts { get; set; }

        public override async Task OnAppearingAsync()
        {
            var layers = await _requestService.GetAsync<List<CurrentLayerContract>>("api/UserLocation");
            var info = await _requestService.GetAsync<List<UserInfoContract>>("api/UserInfo");
            _currentLayerContracts = layers.ToList();
            _userInfoContracts = info.ToList();
        }
    }
}
