using System;
using static Task5.Person;

namespace Task5
{
    public class Person
    {
        public enum Name
        {
            FirstName,
            LastName
        }

        public (Name key, string value) FirstName { get; private set; }
        public (Name key, string value) LastName { get; private set; }
        
        public void SetFirstName(string firstName) => FirstName = (Name.FirstName, firstName);
        public void SetLastName(string lastName) => LastName = (Name.LastName, lastName);
    }
    static public class PersonExtension
    {
        static public Person AddFirstName(this Person person, string firstName)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));

            if (firstName == null)
                throw new ArgumentNullException(nameof(firstName));

            person.SetFirstName(firstName);
            return person;
        }
        static public Person AddLastName(this Person person, string lastName)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));

            if (lastName == null)
                throw new ArgumentNullException(nameof(lastName));

            person.SetLastName(lastName);
            return person;
        }
        static public Person AddName(this Person person, string name)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (person.FirstName != default) // default вряд ли тут подойдёт и нужно всё правильно сделать тут
                return person.AddLastName(name);
            else
                return person.AddFirstName(name);

        }
    }
}
