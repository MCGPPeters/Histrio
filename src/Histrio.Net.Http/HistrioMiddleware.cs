using System.Web.Http;
using System.Web.Http.Dispatcher;
using Histrio.Net.Http.Dispatcher;
using Microsoft.Owin.Builder;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using MidFunc =
    System.Func
        <System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>,
            System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>>;

namespace Histrio.Net.Http
{
    public class HistrioMiddleware
    {
        public HistrioMiddleware(HistrioSettings histrioSettings)
        {
            MidFunc = next =>
            {
                var app = new AppBuilder();
                app.UseWebApi(GetWebApiConfiguration());
                app.Run(ctx => next(ctx.Environment));
                return app.Build();
            };
        }

        public MidFunc MidFunc { get; }

        private static HttpConfiguration GetWebApiConfiguration()
        {
            var config = new HttpConfiguration();

            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            config.Services.Replace(typeof (IHttpControllerTypeResolver),
                new HttpControllerTypeResolver<HistrioMiddleware>());
            //config.Services.Replace(typeof (IHttpControllerActivator),
            //    new CommonServiceLocatorServiceActivator());
            config.MapHttpAttributeRoutes();

            return config;
        }
    }
}