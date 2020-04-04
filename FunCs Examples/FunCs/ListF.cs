/**************************************************************************
 *                                                                        *
 *  Description: FunCs Functional Programming Library                     *
 *  Website:     https://github.com/florinleon/FunCs                      *
 *  Copyright:   (c) 2019, Florin Leon                                    *
 *                                                                        *
 *  This code and information is provided "as is" without warranty of     *
 *  any kind, either expressed or implied, including but not limited      *
 *  to the implied warranties of merchantability or fitness for a         *
 *  particular purpose. You are free to use this source code in your      *
 *  applications as long as the original copyright notice is included.    *
 *                                                                        *
 **************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FunCs
{
    /// <summary>
    /// The class with extension methods for string and IEnumerable.
    /// </summary>
    public static class ListF
    {
        /// <summary>
        /// Converts a string that represents a list of integers into the corresponding IEnumerable(int).
        /// </summary>
        public static IEnumerable<int> ToIntEnumF(this string listString)
        {
            string[] toks = RemoveBrackets(listString).Split(" \t,;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            List<int> list = new List<int>();

            for (int i = 0; i < toks.Length; i++)
                list.Add(Convert.ToInt32(toks[i]));

            return list.AsEnumerable<int>();
        }

        /// <summary>
        /// Converts a string that represents a list of real numbers into the corresponding IEnumerable(double).
        /// </summary>
        public static IEnumerable<double> ToDoubleEnumF(this string listString)
        {
            string[] toks = RemoveBrackets(listString).Split(" \t,;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            List<double> list = new List<double>();

            for (int i = 0; i < toks.Length; i++)
                list.Add(Convert.ToDouble(toks[i]));

            return list.AsEnumerable<double>();
        }

        /// <summary>
        /// Converts a string that represents a list of strings into the corresponding IEnumerable(string).
        /// </summary>
        /// <param name="separator">A separator used to split the list.</param>
        public static IEnumerable<string> ToStringEnumF(this string listString, char separator = ' ')
        {
            string[] toks = RemoveBrackets(listString).Split(separator);
            return toks.AsEnumerable<string>().Where(a => a != "");
        }

        private static string RemoveBrackets(string list)
        {
            list = list.Trim();
            int length = list.Length;

            if (length <= 1)
                return list;

            if (length > 1 && list[0] == '[' && list[length - 1] == ']')
                return list.Substring(1, length - 2);
            else
                return list;
        }

        /// <summary>
        /// Converts the collection of integers to a string representation.
        /// </summary>
        public static string ToStringF(this IEnumerable<int> en)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[ ");
            foreach (int i in en)
                sb.Append($"{i} ");
            sb.Append("]");
            return sb.ToString();
        }

        /// <summary>
        /// Converts the collection of doubles to a string representation.
        /// </summary>
        /// <param name="noDecimals">The number of decimals places to be used when formatting the collection elements.</param>
        public static string ToStringF(this IEnumerable<double> en, int noDecimals = 3)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[ ");
            foreach (double d in en)
                sb.Append(string.Format(new NumberFormatInfo() { NumberDecimalDigits = noDecimals }, "{0:F} ", d));
            sb.Append("]");
            return sb.ToString();
        }

        /// <summary>
        /// Converts the collection of strings to a string representation.
        /// </summary>
        public static string ToStringF(this IEnumerable<string> en)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[ ");
            foreach (string s in en)
                sb.Append($"\"{s}\" ");
            sb.Append("]");
            return sb.ToString();
        }

        /// <summary>
        /// Identifies the first item of the list and the rest of the list.
        /// </summary>
        /// <param name="head">The head of the list, i.e. the first item.</param>
        /// <param name="tail">The rest of the list, starting with the second item.</param>
        public static void MatchHeadTailF<T>(this IEnumerable<T> en, out T head, out IEnumerable<T> tail)
        {
            head = en.First();
            tail = en.Skip(1);
        }

        /// <summary>
        /// Matches an arbitrary pattern on the list of facts. If the pattern cannot be matched, an empty dictionary (results) is returned.
        /// </summary>
        /// <param name="pattern">A pattern containing items to be matched and variables, identified by ? for a single word or $? for multiple words, e.g. ?a or $?b. At least one variable must be named in the pattern.</param>
        /// <param name="results">A dictionary that contains the values of the variables, e.g. results["?a"] contains the value of that variable after the pattern matching.</param>
        public static void MatchF(this IEnumerable<string> en, string pattern, out Dictionary<string, string> results)
        {
            var s = en.Aggregate("", (acc, val) => $"{acc} {val}");
            var em = new ExpertMatchF(s);
            em.Match(pattern, out results);
        }

        /// <summary>
        /// Matches an arbitrary pattern on the list. If the pattern cannot be matched, the returned var1 will be an empty string.
        /// </summary>
        /// <param name="pattern">A pattern with one variable.</param>
        /// <param name="var1">The value of the first variable after pattern matching.</param>
        public static void MatchF(this IEnumerable<string> en, string pattern, out string var1)
        {
            var em = new ExpertMatchF(en.Aggregate("", (acc, val) => $"{acc} {val}"));
            em.MatchVar(pattern, out var1);
        }

        /// <summary>
        /// Matches an arbitrary pattern on the list. The var_x parameters must be in the order of their appearence in the pattern.
        /// If the pattern cannot be matched, the returned var_x will be empty strings.
        /// </summary>
        /// <param name="pattern">A pattern with two variables.</param>
        /// <param name="var1">The value of the first variable after pattern matching.</param>
        /// <param name="var2">The value of the second variable after pattern matching.</param>
        public static void MatchF(this IEnumerable<string> en, string pattern, out string var1, out string var2)
        {
            var em = new ExpertMatchF(en.Aggregate("", (acc, val) => $"{acc} {val}"));
            em.MatchVar(pattern, out var1, out var2);
        }

        /// <summary>
        /// Matches an arbitrary pattern on the list. The var_x parameters must be in the order of their appearence in the pattern.
        /// If the pattern cannot be matched, the returned var_x will be empty strings.
        /// </summary>
        /// <param name="pattern">A pattern with three variables.</param>
        /// <param name="var1">The value of the first variable after pattern matching.</param>
        /// <param name="var2">The value of the second variable after pattern matching.</param>
        /// <param name="var3">The value of the third variable after pattern matching.</param>
        public static void MatchF(this IEnumerable<string> en, string pattern, out string var1, out string var2, out string var3)
        {
            var em = new ExpertMatchF(en.Aggregate("", (acc, val) => $"{acc} {val}"));
            em.MatchVar(pattern, out var1, out var2, out var3);
        }

        /// <summary>
        /// Matches an arbitrary pattern on the list. The var_x parameters must be in the order of their appearence in the pattern.
        /// If the pattern cannot be matched, the returned var_x will be empty strings.
        /// </summary>
        /// <param name="pattern">A pattern with four variables.</param>
        /// <param name="var1">The value of the first variable after pattern matching.</param>
        /// <param name="var2">The value of the second variable after pattern matching.</param>
        /// <param name="var3">The value of the third variable after pattern matching.</param>
        /// <param name="var4">The value of the fourth variable after pattern matching.</param>
        public static void MatchF(this IEnumerable<string> en, string pattern, out string var1, out string var2, out string var3, out string var4)
        {
            var em = new ExpertMatchF(en.Aggregate("", (acc, val) => $"{acc} {val}"));
            em.MatchVar(pattern, out var1, out var2, out var3, out var4);
        }
    }
}