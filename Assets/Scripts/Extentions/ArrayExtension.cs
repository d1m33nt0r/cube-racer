using System;

namespace DefaultNamespace.Extentions
{
    public static class ArrayExtensions
    {
        public static void Add<T>(ref T[] array, T t)
        {
            int newSize = array.Length + 1;
            Array.Resize(ref array, newSize);
            array[newSize - 1] = t;
        }
    }
}