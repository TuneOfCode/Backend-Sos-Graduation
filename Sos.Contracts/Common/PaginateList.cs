namespace Sos.Contracts.Common
{
    /// <summary>
    /// Represents the generic paginate list.
    /// </summary>
    /// <typeparam name="T">The type of list.</typeparam>
    public sealed class PaginateList<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaginateList{T}"/> class.
        /// </summary>
        /// <param name="data">The data of the list.</param>
        /// <param name="page">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="totalCount">The total count.</param>
        public PaginateList(
            IEnumerable<T> data,
            int page,
            int pageSize,
            int totalCount
        )
        {
            Data = data.ToList();
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        public IReadOnlyCollection<T> Data { get; }

        /// <summary>
        /// Gets the page.
        /// </summary>
        public int Page { get; private set; }

        /// <summary>
        /// Gets the page size.
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// Gets the total count.
        /// </summary>
        public int TotalCount { get; }

        /// <summary>
        /// Gets the flag indicating whether the next page exists.
        /// </summary>
        public bool HasNextPage => Page * PageSize < TotalCount;

        /// <summary>
        /// Gets the flag indicating whether the previous page exists.
        /// </summary>
        public bool HasPreviousPage => Page > 1;
    }
}
