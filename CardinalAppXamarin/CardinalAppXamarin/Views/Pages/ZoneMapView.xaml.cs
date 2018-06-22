using CardinalAppXamarin.ViewModels;
using CardinalAppXamarin.Views.Base;
using Xamarin.Forms.Xaml;

namespace CardinalAppXamarin.Views.Pages
{
    public class ZoneMapViewBase : ViewPageBase<ZoneMapViewModel> { }

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ZoneMapView : ZoneMapViewBase
	{
		public ZoneMapView ()
		{
			InitializeComponent ();
		}
	}
}