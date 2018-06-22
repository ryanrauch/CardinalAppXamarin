using CardinalAppXamarin.ViewModels;
using CardinalAppXamarin.Views.Base;
using Xamarin.Forms.Xaml;

namespace CardinalAppXamarin.Views.Pages
{
    public class ZoneHexagonViewBase : ViewPageBase<ZoneHexagonViewModel> { }

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ZoneHexagonView : ZoneHexagonViewBase
	{
		public ZoneHexagonView ()
		{
			InitializeComponent ();
		}
	}
}