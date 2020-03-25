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
using System.Collections.Generic;
using System.Linq;
using FunCs;
using static System.Console;
using static System.Linq.Enumerable;

namespace FunCsExamples
{
    public class MatchList
    {
        public static void Ex1()
        {
            ExpertMatchF em = new ExpertMatchF("[ ]"); // equivalent to checking whether Count == 0

            if (em.MatchListEmpty())
                Console.WriteLine("Empty");

            WriteLine("\r\n");

            var list = "1 2 3 4 5 6 7 8 9 10";

            em = new ExpertMatchF(list);

            if (em.MatchListHeadTail(out string head, out List<string> tail))
            {
                WriteLine(head);
                var cc = tail.Map(x => Convert.ToInt32(x));
                WriteLine(cc.ToStringF());
            }

            WriteLine("\r\n");

            if (em.MatchListGeneral("[ ?head ? $?rest ? ?last ]", out var matches))
            {
                WriteLine(matches["?head"]);
                var restList = matches["$?rest"].Split().Map(x => Convert.ToInt32(x));
                WriteLine(restList.ToStringF());
                WriteLine(matches["?last"]);
            }

            WriteLine("\r\nMatchListGeneral\r\n");

            if (em.MatchListGeneral("[ $? ?x ?y $? ]", out matches))
                WriteLine($"{matches["?x"]} {matches["?y"]}");

            WriteLine("\r\nMatchMultiple\r\n");

            if (em.MatchMultiple(new List<string> { "$? ?x $? ?y $?" }, out var matchList)) // do not use "[" and "]"
            {
                // Combinations(10,2)
                foreach (var m in matchList)
                    Write($"{m["?x"]}-{m["?y"]} ");
            }

            WriteLine();
        }

        public static void Ex2ExtensionMethods()
        {
            var list = "[ 1 2 3 4 5 6 7 8 9 10 ]".ToIntEnumF();

            if (list.MatchF(out var hd, out var tail))
            {
                WriteLine(hd);
                WriteLine(tail.ToStringF());
            }

            WriteLine("\r\n");

            var listS = "[ 1 2 3 4 5 6 7 8 9 10 ]".ToStringEnumF();

            if (listS.MatchF("[ ?head ? $?rest ? ?last ]", out var head, out var rest, out var last))
            {
                WriteLine(head);
                WriteLine(rest.Split().Map(a => Convert.ToInt32(a)).ToStringF());
                WriteLine(last);
            }

            WriteLine("\r\n");

            if (listS.MatchF("[ $? ?x ?y $? ]", out var x, out var y))
                WriteLine($"{x} {y}");
        }
    }
}