using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace FluentDynamoDb.Tests
{
    [TestFixture]
    public class ClassMapListOfComplexTests : ClassMapBase
    {
        public class Foo
        {
            public IEnumerable<Bar> Bars { get; set; }
        }

        public class Bar
        {
            public string BarName { get; set; }
        }

        public class BarMap : ClassMap<Bar>
        {
            public BarMap()
            {
                Map(c => c.BarName);
            }
        }

        public class FooMap : ClassMap<Foo>
        {
            public FooMap(DynamoDbRootEntityConfiguration dynamoDbRootEntityConfiguration, DynamoDbEntityConfiguration dynamoDbEntityConfiguration)
                : base(dynamoDbRootEntityConfiguration, dynamoDbEntityConfiguration)
            {
                HasMany(f => f.Bars);
            }
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var fooMap = new FooMap(DynamoDbRootEntityConfiguration, DynamoDbMappingConfigurationFake.Object);
        }

        [Test]
        public void Map_WhenMappingBarsWithIsAListOfComplexType_ShouldCreateAFieldConfigurationAsExpected()
        {
            Assert.AreEqual("Bars", CurrentFieldConfiguration.PropertyName);
            Assert.AreEqual(typeof(IEnumerable<Bar>), CurrentFieldConfiguration.Type);
            Assert.AreEqual(true, CurrentFieldConfiguration.IsComplexType);
            Assert.AreEqual(1, CurrentFieldConfiguration.FieldConfigurations.Count);
            Assert.AreEqual("BarName", CurrentFieldConfiguration.FieldConfigurations.ElementAt(0).PropertyName);
            Assert.AreEqual(typeof(string), CurrentFieldConfiguration.FieldConfigurations.ElementAt(0).Type);
        }
    }
}