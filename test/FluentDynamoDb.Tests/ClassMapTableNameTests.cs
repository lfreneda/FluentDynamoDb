using FluentDynamoDb.Mappers;
using NUnit.Framework;

namespace FluentDynamoDb.Tests
{
    [TestFixture]
    public class ClassMapTableNameTests : ClassMapBase
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            var fooMap = new FooMap(DynamoDbRootEntityConfiguration, DynamoDbMappingConfigurationFake.Object);
        }

        [Test]
        public void Map_WhenMappingFooTableName_TableNameShouldBeFooTable()
        {
            Assert.AreEqual("FooTable", DynamoDbRootEntityConfiguration.TableName);
        }

        public class Foo
        {
        }

        public class FooMap : ClassMap<Foo>
        {
            public FooMap(DynamoDbRootEntityConfiguration dynamoDbRootEntityConfiguration,
                DynamoDbEntityConfiguration dynamoDbEntityConfiguration)
                : base(dynamoDbRootEntityConfiguration, dynamoDbEntityConfiguration)
            {
                TableName("FooTable");
            }
        }
    }
}