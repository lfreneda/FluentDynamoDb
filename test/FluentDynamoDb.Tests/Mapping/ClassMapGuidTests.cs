using System;
using FluentDynamoDb.Mapping;
using FluentDynamoDb.Mapping.Configuration;
using Moq;
using NUnit.Framework;

namespace FluentDynamoDb.Tests.Mapping
{
    [TestFixture]
    public class ClassMapGuidTests : ClassMapBase
    {
        public class Foo
        {
            public Guid FooGuid { get; set; }
        }

        public class FooMap : ClassMap<Foo>
        {
            public FooMap(DynamoDbRootEntityConfiguration dynamoDbRootEntityConfiguration, DynamoDbEntityConfiguration dynamoDbEntityConfiguration)
                : base(dynamoDbRootEntityConfiguration, dynamoDbEntityConfiguration)
            {
                Map(c => c.FooGuid);
            }
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            var fooMap = new FooMap(DynamoDbRootEntityConfiguration, DynamoDbMappingConfigurationFake.Object);
        }

        [Test]
        public void Map_WhenMappingFooGuid_AddFieldConfigurationShouldBeCalled()
        {
            DynamoDbMappingConfigurationFake.Verify(c => c.AddFieldConfiguration(It.IsAny<FieldConfiguration>()), Times.Once);
        }

        [Test]
        public void Map_WhenMappingFooGuidWithIsAGuid_ShouldCreateAFieldConfigurationAsExpected()
        {
            Assert.AreEqual("FooGuid", CurrentFieldConfiguration.PropertyName);
            Assert.AreEqual(typeof(Guid), CurrentFieldConfiguration.Type);
        }
    }
}