using System.Collections.Generic;
using Amazon.DynamoDBv2.DocumentModel;
using NUnit.Framework;

namespace FluentDynamoDb.Tests
{
    [TestFixture]
    public class DynamoDbMapperWithSimpleClassTests
    {
        private DynamoDbMapper<Foo> _mapper;

        public class Foo
        {
            public string Title { get; set; }
        }

        [SetUp]
        public void SetUp()
        {
            var configuration = new DynamoDbMappingConfiguration();
            configuration.AddFieldConfiguration(new FieldConfiguration { PropertyName = "Title", Type = typeof(string) });

            _mapper = new DynamoDbMapper<Foo>(configuration);
        }

        [Test]
        public void ToDocument_GivenFooClass_ShouldConvertToDocument()
        {
            var document = _mapper.ToDocument(new Foo { Title = "Some title..." });

            Assert.IsTrue(document.Keys.Contains("Title"));
            Assert.AreEqual("Some title...", document["Title"].AsString());
        }

        [Test]
        public void ToDocument_GivenDocumentOfFoo_ShouldConvertToFooInstance()
        {
            var document = new Document();
            document["Title"] = "Some title...";

            var foo = _mapper.ToEntity(document);
            Assert.AreEqual("Some title...", foo.Title);
        }
    }
}