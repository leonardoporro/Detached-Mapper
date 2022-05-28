namespace Detached.Mappers.TypeMappers.Entity
{
    public interface IEntityKey
    {
        object[] ToObject();

        bool IsEmpty { get; }
    }
}
