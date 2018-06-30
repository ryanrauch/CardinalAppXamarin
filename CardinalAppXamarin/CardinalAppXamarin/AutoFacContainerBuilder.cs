using Autofac;
using CardinalAppXamarin.Services;
using CardinalAppXamarin.Services.Interfaces;
using CardinalAppXamarin.Services.Mock;
using CardinalAppXamarin.ViewModels;
using Xamarin.Forms;

namespace CardinalAppXamarin
{
    public static class AutoFacContainerBuilder
    {
        public static IContainer CreateContainer()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<InitialViewModel>().SingleInstance();
            containerBuilder.RegisterType<LoginViewModel>().SingleInstance();
            containerBuilder.RegisterType<MainMapViewModel>().SingleInstance();
            containerBuilder.RegisterType<RegisterViewModel>().SingleInstance();
            containerBuilder.RegisterType<ProfileViewModel>().SingleInstance();
            containerBuilder.RegisterType<MainZoneViewModel>().SingleInstance();
            containerBuilder.RegisterType<SettingsViewModel>().SingleInstance();
            containerBuilder.RegisterType<HeaderViewModel>().SingleInstance();
            containerBuilder.RegisterType<ZoneHexagonViewModel>().SingleInstance();

            containerBuilder.RegisterType<ZoneMapViewModel>();
            containerBuilder.RegisterType<ZoneViewModel>();
            containerBuilder.RegisterType<UserInfoBriefViewCellModel>();

            containerBuilder.RegisterType<FriendsListViewModel>();
            /////////////////
            bool mock = true;
            /////////////////
            if(mock)
            {
                containerBuilder.RegisterType<MockGeolocatorService>().As<IGeolocatorService>().SingleInstance();
                containerBuilder.RegisterType<MockPermissionService>().As<IPermissionService>().SingleInstance();
                containerBuilder.RegisterType<MockDataUpdateService>().As<IMockDataUpdateService>().SingleInstance();
                containerBuilder.RegisterType<MockLocalCredentialService>().As<ILocalCredentialService>().SingleInstance();
            }
            else
            {
                containerBuilder.RegisterType<CrossGeolocatorService>().As<IGeolocatorService>().SingleInstance();
                containerBuilder.RegisterType<PermissionService>().As<IPermissionService>().SingleInstance();
                containerBuilder.RegisterType<IgnoreMockDataUpdateService>().As<IMockDataUpdateService>().SingleInstance();

                //TODO: NuGet: Xamarin.Auth - isn't working for Android v27
                containerBuilder.RegisterType<MockLocalCredentialService>().As<ILocalCredentialService>().SingleInstance();
                //containerBuilder.RegisterType<XamarinAuthLocalCredentialService>().As<ILocalCredentialService>().SingleInstance();
            }
            containerBuilder.RegisterType<HexagonalEquilateralScale>().As<IHexagonal>();
            //containerBuilder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
            containerBuilder.RegisterType<SinglePageNavigationService>().As<INavigationService>().SingleInstance();
            containerBuilder.RegisterType<JwtRequestService>().As<IRequestService>().SingleInstance();
            containerBuilder.RegisterType<HeatGradientService>().As<IHeatGradientService>().SingleInstance();
            containerBuilder.RegisterType<ValidateVersionService>().As<IValidateVersionService>().SingleInstance();
            containerBuilder.RegisterType<LayerService>().As<ILayerService>().SingleInstance();
            containerBuilder.RegisterType<DialogService>().As<IDialogService>().SingleInstance();
            containerBuilder.RegisterType<ZoneService>().As<IZoneService>().SingleInstance();
            
            containerBuilder.RegisterInstance(DependencyService.Get<IAppVersionService>()).AsImplementedInterfaces().SingleInstance();

            containerBuilder.RegisterType<FriendRequestService>().As<IFriendRequestService>().SingleInstance();
            containerBuilder.RegisterType<UserInfoService>().As<IUserInfoService>().SingleInstance();

            return containerBuilder.Build();
        }
    }
}
