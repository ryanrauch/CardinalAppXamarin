using Autofac;
using CardinalAppXamarin.Services;
using CardinalAppXamarin.Services.Interfaces;
using CardinalAppXamarin.Services.Mock;
using CardinalAppXamarin.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CardinalAppXamarin
{
    public static class AutoFacContainerBuilder
    {
        public static IContainer CreateContainer(bool mock)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<InitialViewModel>().SingleInstance();
            containerBuilder.RegisterType<LoginViewModel>().SingleInstance();
            containerBuilder.RegisterType<MainMapViewModel>().SingleInstance();
            containerBuilder.RegisterType<RegisterViewModel>().SingleInstance();

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
            containerBuilder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
            containerBuilder.RegisterType<JwtRequestService>().As<IRequestService>().SingleInstance();
            containerBuilder.RegisterType<HeatGradientService>().As<IHeatGradientService>().SingleInstance();
            containerBuilder.RegisterType<ValidateVersionService>().As<IValidateVersionService>().SingleInstance();
            containerBuilder.RegisterType<LayerService>().As<ILayerService>().SingleInstance();
            containerBuilder.RegisterType<DialogService>().As<IDialogService>().SingleInstance();

            containerBuilder.RegisterInstance(DependencyService.Get<IAppVersionService>()).AsImplementedInterfaces().SingleInstance();

            return containerBuilder.Build();
        }
    }
}
