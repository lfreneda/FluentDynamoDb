using FluentDynamoDb.Mapping;
using FluentDynamoDb.Mapping.Configuration;
using Moq;
using NUnit.Framework;

namespace FluentDynamoDb.Tests.Mapping
{
    [TestFixture]
    public class ClassMapBoolTests : ClassMapBase
    {
        public class Foo
        {
            public bool FooBool { get; set; }
        }

        public class FooMap : ClassMap<Foo>
        {
            public FooMap(DynamoDbRootEntityConfiguration dynamoDbRootEntityConfiguration, DynamoDbEntityConfiguration dynamoDbEntityConfiguration)
                : base(dynamoDbRootEntityConfiguration, dynamoDbEntityConfiguration)
            {
                Map(c => c.FooBool);
            }
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            var fooMap = new FooMap(DynamoDbRootEntityConfiguration, DynamoDbMappingConfigurationFake.Object);
        }

        [Test]
        public void Map_WhenMappingFooBool_AddFieldConfigurationShouldBeCalled()
        {
            DynamoDbMappingConfigurationFake.Verify(c => c.AddFieldConfiguration(It.IsAny<FieldConfiguration>()), Times.Once);
        }

        [Test]
        public void Map_WhenMappingFooBoolWithIsABoolean_ShouldCreateAFieldConfigurationAsExpected()
        {
            Assert.AreEqual("FooBool", CurrentFieldConfiguration.PropertyName);
            Assert.AreEqual(typeof(bool), CurrentFieldConfiguration.Type);
        }
    }
}