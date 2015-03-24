namespace Histrio
{
    public class Dispatchable<T>
    {
        public Dispatchable(Message<T> message)
        {
            Message = message;
        }

        public Message<T> Message { get; set; }
    }
}