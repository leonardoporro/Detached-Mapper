using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Runtime;
using System.Reflection;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using Detached.EntityFramework.Services;
using Microsoft.EntityFrameworkCore.Storage;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Detached.EntityFramework.Plugins.Seeding
{
    public static class DetachedContextExtensions
    {
        public static async Task SeedFromJsonFileAsync(this IDetachedContext context, string filePath, bool identityInsert = true)
        {
            string json = File.ReadAllText(filePath);
            JObject jsonRoot = (JObject)JsonConvert.DeserializeObject(json);

            foreach (var jsonCatalog in jsonRoot.Properties())
            {
                IDetachedSet set = context.Set(jsonCatalog.Name);

                // insert objects (detached roots), one by one.
                foreach (var jsonEntity in jsonCatalog.Values())
                {
                    object entity = jsonEntity.ToObject(set.EntityType);
                    await set.UpdateAsync(entity);
                }

                IEntityType entityType = context.DbContext.Model.FindEntityType(set.EntityType);
                if (identityInsert && HasIdentity(entityType))
                {
                    string tableName = entityType.Relational().TableName; // SQL table name.

                    // enable identity insert.
                    await context.DbContext.Database.OpenConnectionAsync();
                    await context.DbContext.Database.ExecuteSqlCommandAsync($"SET IDENTITY_INSERT [dbo].[{tableName}] ON;");
                    
                    // save changes with identity insert enabled.
                    await context.SaveChangesAsync();

                    await context.DbContext.Database.ExecuteSqlCommandAsync($"SET IDENTITY_INSERT [dbo].[{tableName}] OFF;");
                    context.DbContext.Database.CloseConnection();
                }
                else
                {
                    await context.SaveChangesAsync();
                }
            }
        }
        
        // do not try to enable identity for a table that does not support it.
        static bool HasIdentity(IEntityType entityType)
        {
            return entityType.FindPrimaryKey().Properties.Any(p => p.ValueGenerated != ValueGenerated.Never);
        }
    }
}