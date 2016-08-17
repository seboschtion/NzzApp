using System;

namespace NzzApp.Tests.Collection
{
    public class Item : IComparable<Item>
    {
        public Item(string name)
        {
            Name = name;
        }

        public Item(string name, int? sort)
        {
            Name = name;
            Sort = sort;
        }

        public string Name { get; set; }
        public int? Sort { get; set; }
        public int CompareTo(Item other)
        {
            if (Sort.HasValue && other.Sort.HasValue)
            {
                if (Sort.Value - other.Sort.Value == 0)
                {
                    return other.Name.CompareTo(Name);
                }
                return other.Sort.Value - Sort.Value;
            }
            return other.Name.CompareTo(Name);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}