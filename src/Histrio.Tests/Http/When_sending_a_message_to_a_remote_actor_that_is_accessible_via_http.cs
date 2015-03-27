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
    public abstract class When_sending_a_message_to_a_remote_actor_that_is_accessible_via_http<T> : GivenSubject<Theater>
    {
        private readonly T _message;

        private readonly TaskCompletionSource<T> _promiseOfTheActualValue =
            new TaskCompletionSource<T>();

        private readonly Uri _universalActorLocationOfRemoteActor = new Uri("http://remotehost");
        private Address _remoteActor;

        protected When_sending_a_message_to_a_remote_actor_that_is_accessible_via_http(T message)
        {
            _message = message;
            Given(() =>
            {
                The<IActorNamingService>()
                    .ResolveActorLocation(Arg.Any<Address>())
                    .Returns(_universalActorLocationOfRemoteActor);

                var inMemoryNamingService = new InMemoryNamingService();
                var remoteTheater = new Theater(inMemoryNamingService);
                _remoteActor =
                    remoteTheater.CreateActor(new AssertionBehavior<T>(_promiseOfTheActualValue, 1));

                var appFunc = BuildHistrioMiddleware(remoteTheater);
                var remoteHttpClient = BuildHttpClient(appFunc);

                var localAppFunc = BuildHistrioMiddleware(Subject);
                var localHttpClient = BuildHttpClient(localAppFunc);

                Subject.PermitMessageDispatchOverHttp(remoteHttpClient);
                remoteTheater.PermitMessageDispatchOverHttp(localHttpClient);
            });

            When(() =>
            {
                var messageToSend = message.AsMessage();
                messageToSend.To = _remoteActor;
                Subject.Dispatch(messageToSend);
            });
        }

        [Fact]
        public async Task The_remote_actor_should_receive_the_message()
        {
            var actualMessage = await _promiseOfTheActualValue.Task;

            actualMessage.ShouldBeEquivalentTo(_message);
        }

        private static AppFunc BuildHistrioMiddleware(Theater theater)
        {
            var appBuilder = new AppBuilder();
            var histrioSettings = new TheaterSettings
            {
                Theater = theater
            };
            appBuilder.UseTheater(histrioSettings);
            var appFunc = appBuilder.Build();
            return appFunc;
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

    public class When_sending_a_non_nested_message_to_a_remote_actor_that_is_accessible_via_http : When_sending_a_message_to_a_remote_actor_that_is_accessible_via_http<SomethingHappened>
    {
        public When_sending_a_non_nested_message_to_a_remote_actor_that_is_accessible_via_http() : base(new SomethingHappened("Hell froze over ..."))
        {
        }
    }

    public class When_sending_a_nested_message_to_a_remote_actor_that_is_accessible_via_http : When_sending_a_message_to_a_remote_actor_that_is_accessible_via_http<Nested<SomethingHappened>>
    {
        public When_sending_a_nested_message_to_a_remote_actor_that_is_accessible_via_http() : base(new Nested<SomethingHappened>(new SomethingHappened("Hell froze over ...")))
        {
        }
    }

    public class Nested<T>
    {
        public Nested(T inner)
        {
            Inner = inner;
        }

        public T Inner { get; private set; }
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