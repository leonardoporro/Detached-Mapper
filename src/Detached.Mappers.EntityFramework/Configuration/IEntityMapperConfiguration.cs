namespace Detached.Mappers.EntityFramework.Configuration
{
    public interface IEntityMapperConfiguration
    {
        void Apply(EntityMapperOptionsBuilder builder);
    }
}
