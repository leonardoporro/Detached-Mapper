namespace Detached.Mappers.EntityFramework
{
    public class EFMapParameters : MapParameters
    {
        public object Profile { get; set; } = EFMapperServiceProvider.DEFAULT_PROFILE_KEY;
    }
}
