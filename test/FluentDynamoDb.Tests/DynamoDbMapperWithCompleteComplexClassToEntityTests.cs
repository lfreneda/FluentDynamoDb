using Amazon.DynamoDBv2.DocumentModel;
using NUnit.Framework;

namespace FluentDynamoDb.Tests
{
    [TestFixture]
    public class DynamoDbMapperWithCompleteComplexClassToEntityTests : DynamoDbMapperWithComplexClassBase
    {
        private Foo _foo;

        [SetUp]
        public void SetUp()
        {
            var documentFoo = new Document();
            documentFoo["FooName"] = "TheFooName";
            var documentOther = new Document();
            documentOther["OtherName"] = "TheOtherName";
            var documentBar = new Document();
            documentBar["BarName"] = "TheBarName";
            documentBar["Other"] = documentOther;
            documentFoo["Bar"] = documentBar;

            _foo = Mapper.ToEntity(documentFoo);
        }

        [Test]
        public void ToEntity_GivenDocumentOfFoo_FooShouldNotBeNull()
        {
            Assert.IsNotNull(_foo);
        }

        [Test]
        public void ToEntity_GivenDocumentOfFoo_FooNameShoulBeTheFooName()
        {
            Assert.AreEqual("TheFooName", _foo.FooName);
        }

        [Test]
        public void ToEntity_GivenDocumentOfFoo_fooBarShouldNotBeNull()
        {
            Assert.IsNotNull(_foo.Bar);
        }

        [Test]
        public void ToEntity_GivenDocumentOfFoo_fooBarBarNameShouldBeTheBarName()
        {
            Assert.AreEqual("TheBarName", _foo.Bar.BarName);
        }

        [Test]
        public void ToEntity_GivenDocumentOfFoo_fooBarOtherShouldNotBeNull()
        {
            Assert.IsNotNull(_foo.Bar.Other);
        }

        [Test]
        public void ToEntity_GivenDocumentOfFoo_fooBarOtherOtherNameShouldBeTheOtherName()
        {
            Assert.AreEqual("TheOtherName", _foo.Bar.Other.OtherName);
        }
    }
}