using System;
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

            FooMapInstance.Map(foo => foo.FooGuid);
        }

        [Test]
        public void Map_WhenMappingFooName_AddFieldConfigurationShouldBeCalled()
        {
            DynamoDbEntityConfigurationFake.Verify(c => c.AddFieldConfiguration(It.IsAny<IFieldConfiguration>()), Times.Once);
        }

        [Test]
        public void Map_WhenMappingFooNameWithIsAGuid_ShouldCreateAFieldConfigurationAsExpected()
        {
            Assert.AreEqual("FooGuid", CurrentFieldConfiguration.PropertyName);
            Assert.AreEqual(typeof(Guid), CurrentFieldConfiguration.Type);
        }
    }
}