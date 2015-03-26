using System;
using Moq;
using NUnit.Framework;

namespace FluentDynamoDb.Tests
{
    [TestFixture]
    public class ClassMapDateTimeTests : ClassMapBase
    {
        public class Foo
        {
            public DateTime FooDate { get; set; }
        }

        public class FooMap : ClassMap<Foo>
        {
            public FooMap(DynamoDbRootEntityConfiguration dynamoDbRootEntityConfiguration, DynamoDbEntityConfiguration dynamoDbEntityConfiguration)
                : base(dynamoDbRootEntityConfiguration, dynamoDbEntityConfiguration)
            {
                Map(c => c.FooDate);
            }
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            var fooMap = new FooMap(DynamoDbRootEntityConfiguration, DynamoDbMappingConfigurationFake.Object);
        }

        [Test]
        public void Map_WhenMappingFooDate_AddFieldConfigurationShouldBeCalled()
        {
            DynamoDbMappingConfigurationFake.Verify(c => c.AddFieldConfiguration(It.IsAny<IFieldConfiguration>()), Times.Once);
        }

        [Test]
        public void Map_WhenMappingFooDateWithIsADateTime_ShouldCreateAFieldConfigurationAsExpected()
        {
            Assert.AreEqual("FooDate", CurrentFieldConfiguration.PropertyName);
            Assert.AreEqual(typeof(DateTime), CurrentFieldConfiguration.Type);
        }
    }
}