using CardinalAppXamarin.ViewModels;
using CardinalAppXamarin.Views.Base;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CardinalAppXamarin.Views.Pages
{
    public class TestMainViewBase : ViewPageBase<TestMainViewModel> { }

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TestMainView : TestMainViewBase
	{
		public TestMainView ()
		{
			InitializeComponent ();        
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Random r = new Random();
            for (int i = 0; i < 15; ++i)
            {
                BoxView bv = new BoxView()
                {
                    BackgroundColor = Color.FromRgba(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256), 128),
                    IsVisible = true
                };
                MainHexagonLayout.Children.Add(bv);

                Controls.HexagonButtonView bv2 = new Controls.HexagonButtonView()
                {
                    Radius=25,
                    PointyTop = true,
                    Text="text",
                    BackgroundColor = Color.FromRgba(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256), 128),
                };
                MainHexagonLayout2.Children.Add(bv2);
            }
        }
    }
}