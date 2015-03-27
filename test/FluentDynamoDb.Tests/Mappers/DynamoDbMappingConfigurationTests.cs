using System.Linq;
using FluentDynamoDb.Exceptions;
using FluentDynamoDb.Mappers;
using NUnit.Framework;

namespace FluentDynamoDb.Tests.Mappers
{
    [TestFixture]
    public class DynamoDbMappingConfigurationTests
    {
        [Test]
        public void AddFieldConfiguration_GivenANewDynamoDbEntityConfiguration_FieldsCountShouldBe1()
        {
            var dynamoDbEntityConfiguration = new DynamoDbEntityConfiguration();

            dynamoDbEntityConfiguration.AddFieldConfiguration(new FieldConfiguration("Name", "".GetType()));

            Assert.AreEqual(1, dynamoDbEntityConfiguration.Fields.Count());
        }

        [Test]
        public void AddFieldConfiguration_MappingMoreThanOnceTheSameField_ShouldThrowException()
        {
            var dynamoDbEntityConfiguration = new DynamoDbEntityConfiguration();

            dynamoDbEntityConfiguration.AddFieldConfiguration(new FieldConfiguration("FooName", typeof (string)));

            Assert.That(
                () =>
                    dynamoDbEntityConfiguration.AddFieldConfiguration(new FieldConfiguration("FooName", typeof (string))),
                Throws.Exception.TypeOf<FluentDynamoDbMappingException>()
                    .With
                    .Message
                    .EqualTo("Property FooName has already been mapped"));
        }
    }
}