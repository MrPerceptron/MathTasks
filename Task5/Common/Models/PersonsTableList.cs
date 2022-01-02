using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Task5.Common.Models
{
    public class PersonsTableList : IEnumerable<DataListItem>, ICloneable
    {
        private readonly IList<DataListItem> _persons;

        public PersonsTableList()
        {
            _persons = new List<DataListItem>();
        }

        public PersonsTableList(int capacity)
        {
            _persons = new List<DataListItem>(capacity);
        }

        public int Length => _persons.Count;

        public bool IsTableListFilled => _persons.All(x => x.UsedFirstName && x.UsedLastName);

        public bool LastNameEqualAvailable => _persons.All(x => x.UsedLastName)
            && _persons.Count(x => x.UsedFirstName) == _persons.Count - 1;

        public void Add(DataListItem item) => _persons.Add(item);

        public DataListItem this[int index]
        {
            get => _persons[index < 0 ? (index % _persons.Count + _persons.Count) % _persons.Count : index % _persons.Count];
            set => _persons[index < 0 ? (index % _persons.Count + _persons.Count) % _persons.Count : index % _persons.Count] = value;
        }

        public IEnumerator<DataListItem> GetEnumerator()
        {
            return _persons.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public object Clone()
        {
            PersonsTableList list = new();
            foreach (var item in this)
                list.Add(new DataListItem(item));
            return list;
        }
    }
}