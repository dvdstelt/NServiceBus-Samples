using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using log4net;
using NServiceBus;
using NServiceBus.Pipeline;

namespace LoggingCorrelationId
{
    public class StoreCorrelationIdBehavior : Behavior<IIncomingPhysicalMessageContext>
    {
        public override Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
        {
            if (context.Message.Headers.TryGetValue("BusinessCorrelationId", out string businessCorrelationId))
            {
                // Storing it in NServiceBus context bag, so we can propagate outgoing messages with it as well
                context.Extensions.Set("BusinessCorrelationId", businessCorrelationId);
                // Storing it in Log4Net thread context
                LogicalThreadContext.Properties["BusinessCorrelationId"] = businessCorrelationId;
            }

            return next();
        }
    }
}
