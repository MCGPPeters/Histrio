﻿using Histrio.Commands;

namespace Histrio.Behaviors.StorageCell
{
    public class StorageCellBehavior<T> : BehaviorBase, IHandle<Get>, IHandle<Set<T>>
    {
        private T _contents;

        public void Accept(Get message)
        {
            var reply = new Reply<T>(_contents);
            var replyMessage = reply.AsMessage();
            replyMessage.To = message.Customer;
            Actor.Send(replyMessage);
        }

        public void Accept(Set<T> message)
        {
            _contents = message.Body;
        }
    }
}