using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Amazon.DynamoDBv2.DataModel;
using FluentDynamoDb.Mappers;

namespace FluentDynamoDb
{
    public class ClassMap<TEntity>
    {
        private readonly IClassMapLoader _classMapLoader;
        private readonly DynamoDbEntityConfiguration _dynamoDbEntityConfiguration;
        private readonly DynamoDbRootEntityConfiguration _dynamoDbRootEntityConfiguration;

        public ClassMap()
            : this(new DynamoDbRootEntityConfiguration(), new DynamoDbEntityConfiguration(), new ClassMapLoader())
        {
        }

        internal ClassMap(DynamoDbRootEntityConfiguration dynamoDbRootEntityConfiguration)
            : this(dynamoDbRootEntityConfiguration, new DynamoDbEntityConfiguration(), new ClassMapLoader())
        {
        }

        internal ClassMap(DynamoDbRootEntityConfiguration dynamoDbRootEntityConfiguration,
            DynamoDbEntityConfiguration dynamoDbEntityConfiguration)
            : this(dynamoDbRootEntityConfiguration, dynamoDbEntityConfiguration, new ClassMapLoader())
        {
        }

        internal ClassMap(DynamoDbRootEntityConfiguration dynamoDbRootEntityConfiguration,
            DynamoDbEntityConfiguration dynamoDbEntityConfiguration, IClassMapLoader classMapLoader)
        {
            _dynamoDbRootEntityConfiguration = dynamoDbRootEntityConfiguration;
            _dynamoDbEntityConfiguration = dynamoDbEntityConfiguration;
            _classMapLoader = classMapLoader;
        }

        protected void TableName(string tableName)
        {
            _dynamoDbRootEntityConfiguration.TableName = tableName;
        }

        protected FieldConfiguration Map<TType>(Expression<Func<TEntity, TType>> propertyExpression, IPropertyConverter propertyConverter = null)
        {
            var propertyInfo = GetPropertyInfo(propertyExpression);

            if (propertyInfo != null)
            {
                var fieldConfiguration = new FieldConfiguration(propertyInfo.Name, propertyInfo.PropertyType, false, propertyConverter: propertyConverter);
                _dynamoDbEntityConfiguration.AddFieldConfiguration(fieldConfiguration);
                return fieldConfiguration;
            }

            return null; //TODO: Throws exception
        }

        protected void References<TType>(Expression<Func<TEntity, TType>> propertyExpression)
        {
            var propertyInfo = GetPropertyInfo(propertyExpression);

            if (propertyInfo != null)
            {
                CreateComplexFieldConfiguration<TType>(propertyInfo);
            }
        }

        protected FieldConfiguration HasMany<TType>(Expression<Func<TEntity, IEnumerable<TType>>> propertyExpression)
        {
            var propertyInfo = GetPropertyInfo(propertyExpression);

            if (propertyInfo != null)
            {
                return CreateComplexFieldConfiguration<TType>(propertyInfo);
            }

            return null; //TODO: Throws exception
        }

        private FieldConfiguration CreateComplexFieldConfiguration<TType>(PropertyInfo propertyInfo)
        {
            var classMap = _classMapLoader.Load<TType>();

            var fieldConfiguration = new FieldConfiguration(propertyInfo.Name, propertyInfo.PropertyType, true);

            foreach (var innerFieldConfiguration in classMap.GetMappingConfigurationFields())
            {
                fieldConfiguration.FieldConfigurations.Add(innerFieldConfiguration);
            }

            _dynamoDbEntityConfiguration.AddFieldConfiguration(fieldConfiguration);

            return fieldConfiguration;
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

        internal IEnumerable<FieldConfiguration> GetMappingConfigurationFields()
        {
            return _dynamoDbEntityConfiguration.Fields;
        }

        internal DynamoDbEntityConfiguration GetEntityConfiguration()
        {
            return _dynamoDbEntityConfiguration;
        }

        internal DynamoDbRootEntityConfiguration GetRootConfiguration()
        {
            _dynamoDbRootEntityConfiguration.DynamoDbEntityConfiguration = GetEntityConfiguration();
            return _dynamoDbRootEntityConfiguration;
        }
    }
}