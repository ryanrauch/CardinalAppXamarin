using CardinalAppXamarin.ViewModels;
using CardinalAppXamarin.Views.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CardinalAppXamarin.Views.Pages
{
    public class FriendsListViewBase : ViewPageBase<FriendsListViewModel> { }

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FriendsListView : FriendsListViewBase
	{
		public FriendsListView ()
		{
			InitializeComponent ();
		}
	}
}