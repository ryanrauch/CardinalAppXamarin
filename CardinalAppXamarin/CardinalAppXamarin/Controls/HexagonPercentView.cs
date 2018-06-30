using Xamarin.Forms;

namespace CardinalAppXamarin.Controls
{
    public class HexagonPercentView : ContentView
    {
        public static readonly BindableProperty ShapeColorProperty =
                            BindableProperty.Create(nameof(ShapeColor),
                                                    typeof(Color),
                                                    typeof(HexagonPercentView),
                                                    Color.Transparent);

        public static readonly BindableProperty BorderColorProperty =
                    BindableProperty.Create(nameof(BorderColor),
                                            typeof(Color),
                                            typeof(HexagonPercentView),
                                            Color.Transparent);

        public static readonly BindableProperty PercentProperty =
            BindableProperty.Create(nameof(Percent),
                                    typeof(double),
                                    typeof(HexagonPercentView),
                                    1.0d);

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
