﻿using Detached.Mappers.Options;

namespace Detached.Mappers.Types
{
    public interface ITypeConvention
    {
        void Apply(MapperOptions mapperOptions, IType type);
    }
}
