using System.Collections.Generic;
using Amazon.DynamoDBv2.DocumentModel;
using NUnit.Framework;

namespace FluentDynamoDb.Tests
{
    [TestFixture]
    public class DynamoDbMappeirWithEnumTests
    {
        private DynamoDbMapper<Foo> _mapper;

        public enum Gender
        {
            Male = 1,
            Female = 2
        }

        public class Foo
        {
            public Gender Gender { get; set; }
        }

        [SetUp]
        public void SetUp()
        {
            var configuration = new DynamoDbMappingConfiguration();
            
            configuration.AddFieldConfiguration(new FieldConfiguration
            {
                PropertyName = "Gender",
                Type = typeof(Gender),
                PropertyConverter = new DynamoDbConverterEnum<Gender>()
            });

            _mapper = new DynamoDbMapper<Foo>(configuration);
        }

        [Test]
        public void ToDocument_GivenFooClass_ShouldConvertToDocument()
        {
            var document = _mapper.ToDocument(new Foo { Gender = Gender.Male });

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
    }
}