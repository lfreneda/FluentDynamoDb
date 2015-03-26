using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

namespace FluentDynamoDb
{
    public class FieldConfiguration : IFieldConfiguration
    {
        public FieldConfiguration(string propertyName, Type type, bool isComplexType = false,
            ICollection<IFieldConfiguration> fieldConfigurations = null,
            IPropertyConverter propertyConverter = null)
        {
            Type = type;
            PropertyName = propertyName;
            IsComplexType = isComplexType;
            FieldConfigurations = fieldConfigurations ?? new List<IFieldConfiguration>();
            PropertyConverter = propertyConverter;
        }

        public Type Type { get; set; }
        public string PropertyName { get; set; }
        public bool IsComplexType { get; set; }
        public ICollection<IFieldConfiguration> FieldConfigurations { get; set; }
        public IPropertyConverter PropertyConverter { get; set; }
    }
}