using System.Threading;
using System.Threading.Tasks;

using Chill;

using FluentAssertions;

using Histrio.Behaviors;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NSubstitute;

namespace Histrio.Tests.Bus.Transformation
{
    [TestClass]
    public class When_transforming_an_message : GivenSubject<System>
    {
        private IAddress _addressOfTheCustomer;
        private IAddress _addressOfTheTransformation;

        private TargetPerson _actualTargetPerson;
        private readonly TargetPerson _expectedTargetPerson = new TargetPerson { FirstName = "John", LastName = "Doe" };
        private readonly SourcePerson _sourcePerson = new SourcePerson { FamilyName = "Doe", GivenName = "John" };

        public When_transforming_an_message()
        {
            Given(() =>
            {
                The<ITransformation<SourcePerson>>().Transform<TargetPerson>(_sourcePerson).Returns(_expectedTargetPerson);
                The<IContainer>().Get<TransformationBehavior<SourcePerson, TargetPerson>>().Returns(new TransformationBehavior<SourcePerson, TargetPerson>(The<ITransformation<SourcePerson>>()));
                 
                _addressOfTheTransformation = Subject.AddressOf<TransformationBehavior<SourcePerson, TargetPerson>>();

                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
                var taskFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
                _addressOfTheCustomer = Subject.AddressOf(new CallBackBehavior<TargetPerson>(v => _actualTargetPerson =  v, taskFactory));
            });

            When(async () =>
            {
                await _addressOfTheTransformation.Receive(new Transform<SourcePerson>(_sourcePerson, _addressOfTheCustomer));
            });
        }

        [TestMethod]
        public void Then_the_customer_receives_the_transformed_message()
        {
            while (_actualTargetPerson == null)
            {
            }
            _actualTargetPerson.ShouldBeEquivalentTo(_expectedTargetPerson);
        }
    }
}
