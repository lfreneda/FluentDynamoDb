using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Amazon.DynamoDBv2.DataModel;
using FluentDynamoDb.Exceptions;
using FluentDynamoDb.Mappers;

namespace FluentDynamoDb
{
    public class ClassMap<TEntity>
    {
        private readonly DynamoDbEntityConfiguration _dynamoDbEntityConfiguration;
        private readonly DynamoDbRootEntityConfiguration _dynamoDbRootEntityConfiguration;

        public ClassMap()
            : this(new DynamoDbRootEntityConfiguration(), new DynamoDbEntityConfiguration())
        {
        }

        internal ClassMap(DynamoDbRootEntityConfiguration dynamoDbRootEntityConfiguration)
            : this(dynamoDbRootEntityConfiguration, new DynamoDbEntityConfiguration())
        {
        }

        internal ClassMap(DynamoDbRootEntityConfiguration dynamoDbRootEntityConfiguration,
            DynamoDbEntityConfiguration dynamoDbEntityConfiguration)
        {
            _dynamoDbRootEntityConfiguration = dynamoDbRootEntityConfiguration;
            _dynamoDbEntityConfiguration = dynamoDbEntityConfiguration;
        }

        protected void TableName(string tableName)
        {
            _dynamoDbRootEntityConfiguration.TableName = tableName;
        }

        protected void Map<TType>(Expression<Func<TEntity, TType>> propertyExpression,
            IPropertyConverter propertyConverter = null)
        {
            var propertyInfo = GetPropertyInfo(propertyExpression);

            if (propertyInfo != null)
            {
                var fieldConfiguration = new FieldConfiguration(propertyInfo.Name, propertyInfo.PropertyType, false,
                    propertyConverter: propertyConverter);
                _dynamoDbEntityConfiguration.AddFieldConfiguration(fieldConfiguration);
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
            var mappingType = GetType()
                .Assembly.GetTypes()
                .FirstOrDefault(t => t.IsSubclassOf(typeof (ClassMap<TType>)));
            if (mappingType == null)
            {
                throw new FluentDynamoDbMappingException(string.Format("Could not find mapping for class of type {0}",
                    propertyInfo.PropertyType));
            }

            ClassMap<TType> mapping = null;

            try
            {
                 mapping = Activator.CreateInstance(mappingType) as ClassMap<TType>;
                 if (mapping == null)
                 {
                     throw new FluentDynamoDbMappingException(string.Format("Could not create a instance of type {0}, class must provide a public constructor", mappingType));
                 }
            }
            catch (MissingMethodException ex)
            {
                throw new FluentDynamoDbMappingException(string.Format("Could not create a instance of type {0}, class must provide a public constructor", mappingType), ex);
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
                memberExpression = ((UnaryExpression) propertyExpression.Body).Operand as MemberExpression;
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