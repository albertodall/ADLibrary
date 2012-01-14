using System;
using System.Collections.Generic;

namespace AD.Shared.Collections
{
    /// <summary>
    /// Represents a subset of a collection of objects that can be individually accessed by index and containing metadata about the superset collection of objects this subset was created from.
    /// </summary>
    /// <typeparam name="T">The type of object the collection should contain.</typeparam>
    /// <seealso cref="IPagedList{T}"/>
    /// <seealso cref="List{T}"/>
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class that contains the already divided subset and information about the size of the superset and the subset's position within it.
        /// </summary>
        /// <param name="source">The single subset this collection should represent.</param>
        /// <param name="index">The index of the subset of objects contained by this instance.</param>
        /// <param name="pageSize">The maximum size of any individual subset.</param>
        /// <param name="totalItemCount">The size of the superset.</param>
        /// <exception cref="ArgumentOutOfRangeException">The specified index cannot be less than zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The specified page size cannot be less than one.</exception>
        public PagedList(IEnumerable<T> source, int index, int pageSize, int totalItemCount)
        {  
            // Set source to blank list if source is null to prevent exceptions  
            if (source == null)  
                source = new List<T>();  
             
            TotalItemCount = totalItemCount;
            PageSize = pageSize;
            PageIndex = index;

            if (TotalItemCount > 0)  
                PageCount = (int)Math.Ceiling(TotalItemCount / (double)PageSize);  
            else  
                PageCount = 0;
             
             // Argument checking
             if (index < 0) 
                 throw new ArgumentOutOfRangeException("index", index, "PageIndex cannot be below 0.");
             if (pageSize < 1)
                 throw new ArgumentOutOfRangeException("pageSize", pageSize, "PageSize cannot be less than 1.");
             
             // Add items to the internal list  
             if (TotalItemCount > 0)
                 AddRange(source);  
         }

        /// <summary>
        /// Total number of subsets within the superset.
        /// </summary>
        public int PageCount { get; private set; }

        /// <summary>
        /// Total number of objects contained within the superset.
        /// </summary>
        public int TotalItemCount { get; private set; }

        /// <summary>
        /// Zero-based index of this subset within the superset.
        /// </summary>
        public int PageIndex { get; private set; }

        /// <summary>
        /// Maximum size any individual subset.
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// One-based index of this subset within the superset.
        /// </summary>
        public int PageNumber
        {
            get { return PageIndex + 1; }
        }

        /// <summary>
        /// Returns true if this is NOT the first subset within the superset.
        /// </summary>
        public bool HasPreviousPage
        {
            get { return PageIndex > 0; }
        }

        /// <summary>
        /// Returns true if this is NOT the last subset within the superset.
        /// </summary>
        public bool HasNextPage 
        {
            get { return PageIndex < (PageCount - 1); }
        }

        /// <summary>
        /// Returns true if this is the first subset within the superset.
        /// </summary>
        public bool IsFirstPage
        {
            get { return PageIndex <= 0; }
        }

        /// <summary>
        /// Returns true if this is the last subset within the superset.
        /// </summary>
        public bool IsLastPage
        { 
            get { return PageIndex >= (PageCount - 1); } 
        }
    }
}
