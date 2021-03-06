using System;
using FluentDynamoDb.Mappers;
using Moq;
using NUnit.Framework;

namespace FluentDynamoDb.Tests
{
    [TestFixture]
    public class ClassMapGuidTests : ClassMapBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            var fooMap = new FooMap(DynamoDbRootEntityConfiguration, DynamoDbMappingConfigurationFake.Object);
        }

        [Test]
        public void Map_WhenMappingFooGuid_AddFieldConfigurationShouldBeCalled()
        {
            DynamoDbMappingConfigurationFake.Verify(c => c.AddFieldConfiguration(It.IsAny<FieldConfiguration>()),
                Times.Once);
        }

        [Test]
        public void Map_WhenMappingFooGuidWithIsAGuid_ShouldCreateAFieldConfigurationAsExpected()
        {
            Assert.AreEqual("FooGuid", CurrentFieldConfiguration.PropertyName);
            Assert.AreEqual(typeof (Guid), CurrentFieldConfiguration.Type);
        }

        public class Foo
        {
            public Guid FooGuid { get; set; }
        }

        public class FooMap : ClassMap<Foo>
        {
            public FooMap(DynamoDbRootEntityConfiguration dynamoDbRootEntityConfiguration,
                DynamoDbEntityConfiguration dynamoDbEntityConfiguration)
                : base(dynamoDbRootEntityConfiguration, dynamoDbEntityConfiguration)
            {
                Map(c => c.FooGuid);
            }
        }
    }
}