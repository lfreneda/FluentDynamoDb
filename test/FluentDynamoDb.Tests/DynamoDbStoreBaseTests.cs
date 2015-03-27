using FluentDynamoDb.Mappers;
using Moq;
using NUnit.Framework;

namespace FluentDynamoDb.Tests
{
    [TestFixture]
    public class DynamoDbStoreBaseTests
    {
        private Mock<IClassMapLoader> _classMapLoaderFake;
        private DynamoDbStoreBase _dynamoDbStore;

        [SetUp]
        public void SetUp()
        {
            _classMapLoaderFake = new Mock<IClassMapLoader>();
            _classMapLoaderFake.Setup(c => c.Load<Foo>()).Returns(new ClassMap<Foo>());

            _dynamoDbStore = new DynamoDbStoreBase(_classMapLoaderFake.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _dynamoDbStore.ClearConfiguration();
        }

        [Test]
        public void LoadConfiguration_ShouldCreateClassMap()
        {
            _dynamoDbStore.LoadConfiguration<Foo>();
            _classMapLoaderFake.Verify(c => c.Load<Foo>(), Times.Once);
        }

        [Test]
        public void LoadConfiguration_ShouldCreateClassMapOnlyOnce()
        {
            _dynamoDbStore.LoadConfiguration<Foo>();
            _dynamoDbStore.LoadConfiguration<Foo>();

            _classMapLoaderFake.Verify(c => c.Load<Foo>(), Times.Once);
        }

        public class Foo
        {
            public string Name { get; set; }
        }
    }
}