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
                Controls.HexagonButtonView bv = new Controls.HexagonButtonView()
                {
                    Radius=50.0d,
                    PointyTop = true,
                    Text = "finally",
                    FontSize = 12,
                    BackgroundColor = Color.FromRgba(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256), 128),
                    TextColor = Color.FromRgba(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256), 256)
                };
                MainHexagonLayout.Children.Add(bv);
            }
        }
    }
}