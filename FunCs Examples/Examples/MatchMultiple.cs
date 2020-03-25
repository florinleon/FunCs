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
using System.IO;
using System.Linq;
using FunCs;
using static System.Console;
using static System.Linq.Enumerable;

namespace FunCsExamples
{
    public class MatchMultiple
    {
        public static void Ex1Kinship()
        {
            // parents and children

            var sr = new StreamReader("kinship.csv");
            var lines = sr.ReadToEnd().Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            sr.Close();

            var em = new ExpertMatchF(lines.ToList<string>());

            var patterns = new List<string>
            {
                "father ?f ?c",
                "mother ?m ?c"
            };

            if (em.MatchMultiple(patterns, out var matches))
            {
                foreach (var m in matches)
                    WriteLine($"Father {m["?f"]}, mother {m["?m"]}, child {m["?c"]}");
            }

            WriteLine("\r\n=========================\r\n");

            // grandfathers and grandchildren

            patterns.Clear();
            patterns.Add("father ?g ?p");
            patterns.Add("?r ?p ?c");

            var constraint = "(or (eq ?r father) (eq ?r mother))";

            if (em.MatchMultiple(patterns, constraint, out matches))
            {
                foreach (var m in matches)
                    WriteLine($"Grandfather {m["?g"]}, {m["?r"]} {m["?p"]}, child {m["?c"]}");
            }
        }

        public static void Ex2Cousins()
        {
            var facts = new List<string>
            {
                "parent Liviu children Vlad Maria Nelu",
                "parent Vlad children Oana Mircea",
                "parent Maria children Dan",
                "parent Nelu children Paul Radu Sorin"
            };

            var em = new ExpertMatchF(facts);

            string searchFor = "Oana"; // search for the cousins of Oana

            List<string> patterns = new List<string>
            {
                $"parent ?p1 children $? {searchFor} $?",
                 "parent ?g children $? ?p1 $?",
                 "parent ?g children $? ?p2 $?",
                 "parent ?p2 children $? ?c $?"
            };

            string constraint = "(neq ?p1 ?p2)"; // not equal

            if (em.MatchMultiple(patterns, constraint, out var matches))
            {
                foreach (var m in matches)
                    Write($"{m["?c"]} ");
            }

            WriteLine();
        }

        public static void Ex3Queens4()
        {
            var em = new ExpertMatchF("n 1 2 3 4");

            var patterns = new List<string>
            {
                "n $? ?row $?",
                "n $? ?col $?"
            };

            var queens = new List<string>();

            if (em.MatchMultiple(patterns, out var matchList))
            {
                foreach (var m in matchList)
                    queens.Add($"queen {m["?row"]} {m["?col"]}");
            }

            em = new ExpertMatchF(queens);

            patterns = new List<string>
            {
                "queen 1 ?c1",
                "queen 2 ?c2",
                "queen 3 ?c3",
                "queen 4 ?c4"
            };

            var constraints = "(and " +
                "(neq (abs (- ?c1 ?c2)) 1)" +
                "(neq (abs (- ?c1 ?c3)) 2)" +
                "(neq (abs (- ?c1 ?c4)) 3)" +
                "(neq (abs (- ?c2 ?c3)) 1)" +
                "(neq (abs (- ?c2 ?c4)) 2)" +
                "(neq (abs (- ?c3 ?c4)) 1)" +
                "(neq ?c1 ?c2)" +
                "(neq ?c1 ?c3)" +
                "(neq ?c1 ?c4)" +
                "(neq ?c2 ?c3)" +
                "(neq ?c2 ?c4)" +
                "(neq ?c3 ?c4)" +
                ")";

            if (em.MatchMultiple(patterns, constraints, out matchList))
            {
                foreach (var m in matchList)
                    WriteLine($"{m["?c1"]} {m["?c2"]} {m["?c3"]} {m["?c4"]}");
            }
        }

