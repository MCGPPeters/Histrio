using System.Threading.Tasks;
using FluentAssertions;
using Histrio.Testing;
using Xunit;

namespace Histrio.Tests.Cell
{
    public class When_getting_a_value_from_a_cell : GivenWhenThen
    {
        [Theory,
         InlineData(true),
         InlineData(333),
         InlineData(new[] {true, true}),
         InlineData("foo")]
        public void Then_value_the_cell_is_holding_on_to_is_returned<T>(T expectedValue)
        {
            var theater = new Theater();
            var taskCompletionSource = new TaskCompletionSource<Reply<T>>();
            ;
            Address customer = null;
            Address cell = null;

            Given(() =>
            {
                cell = theater.CreateActor(new CellBehavior<T>());
                var set = new Set<T>(expectedValue);
                theater.Dispatch(set, cell);
                customer = theater.CreateActor(new AssertionBehavior<Reply<T>>(taskCompletionSource, 1));
            });

            When(() =>
            {
                var get = new Get(customer);
                theater.Dispatch(get, cell);
            });

            Then(async () =>
            {
                var actualValue = await taskCompletionSource.Task;
                actualValue.Content.ShouldBeEquivalentTo(expectedValue);
            });
        }
    }
}