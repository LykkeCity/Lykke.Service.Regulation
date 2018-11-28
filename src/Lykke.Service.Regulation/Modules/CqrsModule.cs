using System.Collections.Generic;
using Autofac;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Cqrs;
using Lykke.Cqrs.Configuration;
using Lykke.Messaging;
using Lykke.Messaging.RabbitMq;
using Lykke.Messaging.Serialization;
using Lykke.Service.Registration.Contract;
using Lykke.Service.Registration.Contract.Events;
using Lykke.Service.Regulation.Settings;
using Lykke.Service.Regulation.Workflow.Sagas;
using Lykke.SettingsReader;

namespace Lykke.Service.Regulation.Modules
{
    [UsedImplicitly]
    public class CqrsModule : Module
    {
        private readonly IReloadingManager<AppSettings> _settings;

        public CqrsModule(IReloadingManager<AppSettings> settings)
        {
            _settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            string eventsRoute = "events";
            
            MessagePackSerializerFactory.Defaults.FormatterResolver = MessagePack.Resolvers.ContractlessStandardResolver.Instance;
            var rabbitMqSagasSettings = new RabbitMQ.Client.ConnectionFactory { Uri = _settings.CurrentValue.SagasRabbitMq.RabbitConnectionString };

            builder.Register(context => new AutofacDependencyResolver(context)).As<IDependencyResolver>();
            
            builder.RegisterType<RegistrationSaga>().SingleInstance();

            builder.Register(ctx =>
                {
                    var logFactory = ctx.Resolve<ILogFactory>();

                    var messagingEngine = new MessagingEngine(logFactory,
                        new TransportResolver(new Dictionary<string, TransportInfo>
                        {
                            {
                                "SagasRabbitMq",
                                new TransportInfo(rabbitMqSagasSettings.Endpoint.ToString(),
                                    rabbitMqSagasSettings.UserName, rabbitMqSagasSettings.Password, "None", "RabbitMq")
                            }
                        }),
                        new RabbitMqTransportFactory(logFactory));

                    var sagasEndpointResolver = new RabbitMqConventionEndpointResolver(
                        "SagasRabbitMq",
                        SerializationFormat.MessagePack,
                        environment: "lykke",
                        exclusiveQueuePostfix: "k8s");

                    return new CqrsEngine(logFactory,
                        new AutofacDependencyResolver(ctx.Resolve<IComponentContext>()),
                        messagingEngine,
                        new DefaultEndpointProvider(),
                        true,
                        false, //don't log input messages
                        Register.DefaultEndpointResolver(sagasEndpointResolver),

                        Register.Saga<RegistrationSaga>("regulation-registration-saga")
                            .ListeningEvents(typeof(ClientRegisteredEvent))
                            .From(RegistrationBoundedContext.Name).On(eventsRoute)
                            .WithEndpointResolver(sagasEndpointResolver)
                            .ProcessingOptions(eventsRoute).MultiThreaded(2).QueueCapacity(256)
                    );
                })
                .As<ICqrsEngine>()
                .SingleInstance();
        }
    }
}
