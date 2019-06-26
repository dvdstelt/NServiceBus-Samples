using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

namespace LoggingCorrelationId
{
    public class MyMessageHandler : IHandleMessages<MyMessage>
    {
        private readonly ILog log = LogManager.GetLogger<MyMessageHandler>();

        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            log.Info("Received message");

            return Task.CompletedTask;
        }
    }
}
