using System.Collections.Generic;
using System.Linq;
using FluentDynamoDb.Exceptions;

namespace FluentDynamoDb.Mapping.Configuration
{
    public class DynamoDbEntityConfiguration
    {
        private readonly ICollection<FieldConfiguration> _fields = new List<FieldConfiguration>();
        public IEnumerable<FieldConfiguration> Fields { get { return _fields; } }

        public virtual void AddFieldConfiguration(FieldConfiguration fieldConfiguration)
        {
            if (Fields.Any(f => f.PropertyName == fieldConfiguration.PropertyName))
            {
                throw new FluentDynamoDbMappingException(string.Format("Property {0} has already been mapped", fieldConfiguration.PropertyName));
            }

            _fields.Add(fieldConfiguration);
        }
    }
}