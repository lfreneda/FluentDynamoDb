using System;
using System.Linq;
using System.Linq.Expressions;
using Moq;
using NUnit.Framework;

namespace FluentDynamoDb.Tests
{
    [TestFixture]
    public class ClassMapComplexTypeTests : ClassMapBase
    {
        public class BarMap : ClassMap<Bar>
        {
            public BarMap(DynamoDbEntityConfiguration dynamoDbEntityConfiguration)
                : base(dynamoDbEntityConfiguration)
            {
                Map(c => c.BarName);
            }
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            FooMapInstance.References(c => c.Bar);
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