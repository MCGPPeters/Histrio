using System;
using Histrio.Behaviors;

namespace Histrio.Expressions
{
    public static class New
    {
        public static IAddress Actor(BehaviorBase behavior)
        {
            var address =
                new Address(new Uri(string.Format("actor://localhost/{0}/{1}", Context.System.Name, Guid.NewGuid())));
            Context.System.Register(address, behavior);
            return address;
        }
    }
}