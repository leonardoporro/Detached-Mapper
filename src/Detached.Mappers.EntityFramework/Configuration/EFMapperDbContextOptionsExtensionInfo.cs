using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;

namespace Detached.Mappers.EntityFramework.Configuration
{
    public class EFMapperDbContextOptionsExtensionInfo : DbContextOptionsExtensionInfo
    {
        public EFMapperDbContextOptionsExtensionInfo(EFMapperDbContextOptionsExtension extension)
            : base(extension)
        {
            TypedExtension = extension;
        }

        public override bool IsDatabaseProvider => false;

        public override string LogFragment => "Detached.Mappers.EntityFramework";

        public EFMapperDbContextOptionsExtension TypedExtension { get; }

        public override int GetServiceProviderHashCode()
        {
            return TypedExtension.Configure?.GetHashCode() ?? 0;
        }

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {

        }

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
        {
            return other is EFMapperDbContextOptionsExtensionInfo otherExtInfo
                && TypedExtension.Configure == otherExtInfo.TypedExtension.Configure;
        }
    }
}