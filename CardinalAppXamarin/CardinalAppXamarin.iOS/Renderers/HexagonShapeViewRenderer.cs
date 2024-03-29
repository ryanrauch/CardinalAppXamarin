﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CardinalAppXamarin.Controls;
using CardinalAppXamarin.iOS.Renderers;
using CoreGraphics;
using CoreText;
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

            if (Element == null)
                return;

            switch (e.PropertyName)
            {
                case nameof(Element.IsVisible):
                case nameof(Element.ShapeColor):
                case nameof(Element.BorderColor):
                //case nameof(Element.Text):
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
            //var innerRadius = outerRadius * Element.Percent;
            var pointyTop = true;
            DrawHexagon(context, cx, cy, outerRadius, pointyTop, fill, stroke);
            //if (!String.IsNullOrEmpty(Element.Text))
            //{
            //    var txt = Element.Text;
            //    DrawText(context, txt, cx, cy);
            //    //DrawCenteredText(context, txt, cx, cy);
            //}
        }

        protected virtual void DrawCenteredText(CGContext context, String text, float cx, float cy)
        {
            CGPoint cgp = new CGPoint(cx, cy);
            UIFont uif = UIFont.FromName("Arial", 24);
            NSParagraphStyle nsps = new NSParagraphStyle()
            {
                Alignment = UITextAlignment.Center
            };
            //NSAttributedString nsas = new NSAttributedString(str: text,
            //                                                 font: uif,
            //                                                 foregroundColor: UIColor.Red,
            //                                                 paragraphStyle: nsps);
            NSAttributedString nsas = new NSAttributedString(str: text,
                                                             font: uif,
                                                             foregroundColor: UIColor.Red);
            nsas.DrawString(cgp);
        }

        protected virtual void DrawText(CGContext context, String text, float cx, float cy)
        {
            float fontSize = 15f;
            context.ScaleCTM(1, -1);
            context.TranslateCTM(0, -Bounds.Height);
            context.SelectFont("Helvetica", fontSize, CGTextEncoding.MacRoman);

            //context.SetTextDrawingMode(CGTextDrawingMode.Invisible);
            //context.ShowTextAtPoint(cx, cy, text);
            

            context.SetFillColor(UIColor.Green.CGColor);
            //context.SetStrokeColor(UIColor.Green.CGColor);
            context.SetTextDrawingMode(CGTextDrawingMode.Fill);
            context.ShowTextAtPoint(cx,cy, text);
            var textWidth = (float)context.TextPosition.X - cx;
            var textHeight = (float)context.TextPosition.Y - cy;
            context.TranslateCTM(0 - textWidth / 2, 
                                 0 - textHeight / 2);
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