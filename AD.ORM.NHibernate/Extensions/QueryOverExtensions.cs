using AD.Shared.Collections;
using NHibernate;

namespace AD.ORM.NHibernate.Extensions
{
    public static class QueryOverExtensions
    {
        public static PagedList<T> ToPagedList<T>(this IQueryOver<T, T> queryOver, int pageIndex, int pageSize)
        {
            var rowCountQuery = queryOver.ToRowCountQuery();
            var list = queryOver.Skip(pageIndex * pageSize).Take(pageSize).Future();
            var totalCount = rowCountQuery.FutureValue<int>().Value;
            return new PagedList<T>(list, pageIndex, pageSize, totalCount);
        }
    }
}
