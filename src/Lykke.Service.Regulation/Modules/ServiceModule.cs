using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AzureStorage.Tables;
using Common.Log;
using Lykke.Service.Regulation.AzureRepositories;
using Lykke.Service.Regulation.Core.Domain;
using Lykke.Service.Regulation.Core.Repositories;
using Lykke.Service.Regulation.Core.Services;
using Lykke.Service.Regulation.Core.Settings.ServiceSettings;
using Lykke.Service.Regulation.Services;
using Lykke.SettingsReader;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Service.Regulation.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<RegulationSettings> _settings;
        private readonly ILog _log;
        // NOTE: you can remove it if you don't need to use IServiceCollection extensions to register service specific dependencies
        private readonly IServiceCollection _services;

        public ServiceModule(IReloadingManager<RegulationSettings> settings, ILog log)
        {
            _settings = settings;
            _log = log;

            _services = new ServiceCollection();
        }

        protected override void Load(ContainerBuilder builder)
        {
            // TODO: Do not register entire settings in container, pass necessary settings to services which requires them
            // ex:
            //  builder.RegisterType<QuotesPublisher>()
            //      .As<IQuotesPublisher>()
            //      .WithParameter(TypedParameter.From(_settings.CurrentValue.QuotesPublication))

            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();

            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();

            RegisterRepositories(builder);
            RegisterServices(builder);

            builder.Populate(_services);
        }

        private void RegisterRepositories(ContainerBuilder builder)
        {
            const string tableName = "Regulations";

            builder.Register(c => new RegulationRepository(
                    AzureTableStorage<RegulationEntity>.Create(_settings.ConnectionString(x => x.Db.DataConnString),
                        tableName, _log)))
                .As<IRegulationRepository>();

            builder.Register(c => new ClientRegulationRepository(
                    AzureTableStorage<ClientRegulationEntity>.Create(_settings.ConnectionString(x => x.Db.DataConnString),
                        tableName, _log)))
                .As<IClientRegulationRepository>();

            builder.Register(c => new ClientAvailableRegulationRepository(
                    AzureTableStorage<ClientAvailableRegulationEntity>.Create(_settings.ConnectionString(x => x.Db.DataConnString),
                        tableName, _log)))
                .As<IClientAvailableRegulationRepository>();
        }

        private void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<ClientRegulationService>()
                .As<IClientRegulationService>();

            builder.RegisterType<RegulationService>()
                .As<IRegulationService>();
        }
    }
}