        public static void Ex4Queens8()
        {
            var em = new ExpertMatchF("n 1 2 3 4 5 6 7 8");

            var patterns = new List<string>
            {
                "n $? ?row $?",
                "n $? ?col $?"
            };

            var queens = new List<string>();

            if (em.MatchMultiple(patterns, out var matchList))
            {
                foreach (var m in matchList)
                    queens.Add($"queen {m["?row"]} {m["?col"]}");
            }

            em = new ExpertMatchF(queens);

            patterns = new List<string>
            {
                "queen 1 ?c1",
                "queen 2 ?c2",
                "queen 3 ?c3",
                "queen 4 ?c4",
                "queen 5 ?c5",
                "queen 6 ?c6",
                "queen 7 ?c7",
                "queen 8 ?c8"
            };

            var constraints = "(and " +
                "(neq (abs (- ?c1 ?c2)) 1)" +
                "(neq (abs (- ?c1 ?c3)) 2)" +
                "(neq (abs (- ?c1 ?c4)) 3)" +
                "(neq (abs (- ?c1 ?c5)) 4)" +
                "(neq (abs (- ?c1 ?c6)) 5)" +
                "(neq (abs (- ?c1 ?c7)) 6)" +
                "(neq (abs (- ?c1 ?c8)) 7)" +

                "(neq (abs (- ?c2 ?c3)) 1)" +
                "(neq (abs (- ?c2 ?c4)) 2)" +
                "(neq (abs (- ?c2 ?c5)) 3)" +
                "(neq (abs (- ?c2 ?c6)) 4)" +
                "(neq (abs (- ?c2 ?c7)) 5)" +
                "(neq (abs (- ?c2 ?c8)) 6)" +

                "(neq (abs (- ?c3 ?c4)) 1)" +
                "(neq (abs (- ?c3 ?c5)) 2)" +
                "(neq (abs (- ?c3 ?c6)) 3)" +
                "(neq (abs (- ?c3 ?c7)) 4)" +
                "(neq (abs (- ?c3 ?c8)) 5)" +

                "(neq (abs (- ?c4 ?c5)) 1)" +
                "(neq (abs (- ?c4 ?c6)) 2)" +
                "(neq (abs (- ?c4 ?c7)) 3)" +
                "(neq (abs (- ?c4 ?c8)) 4)" +

                "(neq (abs (- ?c5 ?c6)) 1)" +
                "(neq (abs (- ?c5 ?c7)) 2)" +
                "(neq (abs (- ?c5 ?c8)) 3)" +

                "(neq (abs (- ?c6 ?c7)) 1)" +
                "(neq (abs (- ?c6 ?c8)) 2)" +

                "(neq (abs (- ?c7 ?c8)) 1)" +

                "(neq ?c1 ?c2)" +
                "(neq ?c1 ?c3)" +
                "(neq ?c1 ?c4)" +
                "(neq ?c1 ?c5)" +
                "(neq ?c1 ?c6)" +
                "(neq ?c1 ?c7)" +
                "(neq ?c1 ?c8)" +

                "(neq ?c2 ?c3)" +
                "(neq ?c2 ?c4)" +
                "(neq ?c2 ?c5)" +
                "(neq ?c2 ?c6)" +
                "(neq ?c2 ?c7)" +
                "(neq ?c2 ?c8)" +

                "(neq ?c3 ?c4)" +
                "(neq ?c3 ?c5)" +
                "(neq ?c3 ?c6)" +
                "(neq ?c3 ?c7)" +
                "(neq ?c3 ?c8)" +

                "(neq ?c4 ?c5)" +
                "(neq ?c4 ?c6)" +
                "(neq ?c4 ?c7)" +
                "(neq ?c4 ?c8)" +

                "(neq ?c5 ?c6)" +
                "(neq ?c5 ?c7)" +
                "(neq ?c5 ?c8)" +

                "(neq ?c6 ?c7)" +
                "(neq ?c6 ?c8)" +

                "(neq ?c7 ?c8)" +
                ")";

            if (em.MatchMultiple(patterns, constraints, out matchList))
            {
                List<string> solutions = new List<string>();
                foreach (var m in matchList)
                    solutions.Add($"{m["?c1"]} {m["?c2"]} {m["?c3"]} {m["?c4"]} {m["?c5"]} {m["?c6"]} {m["?c7"]} {m["?c8"]}");

                solutions.Sort();

                foreach (string s in solutions)
                    WriteLine(s);
            }
        }
    }
}