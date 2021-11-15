using System;
using System.Collections;
using System.Collections.Generic;

namespace Task6
{
    public class CircleList<T> : List<T>, IList, IEnumerable<T>, ICollection<T>
    {
        public CircleList() : base() { }
        public CircleList(int capacity) : base(capacity) { }
        public CircleList(IEnumerable<T> collection) : base(collection) { }

        public new T this[int index]
        {
            get => base[(index < 0 ? System.Math.Abs(index) + 1 : index) % Count];
            set => base[(index < 0 ? System.Math.Abs(index) + 1 : index) % Count] = value;
        }
    }
    static class CircleListExtensions
    {
        public static CircleList<T> ToCircleList<T>(this IEnumerable<T> arr)
        {
            return new CircleList<T>(arr);
        }
    }
}
