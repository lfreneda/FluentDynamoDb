using System.Collections.Generic;
using System.Linq;
using Amazon.DynamoDBv2.DocumentModel;
using FluentDynamoDb.Exceptions;
using FluentDynamoDb.Mappers;
using NUnit.Framework;

namespace FluentDynamoDb.Tests.Mappers
{
    [TestFixture]
    public class DynamoDbMapperWithObjectListUsingCamelCaseUnderscoreFieldToEntityWithNoBackingFieldTests
    {
        protected DynamoDbMapper<Foo> Mapper;
        private Document _documentFoo;

        [SetUp]
        public void SetUp()
        {
            var configuration = new DynamoDbEntityConfiguration();

            configuration.AddFieldConfiguration(new FieldConfiguration("FooName", typeof(string)));

            configuration.AddFieldConfiguration(new FieldConfiguration("Bars", typeof(IEnumerable<Bar>), true, new List<FieldConfiguration> {
                    new FieldConfiguration("BarName", typeof (string))
                }, accessStrategy: AccessStrategy.CamelCaseUnderscoreName));

            Mapper = new DynamoDbMapper<Foo>(configuration);

            _documentFoo = new Document();
            _documentFoo["FooName"] = "TheFooName";

            var documentBar1 = new Document();
            documentBar1["BarName"] = "BarName1";

            var documentBar2 = new Document();
            documentBar2["BarName"] = "BarName2";

            _documentFoo["Bars"] = new List<Document> { documentBar1, documentBar2 };
        }

        public class Foo
        {
            public string FooName { get; set; }
            public IEnumerable<Bar> Bars { get; set; }
        }

        public class Bar
        {
            public string BarName { get; set; }
        }

        [Test]
        public void ToEntity_GivenClassFooWithNoBackingField_ShouldThrowException()
        {
            Assert.That(() => Mapper.ToEntity(_documentFoo),
                Throws.Exception.TypeOf<FluentDynamoDbMappingException>()
                      .With
                      .Message
                      .EqualTo("Could not find backing field named _bars of type FluentDynamoDb.Tests.Mappers.DynamoDbMapperWithObjectListUsingCamelCaseUnderscoreFieldToEntityWithNoBackingFieldTests+Foo"));
        }
    }
}