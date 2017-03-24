namespace Histrio.Commands
{
    public static class Send
    {
        public static Message<T> Message<T>(T message)
        {
            return message.AsMessage();
        }

        public static Message<T> Message<T>(Message<T> messageBase)
        {
            return messageBase;
        }
    }
}