using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace FluentDynamoDb
{
    public class DynamoDbConverterEnum<TEnum> : IPropertyConverter
    {
        public DynamoDBEntry ToEntry(object value)
        {
            if (value == null) return null;
            var valueString = Enum.GetName(value.GetType(), value);
            var entry = new Primitive(valueString);
            return entry;
        }

        public object FromEntry(DynamoDBEntry entry)
        {
            if (entry == null) return null;
            var valueString = entry.AsString();
            return (TEnum)Enum.Parse(typeof(TEnum), valueString);
        }
    }
}