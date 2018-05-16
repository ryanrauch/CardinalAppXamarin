using Autofac;
using CardinalAppXamarin.Services;
using CardinalAppXamarin.Services.Interfaces;
using CardinalAppXamarin.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
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

            containerBuilder.RegisterType<HexagonalEquilateralScale>().As<IHexagonal>();

            containerBuilder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
            containerBuilder.RegisterType<XamarinAuthLocalCredentialService>().As<ILocalCredentialService>().SingleInstance();
            //containerBuilder.RegisterInstance(new XamarinAuthLocalCredentialService()).AsImplementedInterfaces().SingleInstance();
            containerBuilder.RegisterType<JwtRequestService>().As<IRequestService>().SingleInstance();
            //containerBuilder.RegisterInstance(new JwtRequestService()).AsImplementedInterfaces().SingleInstance();
            containerBuilder.RegisterType<HeatGradientService>().As<IHeatGradientService>().SingleInstance();
            containerBuilder.RegisterType<ValidateVersionService>().As<IValidateVersionService>().SingleInstance();
            containerBuilder.RegisterType<PermissionService>().As<IPermissionService>().SingleInstance();
            containerBuilder.RegisterType<CrossGeolocatorService>().As<IGeolocatorService>().SingleInstance();
            containerBuilder.RegisterType<LayerService>().As<ILayerService>().SingleInstance();

            //IAppVersionService appVersionService = DependencyService.Get<IAppVersionService>();
            containerBuilder.RegisterInstance(DependencyService.Get<IAppVersionService>()).AsImplementedInterfaces().SingleInstance();

            return containerBuilder.Build();
        }
    }
}
