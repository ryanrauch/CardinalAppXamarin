﻿using CardinalAppXamarin.ViewModels;
using CardinalAppXamarin.Views.Base;
using Xamarin.Forms.Xaml;

namespace CardinalAppXamarin.Views
{
    public class LoginViewBase : ViewPageBase<LoginViewModel> { }

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginView : LoginViewBase
	{
		public LoginView ()
		{
			InitializeComponent ();
		}
    }
}