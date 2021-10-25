namespace TS.Microservices.Domain.Abstractions
{
    public interface IEntity
    {
        object GetKey();
    }

    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; }
    }
}
