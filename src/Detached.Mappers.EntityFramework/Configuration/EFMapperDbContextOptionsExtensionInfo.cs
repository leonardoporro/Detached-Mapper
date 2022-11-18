using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;

namespace Detached.Mappers.EntityFramework.Configuration
{
    public class EFMapperDbContextOptionsExtensionInfo : DbContextOptionsExtensionInfo
    {
        public EFMapperDbContextOptionsExtensionInfo(IDbContextOptionsExtension extension)
            : base(extension)
        {
        }

        public override bool IsDatabaseProvider => false;

        public override string LogFragment => "Detached";

        public override int GetServiceProviderHashCode() => 0;

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {

        }

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other) => true;
    }
}