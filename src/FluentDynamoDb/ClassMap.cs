using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentDynamoDb
{
    public class ClassMap<TEntity>
    {
        private readonly DynamoDbRootEntityConfiguration _dynamoDbRootEntityConfiguration;
        private readonly DynamoDbEntityConfiguration _dynamoDbEntityConfiguration;

        public ClassMap()
            : this(new DynamoDbRootEntityConfiguration(), new DynamoDbEntityConfiguration())
        {

        }

        public ClassMap(DynamoDbRootEntityConfiguration dynamoDbRootEntityConfiguration)
            : this(dynamoDbRootEntityConfiguration, new DynamoDbEntityConfiguration())
        {

        }

        internal ClassMap(DynamoDbRootEntityConfiguration dynamoDbRootEntityConfiguration, DynamoDbEntityConfiguration dynamoDbEntityConfiguration)
        {
            _dynamoDbRootEntityConfiguration = dynamoDbRootEntityConfiguration;
            _dynamoDbEntityConfiguration = dynamoDbEntityConfiguration;
        }

        protected void Map<TType>(Expression<Func<TEntity, TType>> propertyExpression)
        {
            var propertyInfo = GetPropertyInfo(propertyExpression);

            if (propertyInfo != null)
            {
                _dynamoDbEntityConfiguration.AddFieldConfiguration(CreateFieldConfigurationWith(propertyInfo));
            }
        }

        protected void References<TType>(Expression<Func<TEntity, TType>> propertyExpression)
        {
            var propertyInfo = GetPropertyInfo(propertyExpression);

            if (propertyInfo != null)
            {
                CreateComplexFieldConfiguration<TType>(propertyInfo);
            }
        }

        protected void HasMany<TType>(Expression<Func<TEntity, IEnumerable<TType>>> propertyExpression)
        {
            var propertyInfo = GetPropertyInfo(propertyExpression);

            if (propertyInfo != null)
            {
                CreateComplexFieldConfiguration<TType>(propertyInfo);
            }
        }

        private void CreateComplexFieldConfiguration<TType>(PropertyInfo propertyInfo)
        {
            var mappingType = this.GetType().Assembly.GetTypes().FirstOrDefault(t => t.IsSubclassOf(typeof(ClassMap<TType>)));
            if (mappingType == null)
            {
                throw new FluentDynamoDbMappingException(string.Format("Could not find mapping for class of type {0}", propertyInfo.PropertyType));
            }

            var mapping = Activator.CreateInstance(mappingType) as ClassMap<TType>;
            if (mapping == null)
            {
                throw new FluentDynamoDbMappingException(string.Format("Could not create mapping for class of type {0}", propertyInfo.PropertyType));
            }

            var fieldConfiguration = new FieldConfiguration(propertyInfo.Name, propertyInfo.PropertyType, true);

            foreach (var innerFieldConfiguration in mapping.GetMappingConfigurationFields())
            {
                fieldConfiguration.FieldConfigurations.Add(innerFieldConfiguration);
            }

            _dynamoDbEntityConfiguration.AddFieldConfiguration(fieldConfiguration);
        }

        private static PropertyInfo GetPropertyInfo<TType>(Expression<Func<TEntity, TType>> propertyExpression)
        {
            MemberExpression memberExpression = null;

            if (propertyExpression.Body.NodeType == ExpressionType.Convert)
                memberExpression = ((UnaryExpression)propertyExpression.Body).Operand as MemberExpression;
            else if (propertyExpression.Body.NodeType == ExpressionType.MemberAccess)
                memberExpression = propertyExpression.Body as MemberExpression;

            if (memberExpression == null || memberExpression.Member == null)
                throw new ArgumentNullException("propertyExpression", "Not a member access!");

            var propertyInfo = (memberExpression.Member as PropertyInfo);
            return propertyInfo;
        }

        private static IFieldConfiguration CreateFieldConfigurationWith(PropertyInfo propertyInfo)
        {
            return new FieldConfiguration(propertyInfo.Name, propertyInfo.PropertyType);
        }

        public IEnumerable<IFieldConfiguration> GetMappingConfigurationFields()
        {
            return _dynamoDbEntityConfiguration.Fields;
        }

        public DynamoDbRootEntityConfiguration GetConfiguration()
        {
            return _dynamoDbRootEntityConfiguration;
        }
    }
}