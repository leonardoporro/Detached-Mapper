using Detached.Mappers.Types;
using Detached.Mappers.Types.Class;
using HotChocolate;
using System;

namespace Detached.Mappers.HotChocolate.Types
{
    public class OptionalClassTypeFactory : ClassTypeFactory
    {
        public const string OPTIONAL_ANNOTATION = "HOTCHOCOLATE_OPTIONAL";

        public override IType Create(MapperOptions options, Type type)
        {
            ClassType result = null;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Optional<>))
            {
                result = (ClassType)base.Create(options, type);

                result.ItemClrType = type.GetGenericArguments()[0];
                result.Annotations.Add(OPTIONAL_ANNOTATION, true);
            }

            return result;
        }
    }
}