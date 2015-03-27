using Amazon.DynamoDBv2.DocumentModel;
using FluentDynamoDb.Converters;
using FluentDynamoDb.Mappers;
using NUnit.Framework;

namespace FluentDynamoDb.Tests.Mappers
{
    [TestFixture]
    public class DynamoDbMappeirWithEnumTests
    {
        public enum Gender
        {
            Male = 1,
            Female = 2
        }

        private DynamoDbMapper<Foo> _mapper;

        [SetUp]
        public void SetUp()
        {
            var configuration = new DynamoDbEntityConfiguration();

            configuration.AddFieldConfiguration(new FieldConfiguration("Gender", typeof (Gender),
                propertyConverter: new DynamoDbConverterEnum<Gender>()));

            _mapper = new DynamoDbMapper<Foo>(configuration);
        }

        [Test]
        public void ToDocument_GivenFooClass_ShouldConvertToDocument()
        {
            var document = _mapper.ToDocument(new Foo {Gender = Gender.Male});

            Assert.IsTrue(document.Keys.Contains("Gender"));
            Assert.AreEqual("Male", document["Gender"].AsString());
        }

        [Test]
        public void ToDocument_GivenDocumentOfFoo_ShouldConvertToFooInstance()
        {
            var document = new Document();
            document["Gender"] = "Male";

            var foo = _mapper.ToEntity(document);
            Assert.AreEqual(Gender.Male, foo.Gender);
        }

        public class Foo
        {
            public Gender Gender { get; set; }
        }
    }
}