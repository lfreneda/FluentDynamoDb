using System.Collections.Generic;
using Amazon.DynamoDBv2.DocumentModel;
using NUnit.Framework;

namespace FluentDynamoDb.Tests.Mappers
{
    [TestFixture]
    public class DynamoDbMapperWithObjectListToDocumentTests : DynamoDbMapperWithObjectListBase
    {
        private Document _document;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            _document = Mapper.ToDocument(new Foo
            {
                FooName = "TheFooName",
                Bars = new List<Bar>
                {
                    new Bar {BarName = "BarName1"},
                    new Bar {BarName = "BarName2"}
                },
                Other = new Other {OtherName = "TheOtherName"}
            });
        }

        [Test]
        public void ToDocument_GivenFooClass_DocumentShouldContainsFooNameKey()
        {
            Assert.IsTrue(_document.Keys.Contains("FooName"));
        }

        [Test]
        public void ToDocument_GivenFooClass_DocumentShouldContainsBarsKey()
        {
            Assert.IsTrue(_document.Keys.Contains("Bars"));
        }

        [Test]
        public void ToDocument_GivenFooClass_DocumentShouldContainsOtherKey()
        {
            Assert.IsTrue(_document.Keys.Contains("Other"));
        }

        [Test]
        public void ToDocument_GivenFooClass_FooNameShouldBeTheFooName()
        {
            Assert.AreEqual("TheFooName", _document["FooName"].AsString());
        }

        [Test]
        public void ToDocument_GivenFooClass_BarsCountShouldBe2()
        {
            var documentBars = _document["Bars"].AsListOfDocument();
            Assert.AreEqual(2, documentBars.Count);
        }

        [Test]
        public void ToDocument_GivenFooClass_BarsItemShouldContainsBarNameKey()
        {
            var documentBars = _document["Bars"].AsListOfDocument();
            Assert.IsTrue(documentBars[0].Keys.Contains("BarName"));
            Assert.IsTrue(documentBars[1].Keys.Contains("BarName"));
        }

        [Test]
        public void ToDocument_GivenFooClass_BarNameShouldMatchBarName()
        {
            var documentBars = _document["Bars"].AsListOfDocument();
            Assert.AreEqual("BarName1", documentBars[0]["BarName"].AsString());
            Assert.AreEqual("BarName2", documentBars[1]["BarName"].AsString());
        }

        [Test]
        public void ToDocument_GivenFooClass_DocumentOtherShouldContainsOtherNameKey()
        {
            var documentOther = _document["Other"].AsDocument();
            Assert.IsTrue(documentOther.Keys.Contains("OtherName"));
        }

        [Test]
        public void ToDocument_GivenFooClass_DocumentOherOtherNameShouldBeTheOtherName()
        {
            var documentOther = _document["Other"].AsDocument();
            Assert.AreEqual("TheOtherName", documentOther["OtherName"].AsString());
        }
    }
}