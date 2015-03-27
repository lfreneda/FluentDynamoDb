using System.Linq;
using FluentDynamoDb.Exceptions;
using FluentDynamoDb.Mappers;
using NUnit.Framework;

namespace FluentDynamoDb.Tests
{
    [TestFixture]
    public class ClassMapComplexTypeMissingPublicConstructorClassMapTests : ClassMapBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [Test]
        public void Map_WhenMappingBarWithIsAComplexTypeButNotProvidingAClassMapWithPublicConstructor_ShouldThrowsException()
        {
            Assert.That(() => new FooMap(DynamoDbRootEntityConfiguration, DynamoDbMappingConfigurationFake.Object),
                Throws.Exception
                      .TypeOf<FluentDynamoDbMappingException>()
                      .With
                      .Message
                      .EqualTo("Could not create a instance of type FluentDynamoDb.Tests.ClassMapComplexTypeMissingPublicConstructorClassMapTests+BarMap, class must provide a public constructor"));
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

        public class BarMap : ClassMap<Bar>
        {
            private BarMap() // <------- private constructor
            {
                Map(c => c.BarName);
            }
        }
    }
}