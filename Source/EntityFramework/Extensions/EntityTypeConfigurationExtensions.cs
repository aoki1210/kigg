namespace Kigg.Infrastructure.EntityFramework
{
    using System;
    using System.Reflection;
    using System.Linq.Expressions;
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.ModelConfiguration.Configuration;

    internal static class EntityTypeConfigurationExtensions
    {
        public static ManyNavigationPropertyConfiguration<T, U> HasMany<T, U>(this EntityTypeConfiguration<T> mapper, string propertyName)
            where T : class
            where U : class
        {
            LambdaExpression lambda = CreateLambdaExpression<T>(propertyName);

            var expression = (Expression<Func<T, ICollection<U>>>)lambda;

            return mapper.HasMany(expression);

        }

        public static PrimitivePropertyConfiguration Property<T, TProp>(this EntityTypeConfiguration<T> mapper, string propertyName)
            where T : class
            where TProp : struct
        {
            LambdaExpression lambda = CreateLambdaExpression<T>(propertyName);

            var expression = (Expression<Func<T, TProp>>)lambda;

            return mapper.Property(expression);
        }

        private static LambdaExpression CreateLambdaExpression<T>(string propertyName) where T : class
        {
            Type type = typeof(T);

            ParameterExpression arg = Expression.Parameter(type, "x");

            Expression expr = arg;

            PropertyInfo pi = type.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            expr = Expression.Property(expr, pi);

            LambdaExpression lambda = Expression.Lambda(expr, arg);

            return lambda;
        }
    }
}
