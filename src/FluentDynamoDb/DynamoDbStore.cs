using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using FluentDynamoDb.Mappers;

namespace FluentDynamoDb
{
    public class DynamoDbStore<TEntity, TKey> : DynamoDbStoreBase, IDynamoDbStore<TEntity, TKey>
        where TEntity : class, new()
    {
        private readonly IAmazonDynamoDB _amazonDynamoDbClient;
        private readonly Table _entityTable;
        private readonly DynamoDbMapper<TEntity> _mapper;

        public DynamoDbStore()
        {
            var rootConfiguration = LoadConfiguration<TEntity>();

            _amazonDynamoDbClient = new AmazonDynamoDBClient();
            _entityTable = Table.LoadTable(_amazonDynamoDbClient, rootConfiguration.TableName);
            _mapper = new DynamoDbMapper<TEntity>(rootConfiguration.DynamoDbEntityConfiguration);
        }

        public TEntity GetItem(TKey id)
        {
            dynamic idValue = id;
            var document = _entityTable.GetItem(idValue);
            return _mapper.ToEntity(document);
        }

        public TEntity DeleteItem(TKey id)
        {
            dynamic idValue = id;
            var deletedDocument = _entityTable.DeleteItem(idValue, new DeleteItemOperationConfig
            {
                ReturnValues = ReturnValues.AllOldAttributes
            });

            return _mapper.ToEntity(deletedDocument);
        }

        public TEntity UpdateItem(TEntity entity)
        {
            var document = _mapper.ToDocument(entity);

            var updatedDocument = _entityTable.UpdateItem(document, new UpdateItemOperationConfig
            {
                ReturnValues = ReturnValues.AllNewAttributes
            });

            return _mapper.ToEntity(updatedDocument);
        }

        public void PutItem(TEntity entity)
        {
            _entityTable.PutItem(_mapper.ToDocument(entity));
        }

        public void Dispose()
        {
            _amazonDynamoDbClient.Dispose();
        }
    }
}