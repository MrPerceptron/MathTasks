using System;

namespace Task6
{
    public class Person : ICloneable, IEquatable<Person>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public object Clone() => new Person() { FirstName = FirstName, LastName = LastName };

        public bool Equals(Person other) => this.FirstName == other.FirstName && this.LastName == other.LastName;
    }
}
