using Moq;
using NUnit.Framework;

namespace FluentDynamoDb.Tests
{
    [TestFixture]
    public class ClassMapBoolTests : ClassMapBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            FooMapInstance.Map(foo => foo.FooBool);
        }

        [Test]
        public void Map_WhenMappingFooBool_AddFieldConfigurationShouldBeCalled()
        {
            DynamoDbMappingConfigurationFake.Verify(c => c.AddFieldConfiguration(It.IsAny<IFieldConfiguration>()), Times.Once);
        }

        [Test]
        public void Map_WhenMappingFooBoolWithIsABoolean_ShouldCreateAFieldConfigurationAsExpected()
        {
            Assert.AreEqual("FooBool", CurrentFieldConfiguration.PropertyName);
            Assert.AreEqual(typeof(bool), CurrentFieldConfiguration.Type);
        }
    }
}