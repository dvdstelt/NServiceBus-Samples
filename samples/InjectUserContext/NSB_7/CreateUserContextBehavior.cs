using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

namespace InjectUserContext
{
    public class CreateUserContextBehavior : Behavior<IIncomingPhysicalMessageContext>
    {
        private readonly UserRepository userRepository;

        public CreateUserContextBehavior(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public override async Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
        {
            var builder = context.Builder;
            var userContext = builder.Build<UserContext>();

            try
            {
                if (context.Message.Headers.TryGetValue("UserIdentifier", out string userId))
                {
                    var canDeleteRows = userRepository.CanDeleteRows(userId);

                    userContext.UserId = userId;
                    userContext.CanDeleteRows = canDeleteRows;

                    // Storing it in NServiceBus context bag as well, so we can use it to propagate outgoing messages with the UserId as well
                    context.Extensions.Set("UserContext", userContext);
                }

                await next().ConfigureAwait(false);
            }
            finally
            {
                builder.Release(userContext);
            }
        }

        public class Registration : RegisterStep
        {
            public Registration() : 
                base(typeof(CreateUserContextBehavior).Name, typeof(CreateUserContextBehavior), "UserContext")
            {
            }
        }
    }
}
