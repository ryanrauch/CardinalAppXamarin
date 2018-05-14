using Autofac;
using CardinalAppXamarin.Services;
using CardinalAppXamarin.Services.Interfaces;
using CardinalAppXamarin.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardinalAppXamarin
{
    public static class AutoFacContainerBuilder
    {
        public static IContainer CreateContainer()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<MainMapViewModel>().SingleInstance();

            containerBuilder.RegisterType<HexagonalEquilateralScale>().As<IHexagonal>();

            return containerBuilder.Build();
        }
    }
}
