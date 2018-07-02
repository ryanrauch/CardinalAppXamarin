using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CardinalAppXamarin.Controls
{
    public class HexagonButtonView : View
    {
        public static readonly BindableProperty RadiusProperty = BindableProperty.Create(
                nameof(Radius),
                typeof(double),
                typeof(HexagonButtonView),
                10.0);

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public static readonly BindableProperty PointyTopProperty = BindableProperty.Create(
                nameof(PointyTop),
                typeof(bool),
                typeof(HexagonButtonView),
                true);

        public bool PointyTop
        {
            get { return (bool)GetValue(PointyTopProperty); }
            set { SetValue(PointyTopProperty, value); }
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
                nameof(Text),
                typeof(string),
                typeof(HexagonButtonView),
                string.Empty);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}
