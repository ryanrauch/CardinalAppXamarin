using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CardinalAppXamarin.Controls;
using CardinalAppXamarin.iOS.Renderers;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(HexagonButton), typeof(HexagonButtonRenderer))]
namespace CardinalAppXamarin.iOS.Renderers
{
    public class HexagonButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }

        private void Paint(HexagonButton view)
        {
            //this.Layer.CornerRadius = (float)view.CustomBorderRadius;
            //this.Layer.BorderColor = view.CustomBorderColor.ToCGColor();
            //this.Layer.BackgroundColor = view.CustomBackgroundColor.ToCGColor();
            //this.Layer.BorderWidth = (int)view.CustomBorderWidth;
        }
    }
}