using System.Collections.Generic;
using System.Linq;

namespace FluentDynamoDb
{
    public class DynamoDbEntityConfiguration
    {
        public string TableName { get; set; }

        private readonly ICollection<IFieldConfiguration> _fields = new List<IFieldConfiguration>();
        public IEnumerable<IFieldConfiguration> Fields { get { return _fields; } }

        public virtual void AddFieldConfiguration(IFieldConfiguration fieldConfiguration)
        {
            if (Fields.Any(f => f.PropertyName == fieldConfiguration.PropertyName))
            {
                throw new FluentDynamoDbMappingException(string.Format("Property {0} has already been mapped", fieldConfiguration.PropertyName));
            }

            _fields.Add(fieldConfiguration);
        }
    }
}