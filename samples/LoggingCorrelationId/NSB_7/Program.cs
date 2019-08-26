using System;
using System.Reflection;
using System.Threading.Tasks;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using NServiceBus;
using NServiceBus.Pipeline;
using LogManager = NServiceBus.Logging.LogManager;

namespace LoggingCorrelationId
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var endpointConfiguration = new EndpointConfiguration("Endpoint");
            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            transport.Routing().RouteToEndpoint(typeof(MyMessage), "Endpoint");

            ConfigureLog4Net();

            var pipeline = endpointConfiguration.Pipeline;
            pipeline.Register(new StoreCorrelationIdBehavior(), "Stores correlation identifier into session and thread context.");
            pipeline.Register(new PropagateCorrelationIdBehavior(), "Propagates correlation identifier to outgoing messages");

            var endpoint = await Endpoint.Start(endpointConfiguration);

            var sendOptions = new SendOptions();
            sendOptions.SetHeader("BusinessCorrelationId", Guid.NewGuid().ToString().Substring(0, 8));

            var message = new MyMessage();
            await endpoint.Send(message, sendOptions);

            Console.WriteLine("Press a key to quit.");
            Console.ReadKey();
        }

        private static void ConfigureLog4Net()
        {
            var layout = new PatternLayout
            {
                ConversionPattern = "%d [%property{BusinessCorrelationId}] %-5p %c - %m%n"
            };
            layout.ActivateOptions();

            var consoleAppender = new ConsoleAppender
            {
                Threshold = Level.Info,
                Layout = layout
            };
            consoleAppender.ActivateOptions();

            var executingAssembly = Assembly.GetExecutingAssembly();
            var repository = log4net.LogManager.GetRepository(executingAssembly);
            BasicConfigurator.Configure(repository, consoleAppender);

            // Tell NServiceBus to use Log4Net
            LogManager.Use<Log4NetFactory>();
        }
    }
}
