using CardinalAppXamarin.ViewModels;
using CardinalAppXamarin.Views.Base;
using Xamarin.Forms.Xaml;

namespace CardinalAppXamarin.Views
{
    public class RegisterViewBase : ViewPageBase<RegisterViewModel> { }

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegisterView : RegisterViewBase
	{
		public RegisterView ()
		{
			InitializeComponent ();
		}
	}
}