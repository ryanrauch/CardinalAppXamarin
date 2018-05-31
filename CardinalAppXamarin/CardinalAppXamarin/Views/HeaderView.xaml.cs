using CardinalAppXamarin.ViewModels;
using CardinalAppXamarin.Views.Base;
using Xamarin.Forms.Xaml;

namespace CardinalAppXamarin.Views
{
    public class HeaderViewBase : ViewContentBase<HeaderViewModel> { }

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HeaderView : HeaderViewBase
	{
		public HeaderView ()
		{
			InitializeComponent ();
		}
	}
}