namespace Histrio
{
    public static class Send
    {
        public static void Message<T>(IMessage<T> message)
        {
            if (message.Address.Uri.Scheme == "actor")
            {
                Context.System.Dispatch(message);
            }
        }
    }
}