using System.Collections.Generic;

namespace FluentDynamoDb
{
    public class DynamoDbEntityConfiguration
    {
        public IEnumerable<IFieldConfiguration> Fields { get; set; }
        public string TableName { get; set; }
    }
}