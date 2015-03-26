using System.Collections.Generic;
using System.Linq;
using Amazon.DynamoDBv2.DocumentModel;
using NUnit.Framework;

namespace FluentDynamoDb.Tests
{
    [TestFixture]
    public class DynamoDbMapperWithObjectListToEntityTests : DynamoDbMapperWithObjectListBase
    {
        private Foo _foo;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var documentFoo = new Document();
            documentFoo["FooName"] = "TheFooName";

            var documentOther = new Document();
            documentOther["OtherName"] = "TheOtherName";

            documentFoo["Other"] = documentOther;

            var documentBar1 = new Document();
            documentBar1["BarName"] = "BarName1";

            var documentBar2 = new Document();
            documentBar2["BarName"] = "BarName2";

            documentFoo["Bars"] = new List<Document> { documentBar1, documentBar2 };

            _foo = Mapper.ToEntity(documentFoo);
        }

        [Test]
        public void ToEntity_GivenDocumentOfFoo_fooShouldNotBeNull()
        {
            Assert.IsNotNull(_foo);
        }

        [Test]
        public void ToEntity_GivenDocumentOfFoo_FooNameShoulBeTheFooName()
        {
            Assert.AreEqual("TheFooName", _foo.FooName);
        }

        [Test]
        public void ToEntity_GivenDocumentOfFoo_OtherShouldNotBeNull()
        {
            Assert.IsNotNull(_foo.Other);
        }

        [Test]
        public void ToEntity_GivenDocumentOfFoo_fooOtherOtherNameShouldBeTheOtherName()
        {
            Assert.AreEqual("TheOtherName", _foo.Other.OtherName);
        }

        [Test]
        public void ToEntity_GivenDocumentOfFoo_BarsShouldNotBeNull()
        {
            Assert.IsNotNull(_foo.Bars);
        }

        [Test]
        public void ToEntity_GivenDocumentOfFoo_BarsCountShouldBe2()
        {
            Assert.AreEqual(2, _foo.Bars.Count());
        }

        [Test]
        public void ToEntity_GivenDocumentOfFoo_BarsBarNameShouldBeAsExpected()
        {
            Assert.AreEqual("BarName1", _foo.Bars.ElementAt(0).BarName);
            Assert.AreEqual("BarName2", _foo.Bars.ElementAt(1).BarName);
        }
    }
}