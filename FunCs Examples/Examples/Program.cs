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
            MatchList.Ex1(); WriteLine("\r\n");
            MatchList.Ex2ExtensionMethods(); WriteLine("\r\n");

            MatchMultiple.Ex1Kinship(); WriteLine("\r\n");
            MatchMultiple.Ex2Cousins(); WriteLine("\r\n");
            MatchMultiple.Ex3Queens4(); WriteLine("\r\n");
            //MatchMultiple.Ex4Queens8(); WriteLine("\r\n");

            Options.Ex1SomeNone(); WriteLine("\r\n");
            Options.Ex2List(); WriteLine("\r\n");
            Options.Ex3Map(); WriteLine("\r\n");

            MatchOption.Ex1(); WriteLine("\r\n");
            MatchOption.Ex2ExtensionMethod();
        }
    }
}