using System;
using System.Collections.Generic;

namespace FluentDynamoDb
{
    public interface IFieldConfiguration
    {
        Type Type { get; }
        string PropertyName { get; }
        bool IsComplexType { get; }
        ICollection<FieldConfiguration> FieldConfigurations { get; }
    }

    public class FieldConfiguration : IFieldConfiguration
    {
        public Type Type { get; set; }
        public string PropertyName { get; set; }
        public bool IsComplexType { get; set; }
        public ICollection<FieldConfiguration> FieldConfigurations { get; set; }
    }
}