using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using Newtonsoft.Json;

namespace Histrio.Net.Http.Controlers
{
    internal class TheaterController : ApiController
    {
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
            var conversionType = Type.GetType(untypedMessage.AssemblyQualifiedName);
            var jtoken = untypedMessage.Body;
            var messageBody = JsonConvert.DeserializeObject(jtoken.ToString(), conversionType);
            var dispatchMethod = DispatchMethodInfo.MakeGenericMethod(conversionType);

            var universalActorName = untypedMessage.Address;

            dispatchMethod.Invoke(this, new[] {_theater, messageBody, universalActorName});

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        // ReSharper disable once UnusedMember.Local
        // This mthod is called using reflection
        private static void Dispatch<TMessage>(Theater theater,
            TMessage body, string universalActorName)
            where TMessage : class
        {
            var message = new Message<TMessage>(body);
            theater.Dispatch(message, universalActorName);
        }
    }
}