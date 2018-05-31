using CardinalAppXamarin.Controls;
using CardinalAppXamarin.iOS.Renderers;
using FlyoverKit.iOS;
using MapKit;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(FlyoverKitMap), typeof(FlyoverKitMapRenderer))]
namespace CardinalAppXamarin.iOS.Renderers
{
    public class FlyoverKitMapRenderer : MapRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                var nativeMap = Control as MKMapView;
            }

            if (e.NewElement != null)
            {
                var formsMap = (FlyoverKitMap)e.NewElement;
                var nativeMap = Control as MKMapView;

                FlyoverMapView flyoverMapView = new FlyoverMapView(MKMapType.Satellite,
                                                                   new FlyoverCameraConfiguration(FlyoverCameraConfigurationTheme.Default));
                SetNativeControl(flyoverMapView);
                flyoverMapView.Start(new Flyover(FlyoverAwesomePlace.NewYorkStatueOfLiberty));
            }
        }
    }
}