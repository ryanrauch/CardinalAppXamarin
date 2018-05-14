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