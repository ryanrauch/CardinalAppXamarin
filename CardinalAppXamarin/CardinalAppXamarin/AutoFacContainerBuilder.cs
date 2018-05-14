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
            containerBuilder.RegisterType<MainMapViewModel>().SingleInstance();

            containerBuilder.RegisterType<HexagonalEquilateralScale>().As<IHexagonal>();
            containerBuilder.RegisterType<JwtRequestService>().As<IRequestService>().SingleInstance();
            containerBuilder.RegisterType<XamarinAuthLocalCredentialService>().As<ILocalCredentialService>();
            
            //IAppVersionService appVersionService = DependencyService.Get<IAppVersionService>();
            containerBuilder.RegisterInstance(DependencyService.Get<IAppVersionService>()).AsImplementedInterfaces().SingleInstance();

            return containerBuilder.Build();
        }
    }
}
