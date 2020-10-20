using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Detached.Mappers.Model.Types.Class
{
    public class ClassMemberOptionsCollection : KeyedCollection<string, ClassMemberOptions>
    {
        protected override string GetKeyForItem(ClassMemberOptions item) => item.Name;

        public IEnumerable<string> Keys => Dictionary?.Keys;

        public bool TryGetValue(string memberName, out ClassMemberOptions memberOptions)
        {
            if (Dictionary == null)
            {
                memberOptions = null;
                return false;
            }
            else
            {
                return Dictionary.TryGetValue(memberName, out memberOptions);
            }
        }
    }
}