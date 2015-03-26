using System.Collections.Generic;
using NUnit.Framework;

namespace FluentDynamoDb.Tests
{
    public class DynamoDbMapperWithObjectListBase
    {
        protected DynamoDbMapper<Foo> Mapper;

        public class Foo
        {
            public string FooName { get; set; }
            public IEnumerable<Bar> Bars { get; set; }
            public Other Other { get; set; }
        }

        public class Bar
        {
            public string BarName { get; set; }
        }

        public class Other
        {
            public string OtherName { get; set; }
        }

        [SetUp]
        public virtual void SetUp()
        {
            var configuration = new DynamoDbEntityConfiguration();

            configuration.AddFieldConfiguration(new FieldConfiguration { PropertyName = "FooName", Type = typeof(string) });

            configuration.AddFieldConfiguration(new FieldConfiguration
            {
                PropertyName = "Bars",
                Type = typeof(IEnumerable<Bar>),
                IsComplexType = true,
                FieldConfigurations = new List<IFieldConfiguration>
                        {
                            new FieldConfiguration { PropertyName = "BarName", Type = typeof(string) }
                        }
            });

            configuration.AddFieldConfiguration(new FieldConfiguration
            {
                PropertyName = "Other",
                Type = typeof(Other),
                IsComplexType = true,
                FieldConfigurations = new List<IFieldConfiguration>
                        {
                            new FieldConfiguration { PropertyName = "OtherName", Type = typeof(string) }        
                        }
            });

            Mapper = new DynamoDbMapper<Foo>(configuration);
        }
    }
}