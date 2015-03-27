using FluentDynamoDb.Converters;
using FluentDynamoDb.Mappers;
using Moq;
using NUnit.Framework;

namespace FluentDynamoDb.Tests
{
    [TestFixture]
    public class ClassMapEnumWithPropertyConversorTests : ClassMapBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            var fooMap = new FooMap(DynamoDbRootEntityConfiguration, DynamoDbMappingConfigurationFake.Object);
        }

        [Test]
        public void Map_WhenMappingFooName_AddFieldConfigurationShouldBeCalled()
        {
            DynamoDbMappingConfigurationFake.Verify(c => c.AddFieldConfiguration(It.IsAny<FieldConfiguration>()),
                Times.Once);
        }

        [Test]
        public void Map_WhenMappingOptionWhichIsAEnum_ShouldCreateAFieldConfigurationAsExpected()
        {
            Assert.AreEqual("Option", CurrentFieldConfiguration.PropertyName);
            Assert.AreEqual(typeof(FooOption), CurrentFieldConfiguration.Type);
            Assert.IsInstanceOf<DynamoDbConverterEnum<FooOption>>(CurrentFieldConfiguration.PropertyConverter);
        }

        public class Foo
        {
            public FooOption Option { get; set; }
        }

        public enum FooOption
        {
            Option1 = 1,
            Option2 = 2
        }

        public class FooMap : ClassMap<Foo>
        {
            public FooMap(DynamoDbRootEntityConfiguration dynamoDbRootEntityConfiguration,
                DynamoDbEntityConfiguration dynamoDbEntityConfiguration)
                : base(dynamoDbRootEntityConfiguration, dynamoDbEntityConfiguration)
            {
                Map(c => c.Option).With(new DynamoDbConverterEnum<FooOption>());
            }
        }
    }
}