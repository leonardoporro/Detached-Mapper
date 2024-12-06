using Detached.Mappers.Types;
using Detached.Mappers.Types.Class;
using Detached.Mappers.Types.Class.Builder;

namespace Detached.Mappers.Options
{
    public class MapperOptionsBuilder
    {
        public MapperOptionsBuilder(MapperOptions options)
        {
            Options = options;
        }

        public MapperOptionsBuilder()
        {
            Options = new MapperOptions();
        }

        public MapperOptions Options { get; }

        public virtual ClassTypeBuilder<TType> Type<TType>()
        {
            IType type = Options.GetTypeConfiguration(typeof(TType));

            return new ClassTypeBuilder<TType>((ClassType)type, Options);
        }
    }
}