using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DocumentModel;

namespace FluentDynamoDb
{
    public class DynamoDbMapper<TEntity> where TEntity : class,new()
    {
        private readonly DynamoDbEntityConfiguration _configuration;

        public DynamoDbMapper(DynamoDbEntityConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Document ToDocument(TEntity entity)
        {
            return ToDocument(entity, _configuration.Fields);
        }

        private Document ToDocument(object entity, IEnumerable<IFieldConfiguration> fields)
        {
            var document = new Document();

            foreach (var field in fields)
            {
                if (field.IsComplexType)
                {
                    var value = GetPropertyValue(entity, field);
                    if (value == null) continue;

                    if (IsEnumerable(field))
                    {
                        document[field.PropertyName] = CreateDocumentList(value, field.FieldConfigurations);
                        continue;
                    }

                    var innerDocument = ToDocument(value, field.FieldConfigurations);
                    document[field.PropertyName] = innerDocument;
                }
                else
                {
                    dynamic value = GetPropertyValue(entity, field);
                    document[field.PropertyName] = value;
                }
            }

            return document;
        }

        private static object GetPropertyValue(object entity, IFieldConfiguration field)
        {
            return entity.GetType().GetProperty(field.PropertyName).GetValue(entity, null);
        }

        private List<Document> CreateDocumentList(object value, IEnumerable<IFieldConfiguration> configuration)
        {
            return (from object item in (IEnumerable)value select ToDocument(item, configuration)).ToList();
        }

        private static bool IsEnumerable(IFieldConfiguration field)
        {
            return field.Type.IsGenericType && field.Type.GetGenericTypeDefinition() == typeof(IEnumerable<>);
        }

        public TEntity ToEntity(Document document)
        {
            if (document == null) return null;
            return (TEntity)ToEntity(document, _configuration.Fields, typeof(TEntity));
        }

        private object ToEntity(Document document, IEnumerable<IFieldConfiguration> fields, Type type)
        {
            if (document == null) return null;

            var entity = Activator.CreateInstance(type);

            foreach (var field in fields)
            {
                if (field.IsComplexType)
                {
                    if (!document.ContainsKey(field.PropertyName)) continue;

                    var propertyValue = document[field.PropertyName];
                    if (propertyValue == null) continue;

                    if (IsEnumerable(field))
                    {
                        var itemType = field.Type.GetGenericArguments()[0];
                        var listType = typeof(List<>);
                        var constructedListType = listType.MakeGenericType(itemType);
                        var list = (IList)Activator.CreateInstance(constructedListType);

                        foreach (var dbEntry in propertyValue.AsListOfDocument())
                        {
                            var itemDocument = dbEntry.AsDocument();
                            var itemValue = ToEntity(itemDocument, field.FieldConfigurations, itemType);
                            list.Add(itemValue);
                        }

                        entity.GetType().GetProperty(field.PropertyName).SetValue(entity, list);

                        continue;
                    }

                    var innerDocument = propertyValue.AsDocument();
                    var value = ToEntity(innerDocument, field.FieldConfigurations, field.Type);
                    entity.GetType().GetProperty(field.PropertyName).SetValue(entity, value);
                }
                else
                {
                    dynamic value = MappingType[field.Type](document[field.PropertyName]);
                    entity.GetType().GetProperty(field.PropertyName).SetValue(entity, value);
                }
            }

            return entity;
        }

        protected Dictionary<Type, Func<DynamoDBEntry, dynamic>> MappingType = new Dictionary<Type, Func<DynamoDBEntry, dynamic>>
        {
            { typeof (string), value => value.AsString() },
            { typeof (Guid), value => value.AsGuid() },
            { typeof (decimal), value => value.AsDecimal() },
            { typeof (bool), value => value.AsBoolean() },
            { typeof (DateTime), value => value.AsDateTime() },
            { typeof (IEnumerable<string>), value => value.AsListOfString() }
        };
    }
}
