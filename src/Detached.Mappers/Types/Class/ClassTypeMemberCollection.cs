using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Detached.Mappers.Types.Class
{
    public class ClassTypeMemberCollection : KeyedCollection<string, ClassTypeMember>
    {
        protected override string GetKeyForItem(ClassTypeMember item) => item.Name;

        public IEnumerable<string> Keys => Dictionary?.Keys;
    }
}