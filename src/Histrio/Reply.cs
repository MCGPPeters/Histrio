namespace Histrio
{
    /// <summary>
    ///     A message used by <see cref="CellBehavior{T}" /> to send the content to the customer
    /// </summary>
    /// <typeparam name="T">The type of the content stored in the <see cref="CellBehavior{T}" /></typeparam>
    public class Reply<T>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Reply{T}" /> class.
        /// </summary>
        /// <param name="content">The content.</param>
        public Reply(T content)
        {
            Content = content;
        }

        /// <summary>
        ///     Gets the content.
        /// </summary>
        /// <value>
        ///     The content.
        /// </value>
        public T Content { get; private set; }
    }
}