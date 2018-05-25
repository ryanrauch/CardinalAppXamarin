using Autofac;
using CardinalAppXamarin.Views.Pages;

using Xamarin.Forms;

namespace CardinalAppXamarin
{
    public partial class App : Application
	{
        public static IContainer Container { get; set; }

		public App ()
		{
			InitializeComponent();
#if DEBUG
            Container = AutoFacContainerBuilder.CreateContainer(true);
#else
            Container = AutoFacContainerBuilder.CreateContainer(false);
#endif
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
