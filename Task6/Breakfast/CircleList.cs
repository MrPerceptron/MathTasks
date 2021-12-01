using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Task6
{
    public class CircleList<T> : List<T>, IEnumerable, ICloneable, IEquatable<CircleList<T>> where T : ICloneable, IEquatable<T>
    {
        public CircleList() { }
        public CircleList(int capacity) : base(capacity) { }
        public CircleList(IEnumerable<T> collection) : base(collection) { }

        public new T this[int index]
        {
            get => base[index < 0 ? index % Count + Count : index % Count];
            set => base[index < 0 ? index % Count + Count : index % Count] = value;
        }

        public CircleList<T> GenericClone() => (CircleList<T>)this.Clone();
        public object Clone() => new CircleList<T>(this.Select(x => (T)x.Clone()));

        public bool Equals(CircleList<T> other)
        {
            if (this.Count != other.Count || (other == null && this != null))
                return false;

            for (int i = 0; i < this.Count; i++)
            {
                if (!this[i].Equals(other[i]))
                    return false;
            }

            return true;
        }
    }
    static class CircleListExtensions
    {
        public static CircleList<T> ToCircleList<T>(this IEnumerable<T> arr) where T : ICloneable, IEquatable<T>
        {
            return new CircleList<T>(arr);
        }
    }
}
