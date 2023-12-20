namespace Detached.Mappers.EntityFramework.Configuration
{
    public interface IEntityMapperCustomizer
    {
        void Customize(EntityMapperOptionsBuilder builder);
    }
}
