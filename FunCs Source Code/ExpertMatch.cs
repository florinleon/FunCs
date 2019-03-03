/**************************************************************************
 *                                                                        *
 *  Description: FunCs Functional Programming Library                     *
 *  Website:     https://github.com/florinleon/FunCs                      *
 *  Copyright:   (c) 2019, Florin Leon                                    *
 *                                                                        *
 *  The pattern matching functionality based on the Rete algorithm is     *
 *  implemented in a dll from the following project:                      *
 *  garyriley, CLIPS Rule Based Programming Language. Expert System Tool  *
 *  https://sourceforge.net/projects/clipsrules/files/CLIPS/6.40_Beta_2/  *
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
using System.Text;
using RouterFormsExample;

namespace FunCs
{
    internal class Fact
    {
        public List<string> Slots { get; }

        public Fact(List<string> slots)
        {
            Slots = slots;
        }

        public Fact(string str)
        {
            Slots = str.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList<string>();
        }
    }

    /// <summary>
    /// The class that implements different pattern matching cases on single lists or lists of facts. General pattern matching is performed using the Rete algorithm implemented in the Clips expert system tool.
    /// </summary>
    public class ExpertMatchF
    {
        private List<Fact> _facts;
        private CLIPSNET.Environment _clips;

        /// <summary>
        /// Initializes a new instance of the ExpertMatchF class.
        /// </summary>
        /// <param name="list">A list of string items that will be used for pattern matching. To increase clarity and to stress that it is a list, it is recommended to enclose the items within square brackets</param>
        public ExpertMatchF(string list)
        {
            _facts = new List<Fact> { new Fact(RemoveBrackets(list)) };
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
        /// Initializes a new instance of the ExpertMatchF class.
        /// </summary>
        /// <param name="factList">A list of facts that will be used for pattern matching, where each fact is a list of string items. The facts are general and should not contain square brackets.</param>
        public ExpertMatchF(List<string> factList)
        {
            _facts = new List<Fact>();

            foreach (var f in factList)
                _facts.Add(new Fact(f));
        }

        /// <summary>
        /// Returns true if the list is empty.
        /// </summary>
        public bool MatchListEmpty()
        {
            if (_facts.Count() == 1 && _facts.First().Slots.Count() == 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Identifies the first item of the list and the rest of the list. It returns false if the list is empty.
        /// </summary>
        /// <param name="head">The head of the list, i.e. the first item</param>
        /// <param name="tail">The rest of the list, starting with the second item</param>
        public bool MatchListHeadTail(out string head, out List<string> tail)
        {
            head = ""; tail = new List<string>();

            if (_facts.Count != 1)
                return false;

            var slotList = _facts[0].Slots;

            if (slotList.Count == 0)
                return false;
            else if (slotList.Count == 1)
            {
                head = slotList[0];
                return true;
            }
            else
            {
                head = slotList[0];
                tail = slotList.Skip(1).ToList();
                return true;
            }
        }

        /// <summary>
        /// Matches an arbitrary pattern on the list of facts. It returns false if the pattern cannot be matched on the facts.
        /// </summary>
        /// <param name="pattern">A pattern containing items to be matched and variables, identified by ? for a single word or $? for multiple words, e.g. ?a or $?b. At least one variable must be named in the pattern</param>
        /// <param name="results">A dictionary that contains the values of the variables, e.g. results["?a"] contains the value of that variable after the pattern matching</param>
        public bool MatchListGeneral(string pattern, out Dictionary<string, string> results)
        {
            results = new Dictionary<string, string>();

            pattern = RemoveBrackets(pattern);

            var sb = new StringBuilder();

            sb.AppendLine("(defrule init => ");

            foreach (Fact f in _facts)
            {
                sb.Append("(assert (fact ");
                foreach (string s in f.Slots)
                {
                    sb.Append(s);
                    sb.Append(" ");
                }
                sb.AppendLine("))");
            }

            sb.AppendLine(")\r\n");

            var varNames = new List<string>();

            sb.AppendLine("(defrule match ");

            var toks = pattern.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < toks.Length; i++)
            {
                if (toks[i].Length > 1 && toks[i][0] == '?' && !varNames.Contains(toks[i]))
                    varNames.Add(toks[i]);
                else if (toks[i].Length > 2 && toks[i].Substring(0, 2) == "$?" && !varNames.Contains(toks[i]))
                    varNames.Add(toks[i]);
            }

            sb.AppendLine("(fact " + pattern + ")\r\n =>");

            foreach (string v in varNames)
                sb.AppendLine($"(printout t \"{v}\" \" \" {v} crlf)");

            sb.AppendLine("(printout t $$$$$ crlf)");

            sb.AppendLine(")");

            _clips = new CLIPSNET.Environment();
            var rtb = new RouterTextBox();
            rtb.AttachRouter(_clips, 10);
            _clips.LoadFromString(sb.ToString());
            _clips.Reset();
            _clips.Run();

            var clipsResult = rtb.Text.Trim(); // this needs to be optimized

            if (clipsResult == null || clipsResult == "")
                return false;

            var variant = clipsResult.Split(new string[] { "$$$$$" }, 2, StringSplitOptions.RemoveEmptyEntries)[0];

            var varvals = variant.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (string vv in varvals)
            {
                var tvv = vv.Trim();
                var ns = tvv.IndexOf(' '); // variable-name variable-value

                var varName = tvv.Substring(0, ns);

                if (varName[0] == '$')
                    results[varName] = tvv.Substring(ns + 2, tvv.Length - ns - 3); // $?varname (variable-value)
                else
                    results[varName] = tvv.Substring(ns + 1);
            }

            if (results.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Matches arbitrary patterns on the list of facts. It returns false if the patterns cannot be matched on the facts.
        /// </summary>
        /// <param name="patterns">A list of patterns containing items to be matched and variables, identified by ? for a single word or $? for multiple words, e.g. ?a or $?b. At least one variable must be named in the patterns</param>
        /// <param name="results">A dictionary that contains the values of the variables, e.g. results["?a"] contains the value of that variable after the pattern matching</param>
        public bool MatchMultiple(List<string> patterns, out List<Dictionary<string, string>> results)
        {
            return MatchMultiple(patterns, "", out results);
        }

        /// <summary>
        /// Matches arbitrary patterns on the list of facts. It returns false if the pattern cannot be matched on the facts.
        /// </summary>
        /// <param name="patterns">A list of patterns containing items to be matched and variables, identified by ? for a single word or $? for multiple words, e.g. ?a or $?b. At least one variable must be named in the patterns</param>
        /// <param name="constraints">A logical expression which contains the conditions that the matched variable values must satisfy. The Clips language syntax and operators are used to describe the constraints</param>
        /// <param name="results">A dictionary that contains the values of the variables, e.g. results["?a"] contains the value of that variable after the pattern matching</param>
        public bool MatchMultiple(List<string> patterns, string constraints, out List<Dictionary<string, string>> results)
        {
            results = new List<Dictionary<string, string>>();

            var sb = new StringBuilder();

            sb.AppendLine("(defrule init => ");

            foreach (Fact f in _facts)
            {
                sb.Append("(assert (fact ");
                foreach (string s in f.Slots)
                {
                    sb.Append(s);
                    sb.Append(" ");
                }
                sb.AppendLine("))");
            }

            sb.AppendLine(")\r\n");

            var varNames = new List<string>();

            sb.AppendLine("(defrule match ");

            foreach (var p in patterns)
            {
                var toks = p.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < toks.Length; i++)
                {
                    if (toks[i].Length > 1 && toks[i][0] == '?' && !varNames.Contains(toks[i]))
                        varNames.Add(toks[i]);
                    else if (toks[i].Length > 2 && toks[i].Substring(0, 2) == "$?" && !varNames.Contains(toks[i]))
                        varNames.Add(toks[i]);
                }

                sb.AppendLine("(fact " + p + ")\r\n");
            }

            if (constraints.Trim() != "")
                sb.AppendLine("(test " + constraints + ")\r\n");

            sb.AppendLine("=>");

            foreach (string v in varNames)
                sb.AppendLine($"(printout t \"{v}\" \" \" {v} crlf)");

            sb.AppendLine("(printout t $$$$$ crlf)");

            sb.AppendLine(")");

            _clips = new CLIPSNET.Environment();
            var rtb = new RouterTextBox();
            rtb.AttachRouter(_clips, 10);
            _clips.LoadFromString(sb.ToString());
            _clips.Reset();
            _clips.Run();

            var clipsResult = rtb.Text.Trim(); // this needs to be optimized

            if (clipsResult == null || clipsResult == "")
                return false;

            var variants = clipsResult.Split(new string[] { "$$$$$" }, int.MaxValue, StringSplitOptions.RemoveEmptyEntries);

            foreach (string v in variants)
            {
                var varvals = v.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                var d = new Dictionary<string, string>();

                foreach (string vv in varvals)
                {
                    var tvv = vv.Trim();
                    var ns = tvv.IndexOf(' '); // variable-name variable-value

                    var varName = tvv.Substring(0, ns);

                    if (varName[0] == '$')
                        d[varName] = tvv.Substring(ns + 2, tvv.Length - ns - 3); // $?varname (variable-value)
                    else
                        d[varName] = tvv.Substring(ns + 1);
                }

                results.Add(d);
            }

            if (results.Count > 0)
                return true;
            else
                return false;
        }
    }
}