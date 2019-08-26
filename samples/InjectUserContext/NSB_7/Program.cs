using System;
using System.Threading.Tasks;
using NServiceBus;

namespace InjectUserContext
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var endpointConfiguration = new EndpointConfiguration("Endpoint");
            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            transport.Routing().RouteToEndpoint(typeof(MyMessage), "Endpoint");

            var pipeline = endpointConfiguration.Pipeline;
            pipeline.Register<CreateUserContextBehavior.Registration>();

            endpointConfiguration.RegisterComponents(registration: x =>
            {
                x.ConfigureComponent<UserRepository>(DependencyLifecycle.InstancePerUnitOfWork);
                x.ConfigureComponent<UserContext>(DependencyLifecycle.InstancePerUnitOfWork);
                x.ConfigureComponent<MyDependency>(DependencyLifecycle.InstancePerUnitOfWork);
            });

            var endpoint = await Endpoint.Start(endpointConfiguration);

            var sendOptions = new SendOptions();
            sendOptions.SetHeader("UserIdentifier", "42");

            var message = new MyMessage();
            await endpoint.Send(message, sendOptions);

            Console.WriteLine("Press a key to quit.");
            Console.ReadKey();
        }
    }
}
