using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Chill;
using FluentAssertions;
using Histrio.Net.Http;
using Histrio.Testing;
using Microsoft.Owin.Builder;
using Xunit;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace Histrio.Tests.Http
{
    public abstract class When_sending_a_message_to_a_remote_actor_that_is_accessible_via_http<T> :
        GivenSubject<Theater>
    {
        private readonly T _message;

        private readonly TaskCompletionSource<T> _promiseOfTheActualValue =
            new TaskCompletionSource<T>();

        private readonly Uri endpointAddress = new Uri("http://remotehost");
        private Address _remoteActor;

        protected When_sending_a_message_to_a_remote_actor_that_is_accessible_via_http(T message)
        {
            _message = message;
            Given(() =>
            {
                var inMemoryNamingService = new InMemoryActorNamingService();
                SetThe<IActorNamingService>().To(inMemoryNamingService);

                var remoteTheater = new Theater(inMemoryNamingService);
                var remoteAppBuilder = new AppBuilder();
                remoteTheater.AddHttpEndPoint(endpointAddress, remoteAppBuilder);
                
                var localAppBuilder = new AppBuilder();
                Subject.AddHttpEndPoint(endpointAddress, localAppBuilder);

                var localHttpClient = BuildHttpClient(localAppBuilder.Build());
                var remoteHttpClient = BuildHttpClient(remoteAppBuilder.Build());

                Subject.PermitMessageDispatchOverHttp(remoteHttpClient);
                remoteTheater.PermitMessageDispatchOverHttp(localHttpClient);

                _remoteActor = remoteTheater.CreateActor(new AssertionBehavior<T>(_promiseOfTheActualValue, 1));
            });

            When(() => { Subject.Dispatch(message, _remoteActor); });
        }

        [Fact]
        public async Task The_remote_actor_should_receive_the_message()
        {
            var actualMessage = await _promiseOfTheActualValue.Task;

            actualMessage.ShouldBeEquivalentTo(_message);
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

    public class When_sending_a_non_nested_message_to_a_remote_actor_that_is_accessible_via_http :
        When_sending_a_message_to_a_remote_actor_that_is_accessible_via_http<SomethingHappened>
    {
        public When_sending_a_non_nested_message_to_a_remote_actor_that_is_accessible_via_http()
            : base(new SomethingHappened("Hell froze over ..."))
        {
        }
    }
    
    public class When_sending_a_nested_message_to_a_remote_actor_that_is_accessible_via_http :
        When_sending_a_message_to_a_remote_actor_that_is_accessible_via_http<Nested<SomethingHappened>>
    {
        public When_sending_a_nested_message_to_a_remote_actor_that_is_accessible_via_http()
            : base(new Nested<SomethingHappened>(new SomethingHappened("Hell froze over ...")))
        {
        }
    }

    public class When_sending_a_nested_generic_message_to_a_remote_actor_that_is_accessible_via_http :
        When_sending_a_message_to_a_remote_actor_that_is_accessible_via_http<Nested<Nested<SomethingHappened>>>
    {
        public When_sending_a_nested_generic_message_to_a_remote_actor_that_is_accessible_via_http()
            : base(new Nested<Nested<SomethingHappened>>(new Nested<SomethingHappened>(new SomethingHappened("Hell froze over ..."))))
        {
        }
    }


    /// <summary>
    /// Creates a fixture for the fact that an address needs to be serializable (public constructor)
    /// </summary>
    public class When_sending_a_message_containing_an_address_to_a_remote_actor_that_is_accessible_via_http :
        When_sending_a_message_to_a_remote_actor_that_is_accessible_via_http<WithAddress>
    {
        public When_sending_a_message_containing_an_address_to_a_remote_actor_that_is_accessible_via_http()
            : base(new WithAddress { Address = new Address("foo") })
        {
        }
    }


    public class When_sending_a_message_as_its_abstract_base_to_a_remote_actor_that_is_accessible_via_http :
        When_sending_a_message_to_a_remote_actor_that_is_accessible_via_http<Base>
    {
        public When_sending_a_message_as_its_abstract_base_to_a_remote_actor_that_is_accessible_via_http()
            : base(new Derived())
        {
        }
    }

    public abstract class Base
    {
        public string Someprop { get; set; }
    }

    class Derived : Base
    {
        public Derived()
        {
            Someprop = "foo";
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

    public class WithAddress
    {
        public Address Address { get; set; }
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