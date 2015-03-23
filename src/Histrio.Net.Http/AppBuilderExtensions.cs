using Owin;

namespace Histrio.Net.Http
{
    public static class AppBuilderExtensions
    {
        public static IAppBuilder UseHistrio(this IAppBuilder appBuilder, HistrioSettings histrioSettings)
        {
            appBuilder.Use(new HistrioMiddleware(histrioSettings).MidFunc);
            return appBuilder;
        }
    }
}