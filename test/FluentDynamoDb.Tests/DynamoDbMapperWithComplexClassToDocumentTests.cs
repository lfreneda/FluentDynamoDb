using Amazon.DynamoDBv2.DocumentModel;
using NUnit.Framework;

namespace FluentDynamoDb.Tests
{
    [TestFixture]
    public class DynamoDbMapperWithComplexClassToDocumentBase : DynamoDbMapperWithComplexClassBase
    {
        private Document _documentFoo;

        [SetUp]
        public void SetUp()
        {
            var complexFooInstance = new Foo
            {
                FooName = "TheFooName",
                Bar = new Bar
                {
                    BarName = "TheBarName",
                    Other = new Other
                    {
                        OtherName = "TheOtherName"
                    }
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
            Assert.AreEqual("TheBarName", documentoBar["BarName"].AsString());
        }


        [Test]
        public void ToDocument_GivenFooComplexClass_InnerDocumentBarShouldContainsOtherKey()
        {
            var documentoBar = _documentFoo["Bar"].AsDocument();
            Assert.IsTrue(documentoBar.Keys.Contains("Other"));
        }

        [Test]
        public void ToDocument_GivenFooComplexClass_InnerInnerDocumentOtherShouldContainsOtherNameKey()
        {
            var documentoBar = _documentFoo["Bar"].AsDocument();
            var documentOther = documentoBar["Other"].AsDocument();
            Assert.IsTrue(documentOther.Keys.Contains("OtherName"));
        }


        [Test]
        public void ToDocument_GivenFooComplexClass_InnerInnerDocumentOtherOtherNameValueShouldBeTheOtherName()
        {
            var documentoBar = _documentFoo["Bar"].AsDocument();
            var documentOther = documentoBar["Other"].AsDocument();
            Assert.AreEqual("TheOtherName", documentOther["OtherName"].AsString());
        }
    }
}