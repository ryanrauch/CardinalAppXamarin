using MapKit;
using UIKit;

namespace FlyoverKit.iOS
{
    public class FlyoverMapViewController : UIViewController
    {
        public FlyoverMapView FlyoverMapView { get; set; }

        private Flyover _flyover { get; set; }
        public Flyover Flyover
        {
            get { return _flyover; }
            set
            {
                _flyover = value;
                FlyoverMapView.Start(_flyover);
            }
        }

        public FlyoverMapViewController(
            Flyover flyover,
            MKMapType mapType = MKMapType.Satellite,
            FlyoverCameraConfiguration configuration = null)
        {
            if (configuration == null)
            {
                configuration = new FlyoverCameraConfiguration(FlyoverCameraConfigurationTheme.Default);
            }
            FlyoverMapView = new FlyoverMapView(mapType, configuration);
            Flyover = flyover;
            FlyoverMapView.Start(flyover);
        }

        ~FlyoverMapViewController()
        {
            FlyoverMapView.Stop();
        }

        public override void LoadView()
        {
            base.LoadView();
            this.View = FlyoverMapView;
        }
    }
}