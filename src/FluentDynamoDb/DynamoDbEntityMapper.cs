using System;
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
            return ToDocumentInt(entity, _configuration.Fields);
        }

        public Document ToDocumentInt(object entity, IEnumerable<IFieldConfiguration> fields)
        {
            var document = new Document();

            foreach (var field in fields)
            {
                if (field.IsComplexType)
                {
                    var value = entity.GetType().GetProperty(field.PropertyName).GetValue(entity, null);
                    var innerDocument = ToDocumentInt(value, field.FieldConfigurations);
                    document[field.PropertyName] = innerDocument;
                }
                else
                {
                    dynamic value = entity.GetType().GetProperty(field.PropertyName).GetValue(entity, null);
                    document[field.PropertyName] = value;
                }
            }

            return document;
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
                    var innerDocument = document[field.PropertyName].AsDocument();
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
            { typeof (List<string>), value => value.AsListOfString() }
        };
    }
}
