using Moq;
using NUnit.Framework;

namespace FluentDynamoDb.Tests
{
    [TestFixture]
    public class ClassMapIntTests : ClassMapBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            FooMapInstance.Map(foo => foo.FooInt);
        }

        [Test]
        public void Map_WhenMappingFooInt_AddFieldConfigurationShouldBeCalled()
        {
            DynamoDbMappingConfigurationFake.Verify(c => c.AddFieldConfiguration(It.IsAny<IFieldConfiguration>()), Times.Once);
        }

        [Test]
        public void Map_WhenMappingFooIntWithIsAInteger_ShouldCreateAFieldConfigurationAsExpected()
        {
            Assert.AreEqual("FooInt", CurrentFieldConfiguration.PropertyName);
            Assert.AreEqual(typeof(int), CurrentFieldConfiguration.Type);
        }
    }
}