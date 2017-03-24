using Owin;

namespace Histrio.Net.Http
{
    /// <summary>
    ///     Appbuilder extensions
    /// </summary>
    public static class AppBuilderExtensions
    {
        /// <summary>
        ///     Uses the histrio.
        /// </summary>
        /// <param name="appBuilder">The application builder.</param>
        /// <param name="theaterSettings">The histrio settings.</param>
        /// <returns></returns>
        public static IAppBuilder UseTheater(this IAppBuilder appBuilder, TheaterSettings theaterSettings)
        {
            appBuilder.Use(new TheaterMiddleware(theaterSettings).MidFunc);
            theaterSettings.Theater.AddEndpoint(theaterSettings.EndpointAddress);
            return appBuilder;
        }
    }
}