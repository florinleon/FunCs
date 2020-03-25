using System;
using System.Collections.Generic;
using System.Linq;

namespace FunCs
{
    public static class LinqExtensions
    {
        /// <summary>
        /// Projects each element of a sequence into a new form (Linq Select).
        /// </summary>
        /// <returns></returns>
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
    }
}