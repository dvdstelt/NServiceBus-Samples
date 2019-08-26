using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

namespace InjectUserContext
{
    public class MyMessageHandler : IHandleMessages<MyMessage>
    {
        private readonly MyDependency myDependency;
        private readonly ILog log = LogManager.GetLogger<MyMessageHandler>();

        public MyMessageHandler(MyDependency myDependency)
        {
            this.myDependency = myDependency;
        }

        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            log.Info("Received message");

            myDependency.DeleteSomething(message.SomeValue);

            return Task.CompletedTask;
        }
    }
}
