using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

namespace FluentDynamoDb
{
    public interface IFieldConfiguration
    {
        Type Type { get; }
        string PropertyName { get; }
        bool IsComplexType { get; }
        ICollection<FieldConfiguration> FieldConfigurations { get; }
        IPropertyConverter PropertyConverter { get; }
    }
}