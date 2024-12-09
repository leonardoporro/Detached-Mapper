using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Detached.Model.EntityFramework.Options
{
    public class ModelDbContextOptionsExtensionInfo : DbContextOptionsExtensionInfo
    {
        public ModelDbContextOptionsExtensionInfo(ModelDbContextOptionsExtension extension)
            : base(extension)
        {
            TypedExtension = extension;
        }

        public override bool IsDatabaseProvider => false;

        public override string LogFragment => "SailMapper";

        public ModelDbContextOptionsExtension TypedExtension { get; }

        public override int GetServiceProviderHashCode() => 0;

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {

        }

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other) => true;
    }
}