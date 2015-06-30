using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using Histrio.Net.Http.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Histrio.Net.Http.Controlers
{
    internal class TheaterController : ApiController
    {
        private static readonly ILog Logger = LogProvider.For<TheaterController>();

        private static readonly MethodInfo DispatchMethodInfo = typeof (TheaterController)
            .GetMethod("Dispatch", BindingFlags.Static | BindingFlags.NonPublic);

        private readonly Theater _theater;

        internal TheaterController(Theater theater)
        {
            _theater = theater;
        }

        [Route("")]
        public HttpResponseMessage Post([FromBody] UntypedMessage untypedMessage)
        {
            Logger.DebugFormat("A message of type '{0}' arrived at an endpoint for theater '{1}' via http at the url '{2}'",
                untypedMessage.AssemblyQualifiedName, untypedMessage.To, Request.RequestUri.ToString());
            Logger.TraceFormat("Sent message contents : {@message}", untypedMessage);

            var conversionType = Type.GetType(untypedMessage.AssemblyQualifiedName);
            JToken jtoken = (JToken) untypedMessage.Body;
            object deserializedMessageBody;
            try
            {
                deserializedMessageBody = JsonConvert.DeserializeObject(jtoken.ToString(), conversionType, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Unable to deserialize the body of this message : {@message}", ex, untypedMessage);
                throw;
            }
            
            var dispatchMethod = DispatchMethodInfo.MakeGenericMethod(conversionType);
            dispatchMethod.Invoke(this, new[] {_theater, deserializedMessageBody, untypedMessage.To});

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        // ReSharper disable once UnusedMember.Local
        // This mthod is called using reflection
        private static void Dispatch<TMessage>(Theater theater,
            TMessage body, string universalActorName)
            where TMessage : class
        {
            theater.Dispatch(body, universalActorName);
        }
    }
}