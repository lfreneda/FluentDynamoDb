using System.Collections.Generic;
using FluentDynamoDb.Mappers;
using NUnit.Framework;

namespace FluentDynamoDb.Tests.Mappers
{
    public class DynamoDbMapperWithObjectListUsingCamelCaseUnderscoreFieldBase
    {
        protected DynamoDbMapper<Foo> Mapper;

        [SetUp]
        public virtual void SetUp()
        {
            var configuration = new DynamoDbEntityConfiguration();

            configuration.AddFieldConfiguration(new FieldConfiguration("FooName", typeof(string)));

            configuration.AddFieldConfiguration(new FieldConfiguration("Bars", typeof(IEnumerable<Bar>), true, new List<FieldConfiguration> {
                    new FieldConfiguration("BarName", typeof (string))
                }, accessStrategy: AccessStrategy.CamelCaseUnderscoreName));

            configuration.AddFieldConfiguration(new FieldConfiguration("Other", typeof(Other), true, new List<FieldConfiguration> {
                    new FieldConfiguration("OtherName", typeof (string))
                }));

            Mapper = new DynamoDbMapper<Foo>(configuration);
        }

        public class Foo
        {
            public string FooName { get; set; }

            private readonly IList<Bar> _bars = new List<Bar>();
            public IEnumerable<Bar> Bars { get { return _bars; } }

            public void AddBar(Bar bar)
            {
                _bars.Add(bar);
            }

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
    }
}