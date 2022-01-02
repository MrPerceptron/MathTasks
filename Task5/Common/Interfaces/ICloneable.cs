using System;

namespace Task5.Common.Interfaces
{
    public interface ICloneable<T> : ICloneable
    {
        public T CloneInstance()
        {
            return (T)Clone();
        }
    }

    public static class CloneableExtension
    {
        public static T CloneInstance<T>(this T obj) where T : ICloneable<T>
        {
            return obj.CloneInstance();
        }
    }
}
