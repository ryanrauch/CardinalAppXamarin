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

        public static readonly BindableProperty BorderColorProperty =
                    BindableProperty.Create(nameof(BorderColor),
                                            typeof(Color),
                                            typeof(HexagonShapeView),
                                            Color.Transparent);

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text),
                                    typeof(String),
                                    typeof(HexagonShapeView),
                                    String.Empty);
        public Color ShapeColor
        {
            get { return (Color)GetValue(ShapeColorProperty); }
            set { SetValue(ShapeColorProperty, value); }
        }
        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }
        public String Text
        {
            get { return (String)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}
