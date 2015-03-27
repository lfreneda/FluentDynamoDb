namespace FluentDynamoDb.Mappers
{
    public interface IClassMapLoader
    {
        ClassMap<TType> Load<TType>();
    }
}