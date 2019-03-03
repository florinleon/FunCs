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

using FunCs;
using static System.Console;

namespace FunCsExamples
{
    public class MatchOption
    {
        private class Age
        {
            private int Value { get; }

            private Age(int age)
            {
                Value = age;
            }

            public static OptionF<Age> FromInt(int age)
            {
                if (age > 0 && age < 120)
                    return OptionF<Age>.Some(new Age(age));
                else
                    return OptionF<Age>.None();
            }

            public override string ToString()
            {
                return Value.ToString();
            }
        }

        public static void Ex1()
        {
            var a1 = Age.FromInt(10);
            var a2 = Age.FromInt(1000);

            WriteLine(a1);
            WriteLine(a2);

            var em = new ExpertMatchF<Age>(a1);
            if (em.MatchOptionSome(out Age a))
                WriteLine($"Is some: {a}");

            em = new ExpertMatchF<Age>(a2);
            if (em.MatchOptionNone())
                WriteLine("Is none");
        }

        public static void Ex2ExtensionMethod()
        {
            var a1 = Age.FromInt(10);
            var a2 = Age.FromInt(1000);

            WriteLine(a1);
            WriteLine(a2);

            if (a1.MatchSomeF<Age>(out var a))
                WriteLine($"Is some: {a}");

            if (a2.MatchNoneF<Age>())
                WriteLine("Is none");
        }
    }
}