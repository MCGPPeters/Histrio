using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Chill;
using FluentAssertions;
using Histrio.Net.Http;
using Histrio.Testing;
using Microsoft.Owin.Builder;
using NSubstitute;
using Xunit;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace Histrio.Tests.Http
{
    public class When_sending_a_message_to_a_remote_actor_that_is_accessible_via_http : GivenSubject<Theater>
    {
        private const string TheThingThatHappened = "Hell froze over...";

        private readonly TaskCompletionSource<SomethingHappened> _promiseOfTheActualValue =
            new TaskCompletionSource<SomethingHappened>();

        private readonly Uri _universalActorLocationOfLocalActor = new Uri("http://localhost");
        private readonly Uri _universalActorLocationOfRemoteActor = new Uri("http://remotehost");
        private IAddress _remoteActor;

        public When_sending_a_message_to_a_remote_actor_that_is_accessible_via_http()
        {
            Given(() =>
            {
                The<IActorNamingService>()
                    .ResolveActorLocation(Arg.Any<Uri>())
                    .Returns(_universalActorLocationOfLocalActor);

                //make sure the remote actor is created in another theatre that will be simulating the remote theater
                // SetThe<IActorNamingService>().Named("remoteActorNamingService").To(new InMemoryNamingService());
                TheNamed<IActorNamingService>("remoteActorNamingService")
                    .ResolveActorLocation(Arg.Any<Uri>())
                    .Returns(_universalActorLocationOfRemoteActor);
                var remoteTheater = new Theater(TheNamed<IActorNamingService>("remoteActorNamingService"));
                _remoteActor =
                    remoteTheater.CreateActor(new AssertionBehavior<SomethingHappened>(_promiseOfTheActualValue, 1));

                var appFunc = BuildHistrioMiddleware(remoteTheater);
                var remoteHttpClient = BuildHttpClient(appFunc);

                var localAppFunc = BuildHistrioMiddleware(remoteTheater);
                var localHttpClient = BuildHttpClient(localAppFunc);

                Subject.PermitMessageDispatchOverHttp(localHttpClient);
                remoteTheater.PermitMessageDispatchOverHttp(remoteHttpClient);
            });

            When(() =>
            {
                var somethingHappened = new SomethingHappened(TheThingThatHappened);
                var somethingHappenedMessage = somethingHappened.AsMessage();
                somethingHappenedMessage.To = _remoteActor;
                Subject.Dispatch(somethingHappenedMessage);
            });
        }

        private static AppFunc BuildHistrioMiddleware(Theater remoteTheater)
        {
            var appBuilder = new AppBuilder();
            var histrioSettings = new HistrioSettings
            {
                Theater = remoteTheater
            };
            appBuilder.UseHistrio(histrioSettings);
            var appFunc = appBuilder.Build();
            return appFunc;
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