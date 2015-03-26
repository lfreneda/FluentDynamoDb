using Moq;
using NUnit.Framework;

namespace FluentDynamoDb.Tests
{
    [TestFixture]
    public class ClassMapDecimalTests : ClassMapBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            FooMapInstance.Map(foo => foo.FooDecimal);
        }

        [Test]
        public void Map_WhenMappingFooName_AddFieldConfigurationShouldBeCalled()
        {
            DynamoDbEntityConfigurationFake.Verify(c => c.AddFieldConfiguration(It.IsAny<IFieldConfiguration>()), Times.Once);
        }

        [Test]
        public void Map_WhenMappingFooNameWithIsADecimal_ShouldCreateAFieldConfigurationAsExpected()
        {
            Assert.AreEqual("FooDecimal", CurrentFieldConfiguration.PropertyName);
            Assert.AreEqual(typeof(decimal), CurrentFieldConfiguration.Type);
        }
    }
}