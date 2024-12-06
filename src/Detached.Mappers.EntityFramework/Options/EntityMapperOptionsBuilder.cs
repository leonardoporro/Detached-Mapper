using Detached.Mappers.Options;

namespace Detached.Mappers.EntityFramework.Options
{
    public class EntityMapperOptionsBuilder : MapperOptionsBuilder
    {
        public EntityMapperOptionsBuilder(EntityMapperOptions options)
            : base(options)
        {
        }

        public EntityMapperOptionsBuilder()
            : base(new EntityMapperOptions())
        {
        }

        public new EntityMapperOptions Options => (EntityMapperOptions)base.Options;

        public EntityMapperOptionsBuilder AddProfile(object profileKey, Action<MapperOptionsBuilder> configure)
        {
            var options = Options.Profiles.GetOrAdd(profileKey, key => new MapperOptions());

            var builder = new MapperOptionsBuilder(options);

            configure?.Invoke(builder);

            return this;
        }
    }
}