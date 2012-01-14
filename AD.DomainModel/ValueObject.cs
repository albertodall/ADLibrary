using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace AD.DomainModel
{
    /// <summary>
    /// Classe base per tutti i value objects.
    /// Presa da <see cref="http://elegantcode.com/2009/06/07/generic-value-object/"/>
    /// </summary>
    [Serializable]
    public abstract class ValueObject<T> : IEquatable<T> where T : ValueObject<T>
    {
        private List<PropertyInfo> Properties { get; set; }

        protected ValueObject()
        {
            Properties = new List<PropertyInfo>();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != GetType()) return false;

            return Equals(obj as T);
        }

        public override int GetHashCode()
        {
            int hashCode = 29;
            foreach (var property in Properties)
            {
                var propertyValue = property.GetValue(this, null);
                if (propertyValue == null)
                    continue;

                hashCode = (hashCode.GetHashCode() << 5) ^ propertyValue.GetHashCode();
            }

            return hashCode;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var property in Properties)
            {
                var propertyValue = property.GetValue(this, null);
                if (propertyValue == null)
                    continue;

                builder.Append(propertyValue.ToString());
            }

            return builder.ToString();
        }

        protected void RegisterProperty(Expression<Func<T, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            MemberExpression memberExpression;
            if (ExpressionType.Convert == expression.Body.NodeType)
            {
                var body = (UnaryExpression)expression.Body;
                memberExpression = body.Operand as MemberExpression;
            }
            else
            {
                memberExpression = expression.Body as MemberExpression;
            }

            if (memberExpression == null)
            {
                throw new InvalidOperationException();
            }

            Properties.Add(memberExpression.Member as PropertyInfo);
        }

        #region IEquatable<T> Members

        public bool Equals(T other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            foreach (var property in Properties)
            {
                var oneValue = property.GetValue(this, null);
                var otherValue = property.GetValue(other, null);

                if (oneValue == null || otherValue == null) return false;
                if (oneValue.Equals(otherValue) == false) return false;
            }

            return true;
        }

        #endregion

        public static bool operator ==(ValueObject<T> x, ValueObject<T> y)
        {
            if (ReferenceEquals(x, null))
                return ReferenceEquals(y, null);

            return x.Equals(y);
        }

        public static bool operator !=(ValueObject<T> x, ValueObject<T> y)
        {
            return !(x == y);
        }
    }
}