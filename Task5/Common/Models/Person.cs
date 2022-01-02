using System;
using Task5.Common.Interfaces;

namespace Task5.Common.Models
{
    public class Person : ICloneable<Person>, IEquatable<Person>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public object Clone()
        {
            return new Person() { FirstName = FirstName, LastName = LastName };
        }

        public bool Equals(Person? other)
        {
            if (other is null)
                return false;

            return (FirstName == other.FirstName) && (LastName == other.LastName);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Person);
        }

        public override int GetHashCode() => throw new NotImplementedException();
    }
}