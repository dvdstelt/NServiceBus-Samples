using NServiceBus;

namespace InjectUserContext
{
    public class MyMessage : IMessage
    {
        public string SomeValue { get; set; }
    }
}
