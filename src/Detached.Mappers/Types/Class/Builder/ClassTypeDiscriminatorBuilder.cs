using Detached.Mappers.Annotations.Extensions;
using Detached.Mappers.Options;

namespace Detached.Mappers.Types.Class.Builder
{
    public class ClassTypeDiscriminatorBuilder<TType, TMember>
    {
        public ClassTypeDiscriminatorBuilder(ClassType typeOptions, MapperOptions mapperOptions)
        {
            ClassType = typeOptions;
            Options = mapperOptions;
        }

        public ClassType ClassType { get; }

        public MapperOptions Options { get; }

        public virtual ClassTypeBuilder<TNewType> Type<TNewType>()
        {
            var newType = (ClassType)Options.GetType(typeof(TNewType));

            return new ClassTypeBuilder<TNewType>(newType, Options);
        }

        public ClassTypeDiscriminatorBuilder<TType, TMember> HasValue(TMember value, Type instantiationType)
        {
            var annotation = ClassType.Annotations.DiscriminatorValues();

            if (annotation.IsDefined())
            {
                annotation.Value()[value] = instantiationType;
            }
            else
            {
                annotation.Set(new Dictionary<object, Type> { { value, instantiationType } });
            }

            return this;
        }

        public ClassTypeDiscriminatorBuilder<TType, TMember> HasValue<TInstantiation>(TMember value)
        {
            HasValue(value, typeof(TInstantiation));

            return this;
        } 
    }
}