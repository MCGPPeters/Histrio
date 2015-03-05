namespace Histrio.Tests.Bus.Transformation
{
    public interface ITransformation<in TSource>
    {
        TTarget Transform<TTarget>(TSource sourcePerson);
    }
}