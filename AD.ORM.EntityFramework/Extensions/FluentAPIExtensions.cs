using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using System.Reflection;

namespace AD.ORM.EntityFramework.Extensions
{
    /// <summary>
    /// extension methods that help FluentAPI mapping IEnumerable decalred child collections.
    /// </summary>
    public static class FluentAPIExtensions
    {
        private static Expression<Func<T, K>> CreateExpression<T, K>(string propertyName)
        {
            Type type = typeof (T);
            PropertyInfo pi = type.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (pi == null) throw new ArgumentException("propertyName is not valid.");
            ParameterExpression argumentExpression = Expression.Parameter(type, "x");
            MemberExpression memberExpression = Expression.Property(argumentExpression, pi);
            LambdaExpression lambda = Expression.Lambda(memberExpression, argumentExpression);
            Expression<Func<T, K>> expression = (Expression<Func<T, K>>) lambda;

            return expression;
        }

        public static PrimitivePropertyConfiguration Property<TEntity, TPropertyType>(this EntityTypeConfiguration<TEntity> mapper, string propertyName)
            where TEntity : class
            where TPropertyType : struct
        {
            Expression<Func<TEntity, TPropertyType>> expression = CreateExpression<TEntity, TPropertyType>(propertyName);
            return mapper.Property(expression);
        }

        public static DependentNavigationPropertyConfiguration<TEntity> WithMany<TEntity, TTargetEntity>(this OptionalNavigationPropertyConfiguration<TEntity, TTargetEntity> mapper, string propertyName)
            where TEntity : class
            where TTargetEntity : class
        {
            Type type = typeof (TTargetEntity);
            ParameterExpression argumentExpression = Expression.Parameter(type, "x");
            PropertyInfo pi = type.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            MemberExpression memberExpression = Expression.Property(argumentExpression, pi);
            LambdaExpression lambda = Expression.Lambda(memberExpression, argumentExpression);
            var expression = (Expression<Func<TTargetEntity, ICollection<TEntity>>>) lambda;

            return mapper.WithMany(expression);
        }

        public static RequiredNavigationPropertyConfiguration<TEntity, TTargetEntity> HasRequired<TEntity, TTargetEntity>(this EntityTypeConfiguration<TEntity> mapper, string propertyName)
            where TEntity : class
            where TTargetEntity : class
        {
            Expression<Func<TEntity, TTargetEntity>> expression = CreateExpression<TEntity, TTargetEntity>(propertyName);
            return mapper.HasRequired(expression);
        }

        public static EntityTypeConfiguration<TEntity> HasKey<TEntity, TKey>(this EntityTypeConfiguration<TEntity> mapper, string propertyName)
            where TEntity : class
            where TKey : struct
        {
            Expression<Func<TEntity, TKey>> expression = CreateExpression<TEntity, TKey>(propertyName);
            EntityTypeConfiguration<TEntity> m = mapper.HasKey<TKey>(expression);
            return mapper;
        }

        public static ManyNavigationPropertyConfiguration<TSource, TDestination> HasMany<TSource, TDestination>(this EntityTypeConfiguration<TSource> mapper, string propertyName)
            where TSource : class
            where TDestination : class
        {
            Type type = typeof(TSource);
            PropertyInfo pi = type.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (pi == null) throw new ArgumentException("propertyName is not valid.");
            ParameterExpression parameterExpression = Expression.Parameter(type, "x");
            MemberExpression memberExpression = Expression.Property(parameterExpression, pi);
            LambdaExpression lambda = Expression.Lambda(memberExpression, parameterExpression);
            Expression<Func<TSource, ICollection<TDestination>>> expression = (Expression<Func<TSource, ICollection<TDestination>>>) lambda;
            return mapper.HasMany(expression);
        }
    }
}
