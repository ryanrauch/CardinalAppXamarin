using CardinalAppXamarin.Controls;
using CardinalAppXamarin.iOS.Renderers;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(GradientView), typeof(GradientViewRenderer))]
namespace CardinalAppXamarin.iOS.Renderers
{
    public class GradientViewRenderer : ViewRenderer
    {
        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            GradientView gradientView = (GradientView)this.Element;
            CGColor start = gradientView.StartColor.ToCGColor();
            CGColor middle = gradientView.MiddleColor.ToCGColor();
            CGColor end = gradientView.EndColor.ToCGColor();
            double percent = gradientView.Percent;

            var gradientLayer = new CAGradientLayer()
            {
                Frame = rect,
                Colors = new CGColor[] { start, middle, end },
                StartPoint = new CGPoint(0, 0.5),
                EndPoint = new CGPoint(1, 0.5),
                Locations = new NSNumber[] { 0.0, percent, 1.0 }
            };
            NativeView.Layer.InsertSublayer(gradientLayer, 0);
        }
    }
}
