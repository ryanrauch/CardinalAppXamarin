using MapKit;

namespace FlyoverKit.iOS
{
    public class FlyoverMapView : MKMapView
    {
        private FlyoverCamera _flyoverCamera { get; set; }
        public FlyoverCamera FlyoverCamera
        {
            get { return _flyoverCamera; }
            set
            {
                _flyoverCamera = value;
            }
        }

        private MKMapType _flyoverMapType { get; set; }
        public MKMapType FlyoverMapType
        {
            get { return _flyoverMapType; }
            set
            {
                _flyoverMapType = value;
            }
        }

        private FlyoverCameraConfiguration _configuration { get; set; }
        public FlyoverCameraConfiguration Configuration
        {
            get { return _configuration; }
            set
            {
                _configuration = value;
            }
        }

        public FlyoverCameraState State
        {
            get { return FlyoverCamera.State; }
        }

        public FlyoverMapView(
            MKMapType mapType = MKMapType.Satellite,
            FlyoverCameraConfiguration configuration = null)
        {
            //this.Frame = CGRect.Empty; //super.init(frame: .zero)
            // Set flyover map type
            FlyoverMapType = mapType;
            if (configuration == null)
            {
                configuration = new FlyoverCameraConfiguration(FlyoverCameraConfigurationTheme.Default);
            }
            FlyoverCamera = new FlyoverCamera(this, configuration);
            // Hide compass on iOS
            ShowsCompass = false;
            ShowsBuildings = true;
        }

        ~FlyoverMapView()
        {
            Stop();
        }

        public void Start(MKAnnotation annotation)
        {
            UserInteractionEnabled = false;
            Start(new Flyover(annotation.Coordinate));
        }

        public void Start(Flyover flyover)
        {
            UserInteractionEnabled = false;
            FlyoverCamera.Start(flyover);
        }

        public void Stop()
        {
            FlyoverCamera.Stop();
            UserInteractionEnabled = true;
        }
    }
}