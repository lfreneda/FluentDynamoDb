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
                var mappingType = _dynamoDbRootEntityConfiguration.ClassMapAssembly.GetTypes().FirstOrDefault(t => t.IsSubclassOf(typeof(ClassMap<TType>)));
                if (mappingType == null)
                {
                    throw new FluentDynamoDbMappingException(string.Format("Could not find mapping for class of type {0}", propertyInfo.PropertyType));
                }

                ConstructorInfo ctor = mappingType.GetConstructor(new[] { typeof(DynamoDbRootEntityConfiguration) });

                var mapping = ctor.Invoke(new object[] { _dynamoDbRootEntityConfiguration }) as ClassMap<TType>;
                if (mapping == null)
                {
                    throw new FluentDynamoDbMappingException(string.Format("Could not create mapping for class of type {0}", propertyInfo.PropertyType));
                }

                var fieldConfiguration = new FieldConfiguration
                {
                    PropertyName = propertyInfo.Name,
                    Type = propertyInfo.PropertyType,
                    IsComplexType = true,
                };

                foreach (var innerFieldConfiguration in mapping.GetMappingConfiguration().Fields)
                {
                    fieldConfiguration.FieldConfigurations.Add(innerFieldConfiguration);
                }

                _dynamoDbEntityConfiguration.AddFieldConfiguration(fieldConfiguration);
            }
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
            return new FieldConfiguration
            {
                PropertyName = propertyInfo.Name,
                Type = propertyInfo.PropertyType
            };
        }

        public DynamoDbEntityConfiguration GetMappingConfiguration()
        {
            return _dynamoDbEntityConfiguration;
        }

        public DynamoDbRootEntityConfiguration GetConfiguration()
        {
            return _dynamoDbRootEntityConfiguration;
        }
    }
}