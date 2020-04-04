using System;
using System.Collections.Generic;

namespace FunCs
{
    /// <summary>
    /// The class with a static method that defines an infinite sequence.
    /// </summary>
    public class InfiniteF
    {
        /// <summary>
        /// Defines an infinite sequence based on a function that generates each element from the index of the element.
        /// Using lazy evaluation, elements are created when needed for processing.
        /// </summary>
        public static IEnumerable<T> Define<T>(Func<int, T> f)
        {
            int n = 0;
            while (true)
                yield return f(n++);
        }
    }
}