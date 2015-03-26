using Histrio.Behaviors;

namespace Histrio.Expressions
{
    public static class New
    {
        public static Address Actor(BehaviorBase behavior, Theater theater)
        {
            return theater.CreateActor(behavior);
        }
    }
}