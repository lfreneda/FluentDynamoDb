using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentDynamoDb
{
    public class ClassMap<TEntity>
    {
        private readonly DynamoDbEntityConfiguration _dynamoDbEntityConfiguration;

        public ClassMap()
            : this(new DynamoDbEntityConfiguration())
        {

        }

        internal ClassMap(DynamoDbEntityConfiguration dynamoDbEntityConfiguration)
        {
            _dynamoDbEntityConfiguration = dynamoDbEntityConfiguration;
        }

        protected void Map(Expression<Func<TEntity, object>> propertyExpression)
        {
            MemberExpression memberExpression = null;

            if (propertyExpression.Body.NodeType == ExpressionType.Convert)
                memberExpression = ((UnaryExpression)propertyExpression.Body).Operand as MemberExpression;
            else if (propertyExpression.Body.NodeType == ExpressionType.MemberAccess)
                memberExpression = propertyExpression.Body as MemberExpression;

            if (memberExpression == null || memberExpression.Member == null)
                throw new ArgumentNullException("propertyExpression", "Not a member access!");

            var propertyInfo = (memberExpression.Member as PropertyInfo);
            if (propertyInfo != null)
            {
                var fieldConfiguration = CreateFieldWith(propertyInfo);
                _dynamoDbEntityConfiguration.AddFieldConfiguration(fieldConfiguration);
            }
        }

        private static IFieldConfiguration CreateFieldWith(PropertyInfo propertyInfo)
        {
            return new FieldConfiguration
            {
                PropertyName = propertyInfo.Name,
                Type = propertyInfo.PropertyType
            };
        }

        public DynamoDbEntityConfiguration GetConfiguration()
        {
            return _dynamoDbEntityConfiguration;
        }
    }
}