using System;
using System.Collections.Generic;
using System.Linq;

namespace FunCs
{
    /// <summary>
    /// The class with Linq extensions such as Map, Reduce, Bind, Filter, Find and FindIndex.
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Projects each element of a sequence into a new form (Linq Select).
        /// </summary>
        public static IEnumerable<TResult> Map<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return source.Select(selector);
        }

        /// <summary>
        /// Projects each element of a sequence into a new form by incorporating the element's index (Linq Select).
        /// </summary>
        public static IEnumerable<TResult> Map<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector)
        {
            return source.Select(selector);
        }

        /// <summary>
        /// Projects each element of a sequence to an IEnumerable and flattens the resulting sequences into one sequence (Linq SelectMany).
        /// </summary>
        public static IEnumerable<TResult> Bind<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
        {
            return source.SelectMany(selector);
        }

        /// <summary>
        /// Projects each element of a sequence to an IEnumerable, and flattens the resulting sequences into one sequence. The index of each source element is used in the projected form of that element (Linq SelectMany).
        /// </summary>
        public static IEnumerable<TResult> Bind<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector)
        {
            return source.SelectMany(selector);
        }

        /// <summary>
        /// Projects each element of a sequence to an IEnumerable, flattens the resulting sequences into one sequence, and invokes a result selector function on each element therein (Linq SelectMany).
        /// </summary>
        public static IEnumerable<TResult> Bind<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            return source.SelectMany(collectionSelector, resultSelector);
        }

        /// <summary>
        /// Projects each element of a sequence to an IEnumerable, flattens the resulting sequences into one sequence, and invokes a result selector function on each element therein. The index of each source element is used in the intermediate projected form of that element (Linq SelectMany).
        /// </summary>
        public static IEnumerable<TResult> Bind<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            return source.SelectMany(collectionSelector, resultSelector);
        }

        /// <summary>
        /// Applies an accumulator function over a sequence (Linq Aggregate).
        /// </summary>
        public static TSource Reduce<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> func)
        {
            return source.Aggregate(func);
        }

        /// <summary>
        /// Applies an accumulator function over a sequence. The specified seed value is used as the initial accumulator value (Linq Aggregate).
        /// </summary>
        public static TAccumulate Reduce<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
        {
            return source.Aggregate(seed, func);
        }

        /// <summary>
        /// Applies an accumulator function over a sequence. The specified seed value is used as the initial accumulator value, and the specified function is used to select the result value (Linq Aggregate).
        /// </summary>
        public static TResult Reduce<TSource, TAccumulate, TResult>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
        {
            return source.Aggregate(seed, func, resultSelector);
        }

        /// <summary>
        /// Filters a sequence of values based on a predicate (Linq Where).
        /// </summary>
        public static IEnumerable<TSource> Filter<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return source.Where(predicate);
        }

        /// <summary>
        /// Filters a sequence of values based on a predicate. Each element's index is used in the logic of the predicate function (Linq Where).
        /// </summary>
        public static IEnumerable<TSource> Filter<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            return source.Where(predicate);
        }

        /// <summary>
        /// Searches for an element and returns the zero-based index of the first occurrence within the collection.
        /// It returns -1 if no such element exists.
        /// </summary>
        public static int FindIndex<T>(this IEnumerable<T> source, T item)
        {
            if (!source.Contains(item))
                return -1;

            var array = source.ToArray();
            for (int i = 0; i < array.Length; i++)
                if (array[i].Equals(item))
                    return i;

            return -1;
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the collection.
        /// It returns -1 if no such element exists.
        /// </summary>
        public static int FindIndex<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            var array = source.ToArray();
            for (int i = 0; i < array.Length; i++)
                if (predicate(array[i]))
                    return i;

            return -1;
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the first occurrence within the collection.
        /// It throws an exception if no such element exists.
        /// </summary>
        public static T Find<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            var array = source.ToArray();
            for (int i = 0; i < array.Length; i++)
                if (predicate(array[i]))
                    return array[i];

            throw new Exception("Item not found.");
        }

        /// <summary>
        /// Composes the first function f with the second function g: f.To(g) returns a new function h(x) = g(f(x)).
        /// </summary>
        public static Func<T1, T3> To<T1, T2, T3>(this Func<T1, T2> f, Func<T2, T3> g)
        {
            return x => g(f(x));
        }
    }
}