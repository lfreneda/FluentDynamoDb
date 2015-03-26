using System;
using Moq;
using NUnit.Framework;

namespace FluentDynamoDb.Tests
{
    [TestFixture]
    public class ClassMapDateTimeTests : ClassMapBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            FooMapInstance.Map(foo => foo.FooDate);
        }

        [Test]
        public void Map_WhenMappingFooName_AddFieldConfigurationShouldBeCalled()
        {
            DynamoDbEntityConfigurationFake.Verify(c => c.AddFieldConfiguration(It.IsAny<IFieldConfiguration>()), Times.Once);
        }

        [Test]
        public void Map_WhenMappingFooNameWithIsADateTime_ShouldCreateAFieldConfigurationAsExpected()
        {
            Assert.AreEqual("FooDate", CurrentFieldConfiguration.PropertyName);
            Assert.AreEqual(typeof(DateTime), CurrentFieldConfiguration.Type);
        }
    }
}