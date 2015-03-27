using System;

namespace FluentDynamoDb
{
    public interface IDynamoDbStore<TEntity> : IDisposable
        where TEntity : class, new()
    {
        TEntity GetItem(Guid id);
        void PutItem(TEntity entity);
        TEntity UpdateItem(TEntity entity);
        TEntity DeleteItem(Guid id);
    }
}