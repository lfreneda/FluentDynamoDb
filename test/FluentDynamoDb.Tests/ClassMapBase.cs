using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Moq;
using NUnit.Framework;

namespace FluentDynamoDb.Tests
{
    public class ClassMapBase
    {
        public class Foo
        {
            public string FooString { get; set; }
            public Guid FooGuid { get; set; }
            public decimal FooDecimal { get; set; }
            public int FooInt { get; set; }
            public DateTime FooDate { get; set; }
            public bool FooBool { get; set; }
            public Bar Bar { get; set; }
            public BarNoMap BarNoMap { get; set; }
        }

        public class Bar
        {
            public string BarName { get; set; }
        }

        public class BarNoMap
        {
            public string BarName { get; set; }
        }

        public class FooMap : ClassMap<Foo>
        {
            internal FooMap(DynamoDbEntityConfiguration dynamoDbEntityConfiguration, DynamoDbMappingConfiguration dynamoDbMappingConfiguration)
                : base(dynamoDbEntityConfiguration, dynamoDbMappingConfiguration)
            {
            }

            public new void Map<TType>(Expression<Func<Foo, TType>> propertyExpression)
            {
                base.Map<TType>(propertyExpression);
            }

            public new void References<TType>(Expression<Func<Foo, TType>> propertyExpression)
            {
                base.References<TType>(propertyExpression);
            }
        }

        protected FooMap FooMapInstance;
        protected Mock<DynamoDbMappingConfiguration> DynamoDbMappingConfigurationFake;
        protected IFieldConfiguration CurrentFieldConfiguration;

        [SetUp]
        public virtual void SetUp()
        {
            DynamoDbMappingConfigurationFake = new Mock<DynamoDbMappingConfiguration>();

            DynamoDbMappingConfigurationFake.Setup(c => c.AddFieldConfiguration(It.IsAny<IFieldConfiguration>()))
                                           .Callback<IFieldConfiguration>(fieldConfiguration => { CurrentFieldConfiguration = fieldConfiguration; });

            FooMapInstance = new FooMap(new DynamoDbEntityConfiguration { ClassMapAssembly = Assembly.GetExecutingAssembly() }, DynamoDbMappingConfigurationFake.Object);
        }
    }
}
