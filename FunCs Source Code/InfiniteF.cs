using System;
using System.Collections.Generic;

namespace FunCs
{
    public class InfiniteF
    {
        /// <summary>
        /// Defines an infinite sequence based on a function that generates each element from the index of the element.
        /// Using lazy evaluation, elements are created when needed for precessing.
        /// </summary>
        public static IEnumerable<T> Define<T>(Func<int, T> f)
        {
            int n = 0;
            while (true)
                yield return f(n++);
        }
    }
}