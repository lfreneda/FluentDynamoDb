using System.Reflection;
using FluentDynamoDb.Configuration;
using FluentDynamoDb.Exceptions;
using FluentDynamoDb.Mappers;
using NUnit.Framework;

namespace FluentDynamoDb.Tests.Mappers
{
    [TestFixture]
    public class ClassMapLoaderMissingClassMapTests
    {
        [Test]
        public void LoadMapper_GivenFooClassWithNoClassMap_ShouldThrownException()
        {
            FluentDynamoDbConfiguration.Configure(Assembly.GetExecutingAssembly());

            var classMapLoader = new ClassMapLoader();

            Assert.That(() => classMapLoader.Load<Foo>(),
                Throws.Exception
                    .TypeOf<FluentDynamoDbMappingException>()
                    .With
                    .Message
                    .EqualTo(
                        "Could not find mapping for class of type FluentDynamoDb.Tests.Mappers.ClassMapLoaderMissingClassMapTests+Foo"));
        }

        public class Foo
        {
            public string Name { get; set; }
        }
    }
}