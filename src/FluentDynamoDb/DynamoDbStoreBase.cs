using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using FluentDynamoDb.Mappers;

namespace FluentDynamoDb
{
    public class DynamoDbStoreBase
    {
        private static readonly IDictionary<Type, DynamoDbRootEntityConfiguration> MapConfigurations = new ConcurrentDictionary<Type, DynamoDbRootEntityConfiguration>();

        protected readonly IClassMapLoader ClassMapLoader;

        public DynamoDbStoreBase()
            : this(new ClassMapLoader())
        {
        }

        internal DynamoDbStoreBase(IClassMapLoader classMapLoader)
        {
            ClassMapLoader = classMapLoader;
        }

        internal DynamoDbRootEntityConfiguration LoadConfiguration<TEntity>()
        {
            if (!MapConfigurations.ContainsKey(typeof (TEntity)))
            {
                var classMap = ClassMapLoader.Load<TEntity>();
                MapConfigurations.Add(typeof (TEntity), classMap.GetRootConfiguration());
            }

            return MapConfigurations[typeof (TEntity)];
        }

        internal void ClearConfiguration()
        {
            MapConfigurations.Clear();
        }
    }
}