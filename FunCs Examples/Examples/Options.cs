/**************************************************************************
 *                                                                        *
 *  Description: FunCs Examples                                           *
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
using System.Linq;
using FunCs;
using static System.Console;
using static System.Linq.Enumerable;

namespace FunCsExamples
{
    public class Options
    {
        public static void Ex1SomeNone()
        {
            var rez = ReadNatural();
            if (rez.IsSome)
                WriteLine($"Some: {rez.Value}");
            if (rez.IsNone)
                WriteLine("None");
        }

        public static void Ex2List()
        {
            var l1 = "[ 1 2 2.5 abc -5 14 ]".ToStringEnumF()
                .Select(x => ToNatural(x))
                .Where(x => x.IsSome)
                .Select(x => x.Value);

            WriteLine(l1.ToStringF());

            var o1 = ToNatural("1");
            WriteLine(o1);

            var o2 = ToNatural("abc");
            WriteLine(o2);

            var o3 = OptionF<OptionF<int>>.Some(o1);
            WriteLine(o3);

            var o4 = o3.SelectMany((OptionF<int> x) => x);
            WriteLine(o4.ToStringF()); // [ 1 ]

            var l2 = new OptionF<int>[] { o1, o2, o1, o2 }.AsEnumerable();
            var l3 = l2.Where(x => x.IsSome).Select(x => x.Value); // [ 1 1 ]

            WriteLine(l3.ToStringF());
        }

        public static void Ex3Select()
        {
            var os1 = StringNotNumber("abc");
            var l = os1.Select(x => x.Length);
            WriteLine($"{os1} -> {l}");

            os1 = StringNotNumber("123");
            l = os1.Select(x => x.Length);
            WriteLine($"{os1} -> {l}");
        }

        private static OptionF<int> ReadNatural()
        {
            Write("Input a positive integer: ");
            var s = ReadLine();
            return ToNatural(s);
        }

        private static OptionF<string> StringNotNumber(string s)
        {
            if (double.TryParse(s, out double d))
                return OptionF<string>.None();
            else
                return OptionF<string>.Some(s);
        }

        private static OptionF<int> ToNatural(string s)
        {
            bool ok = Int32.TryParse(s, out int result);
            if (!ok)
                return OptionF<int>.None();
            else if (result >= 0)
                return OptionF<int>.Some(result);
            else
                return OptionF<int>.None();
        }
    }
}