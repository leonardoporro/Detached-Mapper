using Detached.Mappers.EntityFramework.Loaders;

namespace Detached.Mappers.EntityFramework.Profiles
{
    public class Profile
    {
        public Profile(MapperOptions options)
        {
            Mapper = new Mapper(options);
            Loader = new Loader(options);
        }

        public Mapper Mapper { get; }

        public Loader Loader { get; }
    }
}
