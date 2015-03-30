using System;

namespace FluentDynamoDb
{
    public interface IDynamoDbStore<TEntity, in TKey> : IDisposable
        where TEntity : class, new()
    {
        TEntity GetItem(TKey id);
        void PutItem(TEntity entity);
        TEntity UpdateItem(TEntity entity);
        TEntity DeleteItem(TKey id);
    }
}