using System.Threading;
using System.Threading.Tasks;
using Chill;
using FluentAssertions;

using Histrio.Behaviors;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Histrio.Tests.Bus
{
    [TestClass]
    public class When_publishing_a_message : GivenSubject<System>
    {
        private IAddress _addressOfTheFirstSubscriber;
        private IAddress _addressOfTheSecondSubscriber;
        private IAddress _addressOfTheThirdSubscriber;
        private IAddress _addressOfTheBus;
        
        private object _messageReceivedByFirstSubscriber;
        private object _messageReceivedBySecondSubscriber;
        private object _messageReceivedByThirdSubscriber;
       
        private int _expectedValue;
        
        private Topic _topic;
        private Topic _secondTopic;

        public When_publishing_a_message()
        {
            Given(async () =>
            {
                The<IContainer>().Get<BusBehaviorBase>().Returns(new GetEventStoreBusBehavior());
                 
                _addressOfTheBus = Subject.AddressOf<BusBehaviorBase>();
                _topic = new Topic("ones");
                _secondTopic = new Topic("twos");
                _expectedValue = 1;

                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
                var taskFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
                _addressOfTheFirstSubscriber = Subject.AddressOf(new CallBackBehavior<object>(v => _messageReceivedByFirstSubscriber =  v, taskFactory));
                _addressOfTheSecondSubscriber = Subject.AddressOf(new CallBackBehavior<object>(v => _messageReceivedBySecondSubscriber = v, taskFactory));
                _addressOfTheThirdSubscriber = Subject.AddressOf(new CallBackBehavior<object>(v => _messageReceivedByThirdSubscriber = v, taskFactory));

                await _addressOfTheBus.Receive(new Publish(_expectedValue, _topic));
                await _addressOfTheBus.Receive(new Publish(2, _secondTopic));
            });

            When(async () =>
            {
                await _addressOfTheBus.Receive(new Subscribe(_topic, _addressOfTheFirstSubscriber));
                await _addressOfTheBus.Receive(new Subscribe(_topic, _addressOfTheSecondSubscriber));
                await _addressOfTheBus.Receive(new Subscribe(_secondTopic, _addressOfTheThirdSubscriber));
            });
        }

        [TestMethod]
        public void Then_first_subscriber_receives_the_message()
        {
            while (_messageReceivedByFirstSubscriber == null)
            {
            }
            _messageReceivedByFirstSubscriber.ShouldBeEquivalentTo(_expectedValue);
        }
        
        [TestMethod]
        public void Then_second_subscriber_receives_the_message()
        {
            while (_messageReceivedBySecondSubscriber == null)
            {
            }
            _messageReceivedBySecondSubscriber.ShouldBeEquivalentTo(_expectedValue);
        }


        [TestMethod]
        public void Then_third_subscriber_receives_the_message()
        {
            while (_messageReceivedByThirdSubscriber == null)
            {
            }
            _messageReceivedByThirdSubscriber.ShouldBeEquivalentTo(2);
        }
    }
}
