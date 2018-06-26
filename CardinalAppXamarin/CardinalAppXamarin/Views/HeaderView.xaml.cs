using CardinalAppXamarin.ViewModels;
using CardinalAppXamarin.Views.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CardinalAppXamarin.Views
{
    //public class HeaderViewBase : ViewContentBase<HeaderViewModel> { }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    //public partial class HeaderView : HeaderViewBase
    public partial class HeaderView : ContentView
    {
        public HeaderView ()
		{
			InitializeComponent ();
		}

        public static readonly BindableProperty TitleTextProperty =
                                    BindableProperty.Create(nameof(TitleText),
                                                            typeof(string),
                                                            typeof(HeaderView),
                                                            "twf",
                                                            BindingMode.TwoWay,
                                                            propertyChanged: TitleTextPropertyChanged);

        public string TitleText
        {
            get { return (string)GetValue(TitleTextProperty); }
            set { SetValue(TitleTextProperty, value); }
        }

        private static void TitleTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (HeaderView)bindable;
            control.TitleText = newValue.ToString();
        }
    }
}