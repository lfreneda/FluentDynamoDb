using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

namespace FluentDynamoDb.Mapping.Configuration
{
    public class FieldConfiguration 
    {
        public FieldConfiguration(string propertyName, Type type, bool isComplexType = false,
            ICollection<FieldConfiguration> fieldConfigurations = null,
            IPropertyConverter propertyConverter = null)
        {
            Type = type;
            PropertyName = propertyName;
            IsComplexType = isComplexType;
            FieldConfigurations = fieldConfigurations ?? new List<FieldConfiguration>();
            PropertyConverter = propertyConverter;
        }

        public Type Type { get; set; }
        public string PropertyName { get; set; }
        public bool IsComplexType { get; set; }
        public ICollection<FieldConfiguration> FieldConfigurations { get; set; }
        public IPropertyConverter PropertyConverter { get; set; }
    }
}