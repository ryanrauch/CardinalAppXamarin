using CardinalAppXamarin.ViewModels;
using CardinalAppXamarin.Views.Base;
using Xamarin.Forms.Xaml;

namespace CardinalAppXamarin.Views
{
    public class ProfileViewBase : ViewContentBase<ProfileViewModel> { }

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfileView : ProfileViewBase
	{
		public ProfileView ()
		{
			InitializeComponent ();
		}
	}
}