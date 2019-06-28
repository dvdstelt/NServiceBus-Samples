using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

namespace LoggingCorrelationId
{
    public class PropagateCorrelationIdBehavior : Behavior<IOutgoingLogicalMessageContext>
    {
        public override Task Invoke(IOutgoingLogicalMessageContext context, Func<Task> next)
        {
            if (context.Extensions.TryGet("BusinessCorrelationId", out string businessCorrelationId))
            {
                context.Headers["BusinessCorrelationId"] = businessCorrelationId;
            }
            return next();
        }
    }
}
