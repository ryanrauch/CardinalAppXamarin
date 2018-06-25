using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CardinalAppXamarin.Controls
{
    public class HexagonShapeView : ContentView
    {
        public static readonly BindableProperty ShapeColorProperty =
                            BindableProperty.Create(nameof(ShapeColor),
                                                    typeof(Color),
                                                    typeof(HexagonShapeView),
                                                    Color.Transparent);
        public Color ShapeColor
        {
            get { return (Color)GetValue(ShapeColorProperty); }
            set { SetValue(ShapeColorProperty, value); }
        }
    }
}
