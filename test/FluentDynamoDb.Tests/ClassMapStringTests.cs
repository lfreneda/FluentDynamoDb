using Moq;
using NUnit.Framework;

namespace FluentDynamoDb.Tests
{
    [TestFixture]
    public class ClassMapStringTests : ClassMapBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            FooMapInstance.Map(foo => foo.FooString);
        }

        [Test]
        public void Map_WhenMappingFooName_AddFieldConfigurationShouldBeCalled()
        {
            DynamoDbMappingConfigurationFake.Verify(c => c.AddFieldConfiguration(It.IsAny<IFieldConfiguration>()), Times.Once);
        }

        [Test]
        public void Map_WhenMappingFooNameWithIsAString_ShouldCreateAFieldConfigurationAsExpected()
        {
            Assert.AreEqual("FooString", CurrentFieldConfiguration.PropertyName);
            Assert.AreEqual(typeof(string), CurrentFieldConfiguration.Type);
        }
    }
}