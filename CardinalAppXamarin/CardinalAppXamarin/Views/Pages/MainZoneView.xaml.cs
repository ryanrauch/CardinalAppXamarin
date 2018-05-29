using CardinalAppXamarin.ViewModels;
using CardinalAppXamarin.Views.Base;
using Xamarin.Forms.Xaml;

namespace CardinalAppXamarin.Views.Pages
{
    public class MainZoneViewBase : ViewPageBase<MainZoneViewModel> { }

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainZoneView : MainZoneViewBase
	{
		public MainZoneView ()
		{
			InitializeComponent ();
		}
	}
}