using Foundation;
using MapKit;
using System;
using UIKit;

namespace FlyoverKit.iOS
{
    public class FlyoverCamera
    {
        private FlyoverCameraConfiguration _configuration { get; set; }
        public FlyoverCameraConfiguration Configuration
        {
            get { return _configuration; }
            set
            {
                _animator?.ForceStopAnimation();
                _configuration = value;
                _mapCamera.Altitude = _configuration.Altitude;
                _mapCamera.Pitch = new nfloat(_configuration.Pitch);
                //if (_flyover != null)
                //{
                //    PerformFlyover(_flyover);
                //}
            }
        }

        public FlyoverCameraState State { get; set; }

        public UIViewAnimationCurve Curve { get; set; } = UIViewAnimationCurve.Linear;

        private MKMapView _mapView { get; set; }

        private MKMapCamera _mapCameraValue { get; set; }
        private MKMapCamera _mapCamera
        {
            get
            {
                if (_mapCameraValue == null)
                {
                    var camera = new MKMapCamera();
                    camera.Altitude = Configuration.Altitude;
                    camera.Pitch = new nfloat(Configuration.Pitch);
                    _mapCameraValue = camera;
                }
                return _mapCameraValue;
            }
            set
            {
                _mapCameraValue = value;
            }
        }

        private Flyover _flyover { get; set; }

        private UIViewPropertyAnimator _animator { get; set; }

        private NSObject _notificationTokenWillResign { get; set; }
        private NSObject _notificationTokenDidBecomeActive { get; set; }

        public FlyoverCamera(
            MKMapView mapView,
            FlyoverCameraConfiguration configuration = null)
        {
            _mapView = mapView;
            if (configuration == null)
            {
                configuration = new FlyoverCameraConfiguration(FlyoverCameraConfigurationTheme.Default);
            }
            Configuration = configuration;
            State = FlyoverCameraState.Stopped;
            _notificationTokenWillResign = UIApplication.Notifications.ObserveWillResignActive(ApplicationWillResignActive);
            _notificationTokenDidBecomeActive = UIApplication.Notifications.ObserveDidBecomeActive(ApplicationDidBecomeActive);
        }

        ~FlyoverCamera()
        {
            Stop();
            //Remove observer
            if (_notificationTokenWillResign != null)
            {
                _notificationTokenWillResign.Dispose();
            }
            _notificationTokenWillResign = null;
            if (_notificationTokenDidBecomeActive != null)
            {
                _notificationTokenDidBecomeActive.Dispose();
            }
            _notificationTokenDidBecomeActive = null;
        }

        public void Start(Flyover flyover)
        {
            // Set flyover
            _flyover = flyover;
            if (UIApplication.SharedApplication.ApplicationState != UIApplicationState.Active)
            {
                // Return out of function
                return;
            }
            // Change state
            State = FlyoverCameraState.Started;
            // Stop current animation
            _animator?.ForceStopAnimation();
            // Set center coordinate
            _mapCamera.CenterCoordinate = flyover.Coordinate;
            // Check if duration is zero or the current mapView camera
            // equals nearly the same to the current flyover
            if (new Flyover(_mapView.Camera).NearlyEquals(_flyover))
            {
                // Simply perform flyover as we are still looking at the same coordinate
                PerformFlyover(flyover);
            }
            else if (Configuration.RegionChangeAnimation == RegionChangeAnimation.Animated
                  && Configuration.Duration > 0)
            {
                // Apply StartAnimationMode animated
                // Initialize start animator
                var startAnimator = new UIViewPropertyAnimator(
                    duration: Configuration.Duration,
                    curve: Curve,
                    animations: () => {
                        _mapView.Camera = _mapCamera;
                    });
                // Add completion
                startAnimator.SetCompletion((x) => PerformFlyover(_flyover));
                // Start animation
                startAnimator.StartAnimation();
            }
            else
            {
                // No animation should be applied
                // Set MapView Camera to look at coordinate
                _mapView.Camera = _mapCamera;
                // Perform flyover
                PerformFlyover(_flyover);
            }
        }

        public void Stop()
        {
            // Change state
            State = FlyoverCameraState.Stopped;
            // Unwrap MapView Camera Heading and fractionComplete
            if (_mapView == null
               || _animator == null)
            {
                _animator?.ForceStopAnimation();
                _animator = null;
                return;
            }
            var heading = _mapView.Camera.Heading;
            var fractionComplete = _animator.FractionComplete;
            // Force stop the animation
            _animator?.ForceStopAnimation();
            // Initialize Animator with stop animation
            _animator = new UIViewPropertyAnimator(
                duration: 0,
                curve: Curve,
                animations: () => {
                    // Subtract the HeadingStep from current heading to retrieve start value
                    heading -= Configuration.HeadingStep;
                    // Initialize the percentage of the completed heading step
                    var percentageCompletedHeadingStep = (double)fractionComplete * Configuration.HeadingStep;
                    // Set MapCamera Heading
                    _mapCamera.Heading = (heading + percentageCompletedHeadingStep) % 360;
                    // Set MapView Camera
                    _mapView.Camera = _mapCamera;
                });
            // Start animation
            _animator?.StartAnimation();
            // Clear animator as animation has been handled
            _animator = null;
        }

        private void PerformFlyover(Flyover flyover)
        {
            if (!flyover.Coordinate.IsValid())
            {
                return;
            }
            if (_mapView == null)
            {
                return;
            }
            // Increase heading by heading step for _mapCamera
            _mapCamera.Heading += Configuration.HeadingStep;
            _mapCamera.Heading = _mapCamera.Heading % 360;
            // Initialize UIViewPropertyAnimator
            _animator = new UIViewPropertyAnimator(
                                Configuration.Duration,
                                Curve,
                                animations: () => {
                                    _mapView.Camera = _mapCamera;
                                });
            // Add completion
            _animator?.SetCompletion((a) => {
                if (_flyover.NearlyEquals(flyover))
                {
                    PerformFlyover(flyover);
                }
            });
            // Start animation
            _animator?.StartAnimation();
        }

        /// <summary>
        /// UIApplicationWillResignActive notification handler
        /// </summary>
        private void ApplicationWillResignActive(object sender, NSNotificationEventArgs args)
        {
            if (State != FlyoverCameraState.Stopped)
            {
                // Stop flyover as application is no longer active
                Stop();
            }
            else
            {
                // Clear flyover to prevent start on DidBecomeActive
                _flyover = null;
            }
        }

        private void ApplicationDidBecomeActive(object sender, NSNotificationEventArgs args)
        {
            // Start if _flyover is available
            if (_flyover != null)
            {
                Start(_flyover);
            }
        }
    }
}