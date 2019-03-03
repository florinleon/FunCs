/**************************************************************************
 *                                                                        *
 *  Description: FunCs Functional Programming Library                     *
 *  Website:     https://github.com/florinleon/FunCs                      *
 *                                                                        *
 *  This code was adapted after the following sources:                    *
 *  Zoran Horvat, Custom Implementation of the Option/Maybe Type in C#,   *
 *     http://codinghelmet.com/articles/custom-implementation-of-the-     *
 *        option-maybe-type-in-cs                                         *
 *  Enrico Buonanno, Functional Programming in C# - LaYumba Library,      *
 *     https://github.com/la-yumba/functional-csharp-code                 *
 *                                                                        *
 **************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FunCs
{
    /// <summary>
    /// The option type is used when an actual value may not exist. An option has an underlying type and can hold a value of that type, i.e. Some(value), or it may contain no value, i.e. None.
    /// </summary>
    public class OptionF<T> : IEnumerable<T>
    {
        private T[] _value;

        private OptionF(T[] value)
        {
            _value = value;
        }

        /// <summary>
        /// Creates an option that has a given value.
        /// </summary>
        /// <param name="value">A non-null value</param>
        public static OptionF<T> Some(T value)
        {
            if (value == null)
                throw new Exception("Cannot wrap a null value in a Some; use None instead.");

            return new OptionF<T>(new[] { value });
        }

        /// <summary>
        /// Creates an option with no value.
        /// </summary>
        public static OptionF<T> None()
        {
            return new OptionF<T>(new T[0]);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns true if the option has a value and false if it has no value.
        /// </summary>
        public bool IsSome
        {
            get
            {
                if (_value.Count() == 1)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Returns true if the option has no value and false if it has a value.
        /// </summary>
        public bool IsNone
        {
            get
            {
                if (_value.Count() == 0)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Returns the value of the option. It throws an exception if the option has no value.
        /// </summary>
        public T Value
        {
            get
            {
                if (_value.Count() == 0)
                    throw new Exception("Option has no Value; it is None.");
                else
                    return _value[0];
            }
        }

        /// <summary>
        /// Converts the option to a string representation: Some(value) or None.
        /// </summary>
        public override string ToString()
        {
            return IsSome ? $"Some({Value})" : "None";
        }

        /// <summary>
        /// Determines whether the specified option object is equal to the current option object.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is OptionF<T> other) || this.IsNone || other.IsNone)
                return false;

            return this.Value.Equals(other.Value);
        }

        /// <summary>
        /// Projects the current type of option into a new type of option. An equivalent name in other functional programming languages is Map.
        /// </summary>
        /// <typeparam name="R">The type of the option returned by the transform function.</typeparam>
        /// <param name="f">A transform function to apply to the current option.</param>
        public OptionF<R> Select<R>(Func<T, R> f)
        {
            if (this.IsNone)
                return OptionF<R>.None();
            else
                return OptionF<R>.Some(f(this.Value));
        }

        /// <summary>
        /// Projects the current type of option into a new type of option and flattens the result. An equivalent name in other functional programming languages is Bind.
        /// </summary>
        /// <typeparam name="R">The type of the option returned by the transform function.</typeparam>
        /// <param name="f">A transform function to apply to the current option.</param>
        public OptionF<R> SelectMany<R>(Func<T, OptionF<R>> f)
        {
            if (this.IsNone)
                return OptionF<R>.None();
            else
                return f(this.Value);
        }

        /// <summary>
        /// Returns the hash code of the current option object.
        /// </summary>
        public override int GetHashCode()
        {
            return -1939223833 + EqualityComparer<T[]>.Default.GetHashCode(_value);
        }
    }

    public static class OptionExtensionMethods
    {
        /// <summary>
        /// Filters a sequence of OptionF objects and returns the list of values of the objects which are Some.
        /// </summary>
        public static IEnumerable<T> WhereSome<T>(this IEnumerable<OptionF<T>> list)
        {
            return list.Where(a => a.IsSome).Select(a => a.Value);
        }

        /// <summary>
        /// Returns true if the option contains a value and false otherwise.
        /// </summary>
        /// <param name="some">The value of the option</param>
        public static bool MatchSomeF<T>(this OptionF<T> opt, out T some)
        {
            if (opt.IsSome)
            {
                some = opt.Value;
                return true;
            }
            else
            {
                some = default(T);
                return false;
            }
        }

        /// <summary>
        /// Returns true if the option does not contain a value and false otherwise.
        /// </summary>
        public static bool MatchNoneF<T>(this OptionF<T> opt)
        {
            return opt.IsNone;
        }
    }
}