using System;
using System.Runtime.CompilerServices;
using Histrio.Behaviors;

namespace Histrio.Expressions
{
    public static class New
    {
        public static IAddress Actor(BehaviorBase behavior)
        {
            return Actor(behavior, Theater.GetInstance());
        }

        public static IAddress Actor(BehaviorBase behavior, Theater theater)
        {
            var uriString = string.Format("uan://{0}/{1}", theater.Name, Guid.NewGuid());
            var universalActorName = new Uri(uriString);
            var address = new Address(universalActorName, theater);
            theater.Register(address, behavior);
            return address;
        }
    }
}