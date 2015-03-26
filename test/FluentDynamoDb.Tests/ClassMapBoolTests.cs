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
        public void Map_WhenMappingFooName_AddFieldConfigurationShouldBeCalled()
        {
            DynamoDbEntityConfigurationFake.Verify(c => c.AddFieldConfiguration(It.IsAny<IFieldConfiguration>()), Times.Once);
        }

        [Test]
        public void Map_WhenMappingFooNameWithIsABoolean_ShouldCreateAFieldConfigurationAsExpected()
        {
            Assert.AreEqual("FooBool", CurrentFieldConfiguration.PropertyName);
            Assert.AreEqual(typeof(bool), CurrentFieldConfiguration.Type);
        }
    }
}