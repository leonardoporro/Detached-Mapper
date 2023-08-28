using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;

namespace Detached.Mappers.EntityFramework.Features
{
    public class EntityMapperDbContextOptionsExtensionInfo : DbContextOptionsExtensionInfo
    {
        public EntityMapperDbContextOptionsExtensionInfo(EntityMapperDbContextOptionsExtension extension)
            : base(extension)
        {
            TypedExtension = extension;
        }

        public override bool IsDatabaseProvider => false;

        public override string LogFragment => "Detached.Mappers.EntityFramework";

        public EntityMapperDbContextOptionsExtension TypedExtension { get; }

        public override int GetServiceProviderHashCode() => 0;

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {

        }

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other) => true;
    }
}