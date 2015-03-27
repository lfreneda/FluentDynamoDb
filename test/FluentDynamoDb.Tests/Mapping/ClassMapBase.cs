using FluentDynamoDb.Mapping.Configuration;
using Moq;

namespace FluentDynamoDb.Tests.Mapping
{
    public class ClassMapBase
    {
        protected Mock<DynamoDbEntityConfiguration> DynamoDbMappingConfigurationFake;
        protected DynamoDbRootEntityConfiguration DynamoDbRootEntityConfiguration;
        protected FieldConfiguration CurrentFieldConfiguration;

        public virtual void SetUp()
        {
            DynamoDbRootEntityConfiguration = new DynamoDbRootEntityConfiguration();

            DynamoDbMappingConfigurationFake = new Mock<DynamoDbEntityConfiguration>();
            DynamoDbMappingConfigurationFake.Setup(c => c.AddFieldConfiguration(It.IsAny<FieldConfiguration>()))
                                             .Callback<FieldConfiguration>(fieldConfiguration => { CurrentFieldConfiguration = fieldConfiguration; });

        }
    }
}
