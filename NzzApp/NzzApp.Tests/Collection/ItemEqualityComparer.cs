using System.Collections.Generic;

namespace NzzApp.Tests.Collection
{
    public class ItemEqualityComparer : IEqualityComparer<Item>
    {
        public bool Equals(Item x, Item y)
        {
            if (x.Sort.HasValue && y.Sort.HasValue)
            {
                return x.Name.Equals(y.Name) && x.Sort.Value == y.Sort.Value;
            }
            return x.Name.Equals(y.Name);
        }

        public int GetHashCode(Item obj)
        {
            return obj.GetHashCode();
        }
    }
}
