using System.Collections;
using System.Collections.Generic;

namespace Task6
{
    public class CircleList<T> : List<T>, IList
    {
        public CircleList() : base() { }
        public CircleList(int capacity) : base(capacity) { }
        public CircleList(IEnumerable<T> collection) : base(collection) { }

        public new T this[int index]
        {
            get => base[index % Count];
            set => base[index % Count] = value;
        }
    }
}
