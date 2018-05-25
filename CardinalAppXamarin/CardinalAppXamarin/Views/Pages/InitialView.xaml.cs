using CardinalAppXamarin.ViewModels;
using CardinalAppXamarin.Views.Base;
using Xamarin.Forms.Xaml;

namespace CardinalAppXamarin.Views.Pages
{
    public class InitialViewBase : ViewPageBase<InitialViewModel> { }

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InitialView : InitialViewBase
	{
		public InitialView ()
		{
			InitializeComponent ();
		}
    }
}