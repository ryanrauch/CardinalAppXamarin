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
        //see this page for original renderer
        //https:----//github.com/vincentgury/XFShapeView/blob/master/src/XFShapeView.iOS/ShapeRenderer.cs

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            var x = (float)(rect.X + Element.Padding.Left);
            var y = (float)(rect.Y + Element.Padding.Top);
            var width = (float)(rect.Width - Element.Padding.HorizontalThickness);
            var height = (float)(rect.Height -  Element.Padding.VerticalThickness);
            var cx = (float)(width / 2f +  Element.Padding.Left);
            var cy = (float)(height / 2f +  Element.Padding.Top);

            var context = UIGraphics.GetCurrentContext();
            var fillColor = base.Element.ShapeColor.ToUIColor();
            var strokeColor = base.Element.BorderColor.ToUIColor();
            var fill = false;
            if(Element.ShapeColor.A > 0)
            {
                fillColor.SetFill();
                fill = true;
            }
            var stroke = false;
            var strokeWidth = 0f;
            if (Element.BorderColor.A > 0)
            {
                strokeColor.SetStroke();
                stroke = true;
                context.SetLineWidth(1);
                strokeWidth = 1f;
            }

            var outerRadius = (Math.Min(height, width) - strokeWidth) / 2f;
            var innerRadius = outerRadius * 0.5f;

            var pointyTop = true;
            DrawHexagon(context, cx, cy, outerRadius, pointyTop, fill, stroke);

            var txt = Element.Text;
            //DrawText(context, txt, cx, cy);
            DrawCenteredTextAtPoint(context, cx, cy, txt, 25);
        }

        protected virtual void DrawText(CGContext context, String text, float cx, float cy)
        {
            
            float fontSize = 15f;
            context.ScaleCTM(1, -1);
            context.TranslateCTM(0, -Bounds.Height);
            //context.TranslateCTM(0, cy - fontSize / 2);
            //context.TranslateCTM(0, fontSize);
            //context.TranslateCTM(x, 0);
            //measure size of text first
            context.SetTextDrawingMode(CGTextDrawingMode.Invisible);
            context.ShowText(text);
            var textWidth = (float)context.TextPosition.X - cx;
            //actually draw text
            context.SetFillColor(UIColor.Red.CGColor);
            //context.SetStrokeColor(UIColor.Green.CGColor);
            context.SetTextDrawingMode(CGTextDrawingMode.Fill);
            context.SelectFont("Helvetica", fontSize, CGTextEncoding.MacRoman);
            //context.ShowText(text);
            context.ShowTextAtPoint(cx - textWidth / 2, cy, text);
        }

        protected void DrawCenteredTextAtPoint(CGContext context, float cx, float cy, string text, int textHeight)
        {
            context.SelectFont("Helvetica-Bold", textHeight, CGTextEncoding.MacRoman);
            context.SetTextDrawingMode(CGTextDrawingMode.Invisible);
            context.ShowTextAtPoint(cx, cy, text, text.Length);
            context.SetTextDrawingMode(CGTextDrawingMode.Fill);
            context.SetFillColor(UIColor.Green.CGColor);
            context.ShowTextAtPoint(cx - (context.TextPosition.X - cx) / 2, cy, text, text.Length);
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