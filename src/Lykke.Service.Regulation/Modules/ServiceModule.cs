using Autofac;
using AzureStorage.Tables;
using Common;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Sdk;
using Lykke.Service.IpGeoLocation;
using Lykke.Service.Regulation.AzureRepositories;
using Lykke.Service.Regulation.Core.Repositories;
using Lykke.Service.Regulation.Core.Services;
using Lykke.Service.Regulation.RabbitPublishers;
using Lykke.Service.Regulation.RabbitSubscribers;
using Lykke.Service.Regulation.Services;
using Lykke.Service.Regulation.Settings;
using Lykke.SettingsReader;

namespace Lykke.Service.Regulation.Modules
{
    [UsedImplicitly]
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<AppSettings> _settings;

        public ServiceModule(IReloadingManager<AppSettings> settings)
        {
            _settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();

            builder.Register(c => 
                new IpGeoLocationClient(_settings.CurrentValue.IpGeoLocationServiceClient.ServiceUrl, 
                    c.Resolve<ILogFactory>().CreateLog(nameof(IpGeoLocationClient))))
                .As<IIpGeoLocationClient>()
                .SingleInstance();

            RegisterRepositories(builder);
            RegisterServices(builder);
            RegisterRabbitMqSubscribers(builder);
            RegisterRabbitMqPublishers(builder);
        }

        private void RegisterRepositories(ContainerBuilder builder)
        {
            const string regulationsTableName = "Regulations";
            const string clientRegulationsTableName = "ClientRegulations";
            builder.Register(c => new RegulationRepository(
                    AzureTableStorage<RegulationEntity>.Create(_settings.ConnectionString(x => x.RegulationService.Db.DataConnString),
                        regulationsTableName, c.Resolve<ILogFactory>())))
                .As<IRegulationRepository>();

            builder.Register(c => new ClientRegulationRepository(
                    AzureTableStorage<ClientRegulationEntity>.Create(_settings.ConnectionString(x => x.RegulationService.Db.DataConnString),
                        clientRegulationsTableName, c.Resolve<ILogFactory>())))
                .As<IClientRegulationRepository>();

            builder.Register(c => new WelcomeRegulationRuleRepository(
                    AzureTableStorage<WelcomeRegulationRuleEntity>.Create(_settings.ConnectionString(x => x.RegulationService.Db.DataConnString),
                        regulationsTableName, c.Resolve<ILogFactory>())))
                .As<IWelcomeRegulationRuleRepository>();
        }

        private void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<ClientRegulationService>()
                .As<IClientRegulationService>();

            builder.RegisterType<WelcomeRegulationRuleService>()
                .As<IWelcomeRegulationRuleService>();

            builder.RegisterType<RegulationService>()
                .As<IRegulationService>();
        }

        private void RegisterRabbitMqSubscribers(ContainerBuilder builder)
        {
            builder.RegisterType<ClientRegisteredSubscriber>()
                .AsSelf()
                .As<IStartable>()
                .As<IStopable>()
                .AutoActivate()
                .SingleInstance()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.RegulationService.RabbitMq.RegistrationQueue));
        }

        private void RegisterRabbitMqPublishers(ContainerBuilder builder)
        {
            builder.RegisterType<ClientRegulationPublisher>()
                .As<IClientRegulationPublisher>()
                .As<IStartable>()
                .As<IStopable>()
                .AutoActivate()
                .SingleInstance()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.RegulationService.RabbitMq.RegulationExchange));
        }
    }
}
