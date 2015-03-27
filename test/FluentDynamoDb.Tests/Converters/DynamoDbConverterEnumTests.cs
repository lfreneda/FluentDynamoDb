using FluentDynamoDb.Converters;
using NUnit.Framework;

namespace FluentDynamoDb.Tests.Converters
{
    [TestFixture]
    public class DynamoDbConverterEnumTests
    {
        public enum Foo
        {
            Option1 = 1,
            Option2 = 2
        }

        private DynamoDbConverterEnum<Foo> _dynamoDbConverterEnum;

        [SetUp]
        public void SetUp()
        {
            _dynamoDbConverterEnum = new DynamoDbConverterEnum<Foo>();
        }

        [Test]
        public void ToEntry_WhenParamValueIsNull_ShouldReturnNull()
        {
            Assert.AreEqual(null, _dynamoDbConverterEnum.ToEntry(null));
        }

        [Test]
        public void FromEntry_WhenDynamoDbEntryIsNull_ShouldReturnNull()
        {
            Assert.AreEqual(null, _dynamoDbConverterEnum.FromEntry(null));
        }
    }
}