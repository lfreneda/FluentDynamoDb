﻿using System.Linq;
using FluentDynamoDb.Mapping;
using FluentDynamoDb.Mapping.Configuration;
using NUnit.Framework;

namespace FluentDynamoDb.Tests.Mapping
{
    [TestFixture]
    public class ClassMapComplexTypeTests : ClassMapBase
    {
        public class Foo
        {
            public Bar Bar { get; set; }
        }

        public class Bar
        {
            public string BarName { get; set; }
        }

        public class BarMap : ClassMap<Bar>
        {
            public BarMap()
            {
                Map(c => c.BarName);
            }
        }

        public class FooMap : ClassMap<Foo>
        {
            public FooMap(DynamoDbRootEntityConfiguration dynamoDbRootEntityConfiguration, DynamoDbEntityConfiguration dynamoDbEntityConfiguration)
                : base(dynamoDbRootEntityConfiguration, dynamoDbEntityConfiguration)
            {
                References(f => f.Bar);
            }
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var fooMap = new FooMap(DynamoDbRootEntityConfiguration, DynamoDbMappingConfigurationFake.Object);
        }

        [Test]
        public void Map_WhenMappingBarWithIsAComplexType_ShouldCreateAFieldConfigurationAsExpected()
        {
            Assert.AreEqual("Bar", CurrentFieldConfiguration.PropertyName);
            Assert.AreEqual(typeof(Bar), CurrentFieldConfiguration.Type);
            Assert.AreEqual(true, CurrentFieldConfiguration.IsComplexType);
            Assert.AreEqual(1, CurrentFieldConfiguration.FieldConfigurations.Count);
            Assert.AreEqual("BarName", CurrentFieldConfiguration.FieldConfigurations.ElementAt(0).PropertyName);
            Assert.AreEqual(typeof(string), CurrentFieldConfiguration.FieldConfigurations.ElementAt(0).Type);
        }
    }
}