using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using log4net;
using NServiceBus;
using NServiceBus.Pipeline;

namespace LoggingCorrelationId
{
public class AddCorrIdToLog4NetBehavior : Behavior<IIncomingPhysicalMessageContext>
{
    public override Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
    {
        var headers = context.Message.Headers;
        var businessCorrelationId = headers["BusinessCorrelationId"];

        LogicalThreadContext.Properties["BusinessCorrelationId"] = businessCorrelationId;

        return next();
    }
}
}
