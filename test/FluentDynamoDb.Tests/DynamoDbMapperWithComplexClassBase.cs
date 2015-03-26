using System.Collections.Generic;
using NUnit.Framework;

namespace FluentDynamoDb.Tests
{
    public class DynamoDbMapperWithComplexClassBase
    {
        protected DynamoDbMapper<Foo> Mapper;

        public class Foo
        {
            public string FooName { get; set; }
            public Bar Bar { get; set; }
        }

        public class Bar
        {
            public string BarName { get; set; }
            public Other Other { get; set; }
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
                PropertyName = "Bar",
                Type = typeof(Bar),
                IsComplexType = true,
                FieldConfigurations = new List<FieldConfiguration>
                        {
                            new FieldConfiguration { PropertyName = "BarName", Type = typeof(string) },
                            new FieldConfiguration { PropertyName = "Other", Type = typeof(Other), IsComplexType = true, FieldConfigurations = new List<FieldConfiguration>
                            {
                                new FieldConfiguration { PropertyName = "OtherName", Type = typeof(string)}
                            }}
                        }
            });

            Mapper = new DynamoDbMapper<Foo>(configuration);
        }
    }
}