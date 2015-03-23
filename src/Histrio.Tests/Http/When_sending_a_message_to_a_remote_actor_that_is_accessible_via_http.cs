using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Chill;
using FluentAssertions;
using Histrio.Behaviors;
using Histrio.Commands;
using Histrio.Expressions;
using Histrio.Net.Http;
using Histrio.Tests.Factorial;
using Microsoft.Owin.Builder;
using Xunit;
using NSubstitute;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace Histrio.Tests.Http
{
    public class When_sending_a_message_to_a_remote_actor_that_is_accessible_via_http : GivenSubject<Theater>
    {
        private readonly TaskCompletionSource<SomethingHappened> _promiseOfTheActualValue =
                new TaskCompletionSource<SomethingHappened>();

        private readonly Uri _universalActorLocationOfRemoteActor = new Uri("http://localhost");
        private IAddress _remoteActor;
        private const string TheThingThatHappened = "Hell froze over...";

        public When_sending_a_message_to_a_remote_actor_that_is_accessible_via_http()
        {
            Given(() =>
            {
                //make sure the remote actor is created in another theatre that will be simulating the remote theater
                var remoteTheater = new Theater(The<IActorNamingService>());

                _remoteActor = New.Actor(new TaskCompletionBehavior<SomethingHappened>(_promiseOfTheActualValue, 1), remoteTheater);
                The<IActorNamingService>()
                    .ResolveActorLocation(_remoteActor.UniversalActorName)
                    .Returns(_universalActorLocationOfRemoteActor);

                var appBuilder = new AppBuilder();
                var histrioSettings = new HistrioSettings
                {
                    Theater = remoteTheater
                };
                appBuilder.UseHistrio(histrioSettings);
                AppFunc appFunc = appBuilder.Build();
                var httpClient = BuildHttpClient(appFunc);


                Subject.PermitMessageDispatchOverHttp(httpClient);
            });
            
            When(() =>
            {
                var somethingHappened = new SomethingHappened(TheThingThatHappened);
                Send.Message(somethingHappened).To(_remoteActor);
            });
        }

        [Fact]
        public async Task The_remote_actor_should_receive_the_message()
        {
            var actualMessage = await _promiseOfTheActualValue.Task;

            actualMessage.TheThingThatHappened.Should().Be(TheThingThatHappened);
        }

        private static HttpClient BuildHttpClient(AppFunc appFunc)
        {
            HttpMessageHandler handler = new OwinHttpMessageHandler(appFunc)
            {
                AllowAutoRedirect = true,
                CookieContainer = new CookieContainer(),
                UseCookies = true
            };
            var httpClient = new HttpClient(handler);
            return httpClient;
        }
    }

    public class SomethingHappened
    {
        public SomethingHappened(string theThingThatHappened)
        {
            TheThingThatHappened = theThingThatHappened;
        }

        public string TheThingThatHappened { get; set; }
    }
}
