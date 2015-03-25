using System.Collections.Generic;

namespace FluentDynamoDb.Tests
{
    public class DynamoDbMapperWithComplexClassBase
    {
        protected readonly DynamoDbMapper<Foo> Mapper;

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

        public DynamoDbMapperWithComplexClassBase()
        {
            var configuration = new DynamoDbEntityConfiguration
            {
                Fields = new List<IFieldConfiguration>
                {
                    new FieldConfiguration { PropertyName = "FooName", Type = typeof(string) },
                    new FieldConfiguration { PropertyName = "Bar", Type = typeof(Bar), IsComplexType = true, FieldConfigurations = new List<FieldConfiguration>
                        {
                            new FieldConfiguration { PropertyName = "BarName", Type = typeof(string) },
                            new FieldConfiguration { PropertyName = "Other", Type = typeof(Other), IsComplexType = true, FieldConfigurations = new List<FieldConfiguration>
                            {
                                new FieldConfiguration { PropertyName = "OtherName", Type = typeof(string)}
                            }}
                        }
                    },
                }
            };

            Mapper = new DynamoDbMapper<Foo>(configuration);
        }
    }
}