using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentDynamoDb.Configuration;
using FluentDynamoDb.Converters;
using FluentDynamoDb.Mappers;
using Moq;
using NUnit.Framework;

namespace FluentDynamoDb.Tests
{
    [TestFixture]
    public class ClassMapCollectionWithAccessStrategyTests : ClassMapBase
    {
        [SetUp]
        public override void SetUp()
        {
            FluentDynamoDbConfiguration.Configure(Assembly.GetExecutingAssembly());

            base.SetUp();
            var fooMap = new FooMap(DynamoDbRootEntityConfiguration, DynamoDbMappingConfigurationFake.Object);
        }

        [Test]
        public void Map_WhenMappingFooName_AddFieldConfigurationShouldBeCalled()
        {
            DynamoDbMappingConfigurationFake.Verify(c => c.AddFieldConfiguration(It.IsAny<FieldConfiguration>()),
                Times.Once);
        }

        [Test]
        public void Map_WhenMappingOptionWhichIsAEnum_ShouldCreateAFieldConfigurationAsExpected()
        {
            Assert.AreEqual("Bars", CurrentFieldConfiguration.PropertyName);
            Assert.AreEqual(typeof(IEnumerable<Bar>), CurrentFieldConfiguration.Type);
            Assert.AreEqual(AccessStrategy.CamelCaseUnderscoreName, CurrentFieldConfiguration.AccessStrategy);
            Assert.AreEqual(true, CurrentFieldConfiguration.IsComplexType);
            Assert.AreEqual(1, CurrentFieldConfiguration.FieldConfigurations.Count);
            Assert.AreEqual("BarName", CurrentFieldConfiguration.FieldConfigurations.ElementAt(0).PropertyName);
            Assert.AreEqual(typeof(string), CurrentFieldConfiguration.FieldConfigurations.ElementAt(0).Type);

            //TODO: Split tests :(
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
                HasMany(c => c.Bars).With(AccessStrategy.CamelCaseUnderscoreName);
            }
        }
    }
}