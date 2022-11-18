using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Detached.Mappers.TypeOptions.Class
{
    public class ClassMemberOptionsCollection : KeyedCollection<string, ClassMemberOptions>
    {
        protected override string GetKeyForItem(ClassMemberOptions item) => item.Name;

        public IEnumerable<string> Keys => Dictionary?.Keys;

        public bool TryGetValue(string key, out ClassMemberOptions options)
        {
            if (Items.Count == 0)
            {
                options = null;
                return false;
            }
            else
            {
                return Dictionary.TryGetValue(key, out options);
            }
        }
    }
}