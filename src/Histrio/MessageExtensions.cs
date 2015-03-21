namespace Histrio
{
    public static class MessageExtensions
    {
        public static Message<T> AsMessage<T>(this T body)
        {
            return new Message<T>(body);
        }
    }
}