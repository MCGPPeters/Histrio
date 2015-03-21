namespace Histrio
{
    public static class Send
    {
        public static Message<T> Message<T>(T message)
        {
            return message.AsMessage();
        }

        public static Message<T> Message<T>(Message<T> message)
        {
            return message;
        }
    }
}