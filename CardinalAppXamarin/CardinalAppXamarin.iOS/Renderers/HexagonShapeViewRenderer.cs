using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CardinalAppXamarin.Controls;
using CardinalAppXamarin.iOS.Renderers;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(HexagonShapeView), typeof(HexagonShapeViewRenderer))]
namespace CardinalAppXamarin.iOS.Renderers
{
    public class HexagonShapeViewRenderer : VisualElementRenderer<HexagonShapeView>
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
        }
    }
}