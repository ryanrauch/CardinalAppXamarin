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

[assembly: ExportRenderer(typeof(HexagonButtonView), typeof(HexagonButtonViewRenderer))]
namespace CardinalAppXamarin.iOS.Renderers
{
    public class HexagonButtonViewRenderer : ViewRenderer<HexagonButtonView, UIView>
    {
        public HexagonButtonViewRenderer()
        {
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == HexagonButtonView.RadiusProperty.PropertyName)
            {
                SetNeedsDisplay();
            }
            if (e.PropertyName == HexagonButtonView.PointyTopProperty.PropertyName)
            {
                SetNeedsDisplay();
            }
            if (e.PropertyName == HexagonButtonView.TextProperty.PropertyName)
            {
                SetNeedsDisplay();
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<HexagonButtonView> e)
        {
            base.OnElementChanged(e);
            if(Control == null)
            {
                //var hexView = new UIView();
                //hexView.ClipsToBounds = false;
                //hexView.UserInteractionEnabled = true;
                //SetNativeControl(hexView);
            }
            if(Control != null)
            {
                Control.ClipsToBounds = false;
               
            }
            //if (e.OldElement != null)
            //{
            //    // Cleanup resources and remove event handlers for this element.
            //}
            //if (e.NewElement != null)
            //{
            //    // Use the properties of this element to assign to the native control
            //    // which is assigned to the base.Control property
            //}
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            if (Element == null)
            {
                return;
            }

            var cx = rect.Width / 2f;
            var cy = rect.Height / 2f;

            var colorSpace = CGColorSpace.CreateDeviceRGB();
            var context = UIGraphics.GetCurrentContext();

            var gradientColors = new CGColor[] { UIColor.LightGray.CGColor, Element.BackgroundColor.ToCGColor() };
            var gradientLocations = new nfloat[] { 0f, 1f };
            var gradient = new CGGradient(colorSpace, gradientColors, gradientLocations);

            var shadow = new NSShadow()
            {
                ShadowColor = UIColor.Black,
                ShadowOffset = new CGSize(3, 3),
                ShadowBlurRadius = 5
            };

            // Draw the hexagon (POINTY-TOP ONLY)
            var radius = Element.Radius;
            var points = new List<CGPoint>();
            for (int i = 0; i < 6; ++i)
            {
                points.Add(new CGPoint(cx + radius * Math.Cos((i * 60 - 30) * Math.PI / 180f),
                                       cy + radius * Math.Sin((i * 60 - 30) * Math.PI / 180f)));
            }
            
            var midPoint = new CGPoint(0.5 * (points[0].X + points[1].X), 0.5 * (points[0].Y + points[1].Y));
            var path = new CGPath();
            path.MoveToPoint(midPoint);
            for (var i = 0; i < points.Count; ++i)
            {
                path.AddLineToPoint(new CGPoint(points[(i + 1) % points.Count].X, points[(i + 1) % points.Count].Y));
            }
            path.CloseSubpath();
            context.AddPath(path);
            
            context.SetFillColor(Element.BackgroundColor.ToCGColor());
            //context.SaveState();
            context.SetShadow(shadow.ShadowOffset, shadow.ShadowBlurRadius, UIColor.Gray.CGColor);
            context.DrawPath(CGPathDrawingMode.Fill);
            //context.BeginTransparencyLayer();
            //path.AddClip();
            //context.DrawLinearGradient(gradient, points[0], points[1], CGGradientDrawingOptions.DrawsAfterEndLocation);
            //context.EndTransparencyLayer();
            //context.RestoreState();

            // Draw Text
            UILabel label = new UILabel(rect)
            {
                Text = Element.Text,
                TextAlignment = UITextAlignment.Center,
                TextColor = Element.TextColor.ToUIColor(),
                Font = UIFont.FromName("highlandgothiclightflf", Element.FontSize)
                //Font = UIFont.SystemFontOfSize(12f)
            };
            NativeView.AddSubview(label);
        }

        //protected virtual void DrawPoints(CGContext context, List<CGPoint> points, bool fill, bool stroke, float radius)
        //{
        //    if (points == null || points.Count == 0)
        //        return;
        //    var midPoint = new CGPoint(0.5 * (points[0].X + points[2].X), 
        //                               0.5 * (radius));
        //    var path = new CGPath();
        //    path.MoveToPoint(midPoint);

        //    for (var i = 0; i < points.Count; ++i)
        //    {
        //        path.AddLineToPoint(points[(i + 1) % points.Count].X, points[(i + 1) % points.Count].Y);
        //    }
        //    path.CloseSubpath();
        //    context.AddPath(path);
        //    DrawPath(context, fill, stroke);
        //}

        //private void DrawPath(CGContext context, bool fill, bool stroke)
        //{
        //    if (fill && stroke)
        //        context.DrawPath(CGPathDrawingMode.FillStroke);
        //    else if (fill)
        //        context.DrawPath(CGPathDrawingMode.Fill);
        //    else if (stroke)
        //        context.DrawPath(CGPathDrawingMode.Stroke);
        //}
    }
}