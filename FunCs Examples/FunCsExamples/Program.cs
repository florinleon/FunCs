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

using static System.Console;

namespace FunCsExamples
{
    public class Program
    {
        private static void Main(string[] args)
        {
            PatternMatchingExamples.Permutations();
            PatternMatchingExamples.Combinations();
            PatternMatchingExamples.SumList();
            PatternMatchingExamples.MatchList();

            PatternMatchingExamples.MatchGeneral1();
            PatternMatchingExamples.MatchGeneral2();
            PatternMatchingExamples.MatchGeneralQueens4();
            PatternMatchingExamples.MatchGeneralQueens8();

            LinqExt.Ex1();
            LinqExt.Ex2();

            Options.Ex1SomeNone(); WriteLine("\r\n");
            Options.Ex2List(); WriteLine("\r\n");
            Options.Ex3Map(); WriteLine("\r\n");

            MatchOption.Ex1(); WriteLine("\r\n");
            MatchOption.Ex2ExtensionMethod();
        }
    }
}