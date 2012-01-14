using System;
using System.Collections.Generic;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace AD.ORM.NHibernate
{
    /// <summary>
    /// A <see cref="IUserType"/> to manage relationships with a well known entities.
    /// </summary>
    /// <typeparam name="T">The type of the well known entity</typeparam>
    /// <remarks>
    /// <typeparamref name="T"/> is the type tp use in the entity owning the relation, the type in the persistence is <see cref="int"/>.
    /// </remarks>
    [Serializable]
    public abstract class WellKnownInstanceType<T> : GenericWellKnownInstanceType<T, Int64> where T : class
    {
        private static readonly SqlType[] _sqlTypes = new[] { SqlTypeFactory.Int32 };

        /// <summary>
        /// Base constructor
        /// </summary>
        /// <param name="repository">The collection that represent a in-memory repository.</param>
        /// <param name="findPredicate">The predicate an instance by the persisted value.</param>
        /// <param name="idGetter">The getter of the persisted value.</param>
        protected WellKnownInstanceType(IEnumerable<T> repository, Func<T, Int64, bool> findPredicate, Func<T, Int64> idGetter)
            : base(repository, findPredicate, idGetter)
        {
        }

        public override SqlType[] SqlTypes
        {
            get { return _sqlTypes; }
        }
    }
}
