using CardinalAppXamarin.iOS.DependencyService;
using CardinalAppXamarin.Services.Interfaces;
using Foundation;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppVersionProvider))]
namespace CardinalAppXamarin.iOS.DependencyService
{
    public class AppVersionProvider : IAppVersionService
    {
        public String Version => NSBundle.MainBundle.InfoDictionary[new NSString("CFBundleVersion")].ToString();
    }
}