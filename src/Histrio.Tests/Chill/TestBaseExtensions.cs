namespace Chill.Autofac
{
    public static class TestBaseExtensions
    {
        /// <summary>
        /// Explicitly register a type so that it will be created from the chill container from now on. 
        /// 
        /// This is handy if you wish to create a concrete type from a container that typically doesn't allow
        /// you to do so. (such as autofac)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void RegisterConcreteType<T>(this TestBase testBase) where T : class
        {
            testBase.Container.RegisterType<T>();
        }
    }
}