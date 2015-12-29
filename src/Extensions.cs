using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Centipede
{
    static class Extensions
    {
        public static int AddSorted<T>(this List<T> list, T item)
            where T : IComparable<T>
        {
            return list.AddSorted(item, Comparer<T>.Default);
        }

        public static int AddSorted<T>(this List<T> list, T item, IComparer<T> comparer)
        {
            if (list.Count == 0 || comparer.Compare(list[0], item) >= 0)
            {
                list.Insert(0, item);
                return 0;
            }

            var index = list.Count - 1;
            if (comparer.Compare(list[index], item) <= 0)
            {
                list.Add(item);
            }
            else
            {
                index = list.BinarySearch(item, comparer);
                if (index < 0)
                    index = ~index;

                list.Insert(index, item);
            }
            return index;
        }
    }
}