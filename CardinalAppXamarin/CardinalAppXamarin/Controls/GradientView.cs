using Xamarin.Forms;

namespace CardinalAppXamarin.Controls
{
    public class GradientView : View
    {
        public static readonly BindableProperty StartColorProperty = 
                                    BindableProperty.Create(nameof(StartColor), 
                                                            typeof(Color), 
                                                            typeof(GradientView), 
                                                            Color.Transparent);
        public static readonly BindableProperty MiddleColorProperty =
                                   BindableProperty.Create(nameof(MiddleColor),
                                                           typeof(Color),
                                                           typeof(GradientView),
                                                           Color.Transparent);
        public static readonly BindableProperty EndColorProperty =
                                    BindableProperty.Create(nameof(EndColor),
                                                            typeof(Color),
                                                            typeof(GradientView),
                                                            Color.Transparent);
        public static readonly BindableProperty PercentProperty =
                                    BindableProperty.Create(nameof(Percent),
                                                            typeof(double),
                                                            typeof(GradientView),
                                                            0.0);
        public Color StartColor
        {
            get { return (Color)GetValue(StartColorProperty); }
            set { SetValue(StartColorProperty, value); }
        }
        public Color MiddleColor
        {
            get { return (Color)GetValue(MiddleColorProperty); }
            set { SetValue(MiddleColorProperty, value); }
        }
        public Color EndColor
        {
            get { return (Color)GetValue(EndColorProperty); }
            set { SetValue(EndColorProperty, value); }
        }
        public double Percent
        {
            get { return (double)GetValue(PercentProperty); }
            set { SetValue(PercentProperty, value); }
        }
    }
}
