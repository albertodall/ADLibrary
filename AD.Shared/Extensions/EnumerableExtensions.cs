using System;
using System.Collections.Generic;
using AD.Shared.Collections;

namespace AD.Shared.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (T item in items)
            {
                action(item);
            }
        }

        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> source, int index, int pageSize, int totalItemCount)
        {
            return new PagedList<T>(source, index, pageSize, totalItemCount);
        }
    }
}
