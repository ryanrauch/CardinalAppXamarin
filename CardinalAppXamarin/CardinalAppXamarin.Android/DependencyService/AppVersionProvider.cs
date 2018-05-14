using CardinalAppXamarin.Droid.DependencyService;
using CardinalAppXamarin.Services.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppVersionProvider))]
namespace CardinalAppXamarin.Droid.DependencyService
{
    public class AppVersionProvider : IAppVersionService
    {
        public string Version
        {
            get
            {
                var context = Android.App.Application.Context;
                var info = context.PackageManager.GetPackageInfo(context.PackageName, 0);

                return $"{info.VersionName}.{info.VersionCode.ToString()}";
            }
        }
    }
}