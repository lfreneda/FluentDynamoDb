using System.Reflection;
using FluentDynamoDb.Configuration;
using FluentDynamoDb.Exceptions;
using FluentDynamoDb.Mappers;
using NUnit.Framework;

namespace FluentDynamoDb.Tests.Mappers
{
    [TestFixture]
    public class ClassMapLoaderMissingPublicConstructorClassMapTests
    {
        [Test]
        public void LoadMapper_GivenFooClassAndFooMapWithPrivateConstructor_ShouldThrowsException()
        {
            FluentDynamoDbConfiguration.Configure(Assembly.GetExecutingAssembly());

            var classMapLoader = new ClassMapLoader();

            Assert.That(() => classMapLoader.Load<Foo>(),
                Throws.Exception
                    .TypeOf<FluentDynamoDbMappingException>()
                    .With
                    .Message
                    .EqualTo(
                        "Could not create a instance of type FluentDynamoDb.Tests.Mappers.ClassMapLoaderMissingPublicConstructorClassMapTests+FooMap, class must provide a public constructor"));
        }

        public class Foo
        {
            public string Name { get; set; }
        }

        public class FooMap : ClassMap<Foo>
        {
            private FooMap() // <---- private constructor
            {
                Map(c => c.Name);
            }
        }
    }
}