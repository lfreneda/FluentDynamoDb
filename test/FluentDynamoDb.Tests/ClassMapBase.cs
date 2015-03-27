using FluentDynamoDb.Mappers;
using Moq;

namespace FluentDynamoDb.Tests
{
    public class ClassMapBase
    {
        protected FieldConfiguration CurrentFieldConfiguration;
        protected Mock<DynamoDbEntityConfiguration> DynamoDbMappingConfigurationFake;
        protected DynamoDbRootEntityConfiguration DynamoDbRootEntityConfiguration;

        public virtual void SetUp()
        {
            DynamoDbRootEntityConfiguration = new DynamoDbRootEntityConfiguration();

            DynamoDbMappingConfigurationFake = new Mock<DynamoDbEntityConfiguration>();
            DynamoDbMappingConfigurationFake.Setup(c => c.AddFieldConfiguration(It.IsAny<FieldConfiguration>()))
                .Callback<FieldConfiguration>(fieldConfiguration => { CurrentFieldConfiguration = fieldConfiguration; });
        }
    }
}