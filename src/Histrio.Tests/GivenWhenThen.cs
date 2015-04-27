using System;

namespace Histrio.Tests
{
    public class GivenWhenThen : global::Chill.GivenWhenThen
    {
        public void Then(Action action)
        {
            action();
        }
    }
}