using System.Threading.Tasks;

using Histrio.Behaviors;

namespace Histrio.Tests.Bus.Transformation
{
    public class TransformationBehavior<TSource, TTarget> : BehaviorBase, IHandle<Transform<TSource>>
    {
        private readonly ITransformation<TSource> transformation;

        public TransformationBehavior(ITransformation<TSource> transformation)
        {
            this.transformation = transformation;
        }

        public async Task Accept(Transform<TSource> message)
        {
            await message.AddressOfTheCustomer.Receive(transformation.Transform<TTarget>(message.Source));
        }
    }
}