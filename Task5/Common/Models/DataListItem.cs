using System;

namespace Task5.Common.Models
{
    public class DataListItem : Person, IEquatable<DataListItem>
    {
        public DataListItem() { }

        public DataListItem(DataListItem item)
        {
            FirstName = item.FirstName;
            LastName = item.LastName;
            UsedFirstName = item.UsedFirstName;
            UsedLastName = item.UsedLastName;
        }

        public bool UsedFirstName { get; set; }
        public bool UsedLastName { get; set; }

        public bool Equals(DataListItem? other)
        {
            if(other is null)
                throw new ArgumentNullException(nameof(other));

            return base.Equals(other) && (UsedFirstName == other.UsedFirstName) && (UsedLastName == other.UsedLastName);
        }

        public override bool Equals(object? obj) => Equals(obj as DataListItem);

        public override int GetHashCode() => throw new NotImplementedException();
    }
}