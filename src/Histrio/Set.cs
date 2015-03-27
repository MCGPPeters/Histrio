namespace Histrio
{
    /// <summary>
    /// A message to send to the <see cref="CellBehavior{T}"/> to store content in it
    /// </summary>
    /// <typeparam name="T">The type of content that is to be stored in the <see cref="CellBehavior{T}"/></typeparam>
    public class Set<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Set{T}"/> class.
        /// </summary>
        /// <param name="content">The content that is to be stored in the <see cref="CellBehavior{T}"/></param>
        public Set(T content)
        {
            Content = content;
        }

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public T Content { get; private set; }
    }
}