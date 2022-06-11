using System;

namespace Detached.Mappers.TypeOptions.Class.Builder
{
    public class ClassDiscriminatorBuilder<TType, TMember>
    {
        public ClassDiscriminatorBuilder(ClassTypeOptions typeOptions)
        {
            TypeOptions = typeOptions;
        }

        public ClassTypeOptions TypeOptions { get; }

        public ClassDiscriminatorBuilder<TType, TMember> Value(TMember value, Type instantiationType)
        {
            TypeOptions.GetDiscriminatorValues()[value] = instantiationType;

            return this;
        }
    }
}