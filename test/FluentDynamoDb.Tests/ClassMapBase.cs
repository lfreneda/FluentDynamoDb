using Moq;

namespace FluentDynamoDb.Tests
{
    public class ClassMapBase
    {
        protected Mock<DynamoDbEntityConfiguration> DynamoDbMappingConfigurationFake;
        protected DynamoDbRootEntityConfiguration DynamoDbRootEntityConfiguration;
        protected IFieldConfiguration CurrentFieldConfiguration;

        public virtual void SetUp()
        {
            DynamoDbRootEntityConfiguration = new DynamoDbRootEntityConfiguration();

            DynamoDbMappingConfigurationFake = new Mock<DynamoDbEntityConfiguration>();
            DynamoDbMappingConfigurationFake.Setup(c => c.AddFieldConfiguration(It.IsAny<IFieldConfiguration>()))
                                             .Callback<IFieldConfiguration>(fieldConfiguration => { CurrentFieldConfiguration = fieldConfiguration; });

        }
    }
}
