using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentDynamoDb
{
    public class ClassMap<TEntity>
    {
        private readonly DynamoDbEntityConfiguration _dynamoDbEntityConfiguration;
        private readonly DynamoDbMappingConfiguration _dynamoDbMappingConfiguration;

        public ClassMap()
            : this(new DynamoDbEntityConfiguration(), new DynamoDbMappingConfiguration())
        {

        }

        public ClassMap(DynamoDbEntityConfiguration dynamoDbEntityConfiguration)
            : this(dynamoDbEntityConfiguration, new DynamoDbMappingConfiguration())
        {

        }

        internal ClassMap(DynamoDbEntityConfiguration dynamoDbEntityConfiguration, DynamoDbMappingConfiguration dynamoDbMappingConfiguration)
        {
            _dynamoDbEntityConfiguration = dynamoDbEntityConfiguration;
            _dynamoDbMappingConfiguration = dynamoDbMappingConfiguration;
        }

        protected void Map<TType>(Expression<Func<TEntity, TType>> propertyExpression)
        {
            var propertyInfo = GetPropertyInfo(propertyExpression);

            if (propertyInfo != null)
            {
                _dynamoDbMappingConfiguration.AddFieldConfiguration(CreateFieldConfigurationWith(propertyInfo));
            }
        }

        protected void References<TType>(Expression<Func<TEntity, TType>> propertyExpression)
        {
            var propertyInfo = GetPropertyInfo(propertyExpression);

            if (propertyInfo != null)
            {
                var mappingType = _dynamoDbEntityConfiguration.ClassMapAssembly.GetTypes().FirstOrDefault(t => t.IsSubclassOf(typeof(ClassMap<TType>)));
                if (mappingType == null)
                {
                    throw new FluentDynamoDbMappingException(string.Format("Could not find mapping for class of type {0}", propertyInfo.PropertyType));
                }

                ConstructorInfo ctor = mappingType.GetConstructor(new[] { typeof(DynamoDbEntityConfiguration) });

                var mapping = ctor.Invoke(new object[] { _dynamoDbEntityConfiguration }) as ClassMap<TType>;
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

                _dynamoDbMappingConfiguration.AddFieldConfiguration(fieldConfiguration);
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

        public DynamoDbMappingConfiguration GetMappingConfiguration()
        {
            return _dynamoDbMappingConfiguration;
        }

        public DynamoDbEntityConfiguration GetConfiguration()
        {
            return _dynamoDbEntityConfiguration;
        }
    }
}