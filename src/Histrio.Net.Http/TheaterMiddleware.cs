using System.Web.Http;
using System.Web.Http.Dispatcher;
using Histrio.Net.Http.Controlers;
using Histrio.Net.Http.Dispatcher;
using Microsoft.Owin.Builder;
using Owin;
using MidFunc = System.Func
    <System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>,
        System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>>;

namespace Histrio.Net.Http
{
    /// <summary>
    ///     OWIN middleware that exposes a Theater via HTTP
    /// </summary>
    internal class TheaterMiddleware
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TheaterMiddleware" /> class.
        /// </summary>
        /// <param name="theaterSettings">The histrio settings used to configure the middelware</param>
        internal TheaterMiddleware(TheaterSettings theaterSettings)
        {
            var controllerActivator = new TheaterControllerActivator(theaterSettings.Theater);

            MidFunc = next =>
            {
                var app = new AppBuilder();
                app.UseWebApi(GetWebApiConfiguration(controllerActivator));
                app.Run(ctx => next(ctx.Environment));
                return app.Build();
            };
        }

        /// <summary>
        ///     Gets the function representing the middleware
        /// </summary>
        /// <value>
        ///     The function representing the middleware
        /// </value>
        internal MidFunc MidFunc { get; private set; }

        private static HttpConfiguration GetWebApiConfiguration(IHttpControllerActivator controllerActivator)
        {
            var config = new HttpConfiguration();

            config.Services.Replace(typeof (IHttpControllerTypeResolver),
                new SingleHttpControllerTypeResolver<TheaterController>());
            config.Services.Replace(typeof (IHttpControllerActivator),
                controllerActivator);
            config.MapHttpAttributeRoutes();

            return config;
        }
    }
}