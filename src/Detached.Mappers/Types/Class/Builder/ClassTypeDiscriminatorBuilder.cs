using System;
using System.Collections.Generic;

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
            var annotation = TypeOptions.Annotations.DiscriminatorValues();

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