using CardinalAppXamarin.ViewModels;
using CardinalAppXamarin.Views.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CardinalAppXamarin.Views
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