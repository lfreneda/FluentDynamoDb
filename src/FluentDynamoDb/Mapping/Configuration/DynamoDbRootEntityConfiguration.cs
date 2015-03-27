namespace FluentDynamoDb.Mapping.Configuration
{
    public class DynamoDbRootEntityConfiguration
    {
        public string TableName { get; set; }
        public DynamoDbEntityConfiguration DynamoDbEntityConfiguration { get; set; }
    }
}