namespace OpenSourceCurrencyApi.Structures.Models
{
    public sealed class Node<T>
    {
        /// <summary>
        /// Gets the Value
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Gets next node
        /// </summary>
        public Node<T> Next { get; set; }

        /// <summary>
        /// Gets previous node
        /// </summary>
        public Node<T> Previous { get; set; }

        /// <summary>
        /// Initializes a new <see cref="Node"/> instance
        /// </summary>
        /// <param name="item">Value to be assigned</param>
        internal Node(T item)
        {
            this.Value = item;
        }
    }

}
