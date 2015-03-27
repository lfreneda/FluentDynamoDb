using System;
using System.Linq;
using FluentDynamoDb.Configuration;
using FluentDynamoDb.Exceptions;

namespace FluentDynamoDb.Mappers
{
    public class ClassMapLoader : IClassMapLoader
    {
        public ClassMap<TType> Load<TType>()
        {
            if (FluentDynamoDbConfiguration.ClassMapLocationAssembly == null)
            {
                throw new FluentDynamoDbMappingException(
                    string.Format(
                        "ClassMapLocationAssembly was not provided, you should run FluentDynamoDbConfiguration.Configure() to define a assembly location for mappers"));
            }

            var mappingType =
                FluentDynamoDbConfiguration.ClassMapLocationAssembly.GetTypes()
                    .FirstOrDefault(t => t.IsSubclassOf(typeof (ClassMap<TType>)));
            if (mappingType == null)
            {
                throw new FluentDynamoDbMappingException(string.Format("Could not find mapping for class of type {0}",
                    typeof (TType)));
            }

            ClassMap<TType> mapping = null;

            try
            {
                mapping = Activator.CreateInstance(mappingType) as ClassMap<TType>;
                if (mapping == null)
                {
                    throw new FluentDynamoDbMappingException(
                        string.Format(
                            "Could not create a instance of type {0}, class must provide a public constructor",
                            mappingType));
                }
            }
            catch (MissingMethodException ex)
            {
                throw new FluentDynamoDbMappingException(
                    string.Format("Could not create a instance of type {0}, class must provide a public constructor",
                        mappingType), ex);
            }

            return mapping;
        }
    }
}