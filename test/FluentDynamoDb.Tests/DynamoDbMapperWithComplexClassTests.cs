using System.Collections.Generic;
using Amazon.DynamoDBv2.DocumentModel;
using NUnit.Framework;

namespace FluentDynamoDb.Tests
{
    [TestFixture]
    public class DynamoDbMapperWithComplexClassTests
    {
        private readonly DynamoDbMapper<Foo> _mapper;

        public class Foo
        {
            public string FooName { get; set; }
            public Bar Bar { get; set; }
        }

        public class Bar
        {
            public string BarName { get; set; }
            public Other Other { get; set; }
        }

        public class Other
        {
            public string OtherName { get; set; }
        }

        public DynamoDbMapperWithComplexClassTests()
        {
            var configuration = new DynamoDbEntityConfiguration
            {
                Fields = new List<IFieldConfiguration>
                {
                    new FieldConfiguration { PropertyName = "FooName", Type = typeof(string) },
                    new FieldConfiguration { PropertyName = "Bar", Type = typeof(Bar), IsComplexType = true, FieldConfigurations = new List<FieldConfiguration>
                        {
                            new FieldConfiguration { PropertyName = "BarName", Type = typeof(string) },
                            new FieldConfiguration { PropertyName = "Other", Type = typeof(Other), IsComplexType = true, FieldConfigurations = new List<FieldConfiguration>
                            {
                                new FieldConfiguration { PropertyName = "OtherName", Type = typeof(string)}
                            }}
                        }
                    },
                }
            };

            _mapper = new DynamoDbMapper<Foo>(configuration);
        }

        [Test]
        public void ToDocument_GivenFooClass_ShouldConvertToDocument()
        {
            var documentFoo = _mapper.ToDocument(new Foo { FooName = "TheFooName", Bar = new Bar { BarName = "TheBarName", Other = new Other { OtherName = "TheOtherName" } } });

            Assert.IsTrue(documentFoo.Keys.Contains("FooName"));
            Assert.AreEqual("TheFooName", documentFoo["FooName"].AsString());
            Assert.IsTrue(documentFoo.Keys.Contains("Bar"));
            var documentoBar = documentFoo["Bar"].AsDocument();
            Assert.IsTrue(documentoBar.Keys.Contains("BarName"));
            Assert.AreEqual("TheBarName", documentoBar["BarName"].AsString());
            var documentOther = documentoBar["Other"].AsDocument();
            Assert.IsTrue(documentOther.Keys.Contains("OtherName"));
            Assert.AreEqual("TheOtherName", documentOther["OtherName"].AsString());
        }

        [Test]
        public void ToEntity_GivenDocumentOfFoo_ShouldConvertToEntity()
        {
            var documentFoo = new Document();
            documentFoo["FooName"] = "TheFooName";
            var documentOther = new Document();
            documentOther["OtherName"] = "TheOtherName";
            var documentBar = new Document();
            documentBar["BarName"] = "TheBarName";
            documentBar["Other"] = documentOther;
            documentFoo["Bar"] = documentBar;

            var foo = _mapper.ToEntity(documentFoo);

            Assert.AreEqual("TheFooName", foo.FooName);
            Assert.IsNotNull(foo.Bar);
            Assert.AreEqual("TheBarName", foo.Bar.BarName);
            Assert.IsNotNull(foo.Bar.Other);
            Assert.AreEqual("TheOtherName", foo.Bar.Other.OtherName);
        }
    }
}