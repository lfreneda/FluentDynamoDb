using FluentDynamoDb.Mapping;
using FluentDynamoDb.Mapping.Configuration;
using Moq;
using NUnit.Framework;

namespace FluentDynamoDb.Tests.Mapping
{
    [TestFixture]
    public class ClassMapDecimalTests : ClassMapBase
    {
        public class Foo
        {
            public decimal FooDecimal { get; set; }
        }

        public class FooMap : ClassMap<Foo>
        {
            public FooMap(DynamoDbRootEntityConfiguration dynamoDbRootEntityConfiguration, DynamoDbEntityConfiguration dynamoDbEntityConfiguration)
                : base(dynamoDbRootEntityConfiguration, dynamoDbEntityConfiguration)
            {
                Map(c => c.FooDecimal);
            }
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            var fooMap = new FooMap(DynamoDbRootEntityConfiguration, DynamoDbMappingConfigurationFake.Object);
        }

        [Test]
        public void Map_WhenMappingFooName_AddFieldConfigurationShouldBeCalled()
        {
            DynamoDbMappingConfigurationFake.Verify(c => c.AddFieldConfiguration(It.IsAny<FieldConfiguration>()), Times.Once);
        }

        [Test]
        public void Map_WhenMappingFooDecimalWithIsADecimal_ShouldCreateAFieldConfigurationAsExpected()
        {
            Assert.AreEqual("FooDecimal", CurrentFieldConfiguration.PropertyName);
            Assert.AreEqual(typeof(decimal), CurrentFieldConfiguration.Type);
        }
    }
}