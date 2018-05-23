using CardinalAppXamarin.ViewModels;
using CardinalAppXamarin.Views.Base;
using Xamarin.Forms.Xaml;

namespace CardinalAppXamarin.Views
{
    public class SettingsViewBase : ViewPageBase<SettingsViewModel> { }

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsView : SettingsViewBase
	{
		public SettingsView ()
		{
			InitializeComponent ();
		}
	}
}