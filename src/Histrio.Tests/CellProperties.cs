using System.Threading.Tasks;
using Xunit;
using FsCheck.Xunit;

namespace Histrio.Tests
{
    public class CellProperties
    {
        [Property(DisplayName = "When setting a cells value, that value can be retrieved")]
        public async Task SettingEnablesGetting(int value)
        {
            var cell = Primitives.Create(new CellBehavior<int>());
            cell.Accept(new Set<int>(value));
            var taskCompletionSource = new TaskCompletionSource<Reply<int>>();
            var customer = Primitives.Create(new AssertionBehavior<Reply<int>>(taskCompletionSource, 1));
            cell.Accept(new Get{ Customer = customer});

            var actual = await taskCompletionSource.Task;

            Assert.Equal(value, actual.Value);

        }
    }

    internal class CellBehavior<T> : IBehavior
    {
    }
}
