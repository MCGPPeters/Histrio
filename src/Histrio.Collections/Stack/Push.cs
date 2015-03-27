namespace Histrio.Collections.Stack
{
    /// <summary>
    ///     A message to send to a StackNode (<see cref="StackNodeBehavior{T}" />) to push a value onto it
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Push<T>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Push{T}" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public Push(T value)
        {
            Value = value;
        }

        /// <summary>
        ///     Gets the value.
        /// </summary>
        /// <value>
        ///     The value.
        /// </value>
        public T Value { get; private set; }
    }
}