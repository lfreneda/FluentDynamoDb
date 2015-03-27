using Amazon.DynamoDBv2.DocumentModel;
using NUnit.Framework;

namespace FluentDynamoDb.Tests.Mappers
{
    [TestFixture]
    public class DynamoDbMapperWithComplexIncompleteClassToDocumentTests : DynamoDbMapperWithComplexClassBase
    {
        private Document _documentFoo;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var complexFooInstance = new Foo
            {
                FooName = "TheFooName",
                Bar = new Bar
                {
                    BarName = null
                }
            };

            _documentFoo = Mapper.ToDocument(complexFooInstance);
        }

        [Test]
        public void ToDocument_GivenFooComplexClass_ShouldContainsKeyFooName()
        {
            Assert.IsTrue(_documentFoo.Keys.Contains("FooName"));
        }

        [Test]
        public void ToDocument_GivenFooComplexClass_FooNameValueShouldBeTheFooName()
        {
            Assert.AreEqual("TheFooName", _documentFoo["FooName"].AsString());
        }

        [Test]
        public void ToDocument_GivenFooComplexClass_ShouldContainsKeyBar()
        {
            Assert.IsTrue(_documentFoo.Keys.Contains("Bar"));
        }

        [Test]
        public void ToDocumento_GivenFooComplexClass_InnerDocumentBarShouldContainsBarNameKey()
        {
            var documentoBar = _documentFoo["Bar"].AsDocument();
            Assert.IsTrue(documentoBar.Keys.Contains("BarName"));
        }

        [Test]
        public void ToDocumento_GivenFooComplexClass_InnerDocumentBarBarNameValueShoulBeTheBarName()
        {
            var documentoBar = _documentFoo["Bar"].AsDocument();
            Assert.AreEqual(null, documentoBar["BarName"].AsString());
        }

        [Test]
        public void ToDocument_GivenFooComplexClass_InnerDocumentBarShouldNotContainsOtherKey()
        {
            var documentoBar = _documentFoo["Bar"].AsDocument();
            Assert.IsFalse(documentoBar.Keys.Contains("Other"));
        }
    }
}