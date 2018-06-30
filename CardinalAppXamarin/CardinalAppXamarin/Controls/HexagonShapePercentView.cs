using Xamarin.Forms;

namespace CardinalAppXamarin.Controls
{
    public class HexagonShapePercentView : ContentView
    {
        public static readonly BindableProperty ShapeColorProperty =
                            BindableProperty.Create(nameof(ShapeColor),
                                                    typeof(Color),
                                                    typeof(HexagonShapePercentView),
                                                    Color.Transparent);

        public static readonly BindableProperty BorderColorProperty =
                    BindableProperty.Create(nameof(BorderColor),
                                            typeof(Color),
                                            typeof(HexagonShapePercentView),
                                            Color.Transparent);

        public static readonly BindableProperty PercentProperty =
            BindableProperty.Create(nameof(Percent),
                                    typeof(float),
                                    typeof(HexagonShapePercentView),
                                    1.0);

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
        public float Percent
        {
            get { return (float)GetValue(PercentProperty); }
            set { SetValue(PercentProperty, value); }
        }
    }
}
