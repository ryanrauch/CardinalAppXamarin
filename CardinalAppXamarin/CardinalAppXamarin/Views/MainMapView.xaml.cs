using CardinalAppXamarin.ViewModels;
using CardinalAppXamarin.Views.Base;
using Xamarin.Forms.Xaml;

namespace CardinalAppXamarin.Views
{
    public class MainMapViewBase : ViewPageBase<MainMapViewModel> { }

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainMapView : MainMapViewBase
	{
		public MainMapView ()
		{
			InitializeComponent ();
		}
	}
}