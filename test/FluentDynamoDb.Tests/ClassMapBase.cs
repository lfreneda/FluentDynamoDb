using System;
using System.Linq;
using System.Linq.Expressions;
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
        }

        public class FooMap : ClassMap<Foo>
        {
            internal FooMap(DynamoDbEntityConfiguration dynamoDbEntityConfiguration)
                : base(dynamoDbEntityConfiguration)
            {
            }

            public new void Map(Expression<Func<Foo, object>> propertyExpression)
            {
                base.Map(propertyExpression);
            }
        }

        protected FooMap FooMapInstance;
        protected Mock<DynamoDbEntityConfiguration> DynamoDbEntityConfigurationFake;
        protected IFieldConfiguration CurrentFieldConfiguration;

        [SetUp]
        public virtual void SetUp()
        {
            DynamoDbEntityConfigurationFake = new Mock<DynamoDbEntityConfiguration>();
            DynamoDbEntityConfigurationFake.Setup(c => c.AddFieldConfiguration(It.IsAny<IFieldConfiguration>()))
                                           .Callback<IFieldConfiguration>(fieldConfiguration => { CurrentFieldConfiguration = fieldConfiguration; });

            FooMapInstance = new FooMap(DynamoDbEntityConfigurationFake.Object);
        }
    }
}
