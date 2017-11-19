using System;
using Autofac;

namespace Lykke.Service.Regulation.Client
{
    public static class AutofacExtension
    {
        public static void RegisterRegulationClient(this ContainerBuilder builder, string serviceUrl)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (serviceUrl == null) throw new ArgumentNullException(nameof(serviceUrl));
            if (string.IsNullOrWhiteSpace(serviceUrl))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceUrl));

            builder.RegisterInstance(new RegulationClient(serviceUrl)).As<IRegulationClient>().SingleInstance();
        }
    }
}
