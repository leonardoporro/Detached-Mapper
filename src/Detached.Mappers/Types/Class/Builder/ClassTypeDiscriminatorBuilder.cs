using System;

namespace Detached.Mappers.Types.Class.Builder
{
    public class ClassTypeDiscriminatorBuilder<TType, TMember>
    {
        public ClassTypeDiscriminatorBuilder(ClassType typeOptions)
        {
            TypeOptions = typeOptions;
        }

        public ClassType TypeOptions { get; }

        public ClassTypeDiscriminatorBuilder<TType, TMember> HasValue(TMember value, Type instantiationType)
        {
            TypeOptions.GetDiscriminatorValues()[value] = instantiationType;

            return this;
        }

        public ClassTypeDiscriminatorBuilder<TType, TMember> HasValue<TInstantiation>(TMember value)
        {
            HasValue(value, typeof(TInstantiation));

            return this;
        }
    }
}