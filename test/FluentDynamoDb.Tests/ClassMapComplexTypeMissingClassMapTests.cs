using System.Linq;
using FluentDynamoDb.Exceptions;
using FluentDynamoDb.Mappers;
using NUnit.Framework;

namespace FluentDynamoDb.Tests
{
    [TestFixture]
    public class ClassMapComplexTypeMissingClassMapTests : ClassMapBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [Test]
        public void Map_WhenMappingBarWithIsAComplexTypeButNotProvidingAClassMap_ShouldThrowsException()
        {
            Assert.That(() => new FooMap(DynamoDbRootEntityConfiguration, DynamoDbMappingConfigurationFake.Object),
                Throws.Exception
                      .TypeOf<FluentDynamoDbMappingException>()
                      .With
                      .Message
                      .EqualTo("Could not find mapping for class of type FluentDynamoDb.Tests.ClassMapComplexTypeMissingClassMapTests+Bar"));
        }

        public class Foo
        {
            public Bar Bar { get; set; }
        }

        public class Bar
        {
            public string BarName { get; set; }
        }

        public class FooMap : ClassMap<Foo>
        {
            public FooMap(DynamoDbRootEntityConfiguration dynamoDbRootEntityConfiguration,
                DynamoDbEntityConfiguration dynamoDbEntityConfiguration)
                : base(dynamoDbRootEntityConfiguration, dynamoDbEntityConfiguration)
            {
                References(f => f.Bar);
            }
        }
    }
}