using Autofac;
using AzureStorage.Tables;
using Common;
using Common.Log;
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
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<AppSettings> _settings;
        private readonly ILog _log;

        public ServiceModule(IReloadingManager<AppSettings> settings, ILog log)
        {
            _settings = settings;
            _log = log;
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

            builder.RegisterIpGeoLocationClient(_settings.CurrentValue.IpGeoLocationServiceClient.ServiceUrl, _log);

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
                        regulationsTableName, _log)))
                .As<IRegulationRepository>();

            builder.Register(c => new ClientRegulationRepository(
                    AzureTableStorage<ClientRegulationEntity>.Create(_settings.ConnectionString(x => x.RegulationService.Db.DataConnString),
                        clientRegulationsTableName, _log)))
                .As<IClientRegulationRepository>();

            builder.Register(c => new WelcomeRegulationRuleRepository(
                    AzureTableStorage<WelcomeRegulationRuleEntity>.Create(_settings.ConnectionString(x => x.RegulationService.Db.DataConnString),
                        regulationsTableName, _log)))
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
