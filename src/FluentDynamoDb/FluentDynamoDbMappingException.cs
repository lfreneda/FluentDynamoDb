using System;

namespace FluentDynamoDb
{
    public class FluentDynamoDbMappingException : ApplicationException
    {
        public FluentDynamoDbMappingException(string message)
            : base(message)
        {
        }
    }
}