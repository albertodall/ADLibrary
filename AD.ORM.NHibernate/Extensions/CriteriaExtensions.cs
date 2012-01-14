using System.Collections.Generic;
using NHibernate;
using AD.Shared.Collections;
using NHibernate.Criterion;

namespace AD.ORM.NHibernate.Extensions
{
    public static class CriteriaExtensions
    {
        public static PagedList<T> ToPagedList<T>(this ICriteria criteria, int pageIndex, int pageSize)
        {
            var rowCountQuery = criteria.SetProjection(Projections.Count(Projections.Id()));
            IEnumerable<T> list = criteria.SetFirstResult(pageIndex * pageSize).SetMaxResults(pageSize).Future<T>();
            var totalCount = rowCountQuery.FutureValue<int>().Value;
            return new PagedList<T>(list, pageIndex, pageSize, totalCount);
        }
    }
}
