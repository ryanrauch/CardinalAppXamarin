using CardinalAppXamarin.Controls;
using CardinalAppXamarin.iOS.Renderers;
using CoreGraphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(HexagonPercentView), typeof(HexagonPercentViewRenderer))]
namespace CardinalAppXamarin.iOS.Renderers
{
    public class HexagonPercentViewRenderer : VisualElementRenderer<HexagonPercentView>
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Element == null)
                return;

            switch (e.PropertyName)
            {
                case nameof(Element.IsVisible):
                case nameof(Element.ShapeColor):
                case nameof(Element.BorderColor):
                case nameof(Element.Percent):
                    SetNeedsDisplay();
                    break;
            }
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            var x = (float)(rect.X + Element.Padding.Left);
            var y = (float)(rect.Y + Element.Padding.Top);
            var width = (float)(rect.Width - Element.Padding.HorizontalThickness);
            var height = (float)(rect.Height - Element.Padding.VerticalThickness);
            var cx = (float)(width / 2f + Element.Padding.Left);
            var cy = (float)(height / 2f + Element.Padding.Top);

            var context = UIGraphics.GetCurrentContext();
            var strokeColor = base.Element.BorderColor.ToUIColor();
            var fill = false;
            UIColor transparentColor = UIColor.Clear;
            transparentColor.SetFill();
            var stroke = false;
            var strokeWidth = 0f;
            if (Element.BorderColor.A > 0)
            {
                strokeColor.SetStroke();
                stroke = true;
                context.SetLineWidth(1);
                // dashes 10units long, with 4units of separation
                context.SetLineDash(0, new nfloat[] { 5, 2 * (nfloat)Math.PI });
                strokeWidth = 1f;
            }
            var outerRadius = (Math.Min(height, width) - strokeWidth) / 2f;
            var pointyTop = true;
            // draw outer hexagon, with transparent fill
            DrawHexagon(context, cx, cy, outerRadius, pointyTop, fill, stroke);

            var fillColor = base.Element.ShapeColor.ToUIColor();
            if (Element.ShapeColor.A > 0)
            {
                fillColor.SetFill();
                fill = true;
            }
            strokeColor = fillColor;
            strokeWidth = 0;
            context.SetLineWidth(0);
            stroke = false;
            var pcent = Element.Percent;
            if(pcent < .3)
            {
                pcent = .3; //keep enough room for icons to be visible
            }
            if(pcent > 1)
            {
                pcent = 1f;
            }
            var innerRadius = outerRadius * (float)pcent;
            // draw inner hexagon, with fill of "ShapeColor" and no border
            DrawHexagon(context, cx, cy, innerRadius, pointyTop, fill, stroke);
        }

        protected virtual void DrawHexagon(CGContext context, float x, float y, float outerRadius, bool pointyTop, bool fill, bool stroke)
        {
            var points = new List<CGPoint>();
            for (int i = 0; i < 6; ++i)
            {
                points.Add(new CGPoint(x + outerRadius * (float)Math.Cos((i * 60 - 30) * Math.PI / 180f),
                                       y + outerRadius * (float)Math.Sin((i * 60 - 30) * Math.PI / 180f)));
            }
            DrawPoints(context, points, fill, stroke);
        }

        protected virtual void DrawPoints(CGContext context, List<CGPoint> points, bool fill, bool stroke)
        {
            if (points == null || points.Count == 0)
                return;
            var midPoint = new CGPoint(0.5 * (points[0].X + points[1].X), 0.5 * (points[0].Y + points[1].Y));
            var path = new CGPath();
            path.MoveToPoint(midPoint);

            for (var i = 0; i < points.Count; ++i)
            {
                path.AddLineToPoint(points[(i + 1) % points.Count].X, points[(i + 1) % points.Count].Y);
            }
            path.CloseSubpath();
            context.AddPath(path);
            DrawPath(context, fill, stroke);
        }

        private void DrawPath(CGContext context, bool fill, bool stroke)
        {
            if (fill && stroke)
                context.DrawPath(CGPathDrawingMode.FillStroke);
            else if (fill)
                context.DrawPath(CGPathDrawingMode.Fill);
            else if (stroke)
                context.DrawPath(CGPathDrawingMode.Stroke);
        }
    }
}