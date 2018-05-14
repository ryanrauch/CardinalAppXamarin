using Autofac;
using CardinalAppXamarin.Services.Interfaces;
using CardinalAppXamarin.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace CardinalAppXamarin
{
	public partial class App : Application
	{
        public static IContainer Container { get; set; }

		public App ()
		{
			InitializeComponent();
            Container = AutoFacContainerBuilder.CreateContainer();
            MainPage = new NavigationPage(new InitialView());
            //MainPage = new NavigationPage(new MainMapView());
        }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
