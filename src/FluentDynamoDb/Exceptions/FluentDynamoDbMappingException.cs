using System;

namespace FluentDynamoDb.Exceptions
{
    public class FluentDynamoDbMappingException : ApplicationException
    {
        public FluentDynamoDbMappingException(string message)
            : base(message)
        {
        }

        public FluentDynamoDbMappingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}