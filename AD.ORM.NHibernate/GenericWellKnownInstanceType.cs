using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace AD.ORM.NHibernate
{
    [Serializable]
    public abstract class GenericWellKnownInstanceType<T, TId> : IUserType where T : class
    {
        private readonly Func<T, TId, bool> _findPredicate;
        private readonly Func<T, TId> _idGetter;
        private readonly IEnumerable<T> _repository;

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="repository">La collection che rappresenta il repository in-memory.</param>
        /// <param name="findPredicate">Funzione che ricerca un istanza tramite il suo ID.</param>
        /// <param name="idGetter">Funzione che ritorna il valore persistito.</param>
        protected GenericWellKnownInstanceType(IEnumerable<T> repository, Func<T, TId, bool> findPredicate, Func<T, TId> idGetter)
        {
            _repository = repository;
            _findPredicate = findPredicate;
            _idGetter = idGetter;
        }

        public Type ReturnedType
        {
            get { return typeof(T); }
        }

        public bool IsMutable
        {
            get { return false; }
        }

        public new bool Equals(object x, object y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }
            if (ReferenceEquals(null, x) || ReferenceEquals(null, y))
            {
                return false;
            }

            return x.Equals(y);
        }

        public int GetHashCode(object x)
        {
            return (x == null) ? 0 : x.GetHashCode();
        }

        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            int index0 = rs.GetOrdinal(names[0]);
            if (rs.IsDBNull(index0))
            {
                return null;
            }
            
            var value = (TId)Convert.ChangeType(rs[index0], typeof(TId));   
            //var value = (TId)rs.GetValue(index0);
            return _repository.FirstOrDefault(x => _findPredicate(x, value));
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            if (value == null)
            {
                ((IDbDataParameter)cmd.Parameters[index]).Value = DBNull.Value;
            }
            else
            {
                ((IDbDataParameter)cmd.Parameters[index]).Value = _idGetter((T)value);
            }
        }

        public object DeepCopy(object value)
        {
            return value;
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        public object Disassemble(object value)
        {
            return value;
        }

        /// <summary>
        /// I SQL types delle colonne mappate da questo tipo. 
        /// </summary>
        public abstract SqlType[] SqlTypes { get; }
    }
}
