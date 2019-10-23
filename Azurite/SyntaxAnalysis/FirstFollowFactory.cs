
using Azurite.LexicalParser;
using Azurite.SyntaxAnalysis.Grammar;
using Azurite.SyntaxAnalysis.SyntaxParsingTable;
using Azurite.SyntaxAnalysis.SyntaxTree;
using System.Collections.Generic;
using System.Linq;

namespace Azurite.SyntaxAnalysis
{

    /// <summary>
    /// Factory to generate first and follow sets based on the input grammar.
    /// </summary>
    public class FirstFollowFactory
    {

        /// <summary>
        /// This method calculates the first sets for all nonterminal symbol
        /// in the given grammar.
        /// </summary>
        /// <param name="grammar">The grammar</param>
        /// <returns>List of first sets</returns>
        public static List<TerminalList> CalculateFirstSet(SyntaxGrammar grammar)
        {
            List<TerminalList> firstSet = new List<TerminalList>();
            List<SyntaxTreeNonterminal> deduced = new List<SyntaxTreeNonterminal>();

            RecursiveCalculateFirst(ref firstSet, grammar, grammar.ProductionRules[0].LeftSide);

            return firstSet;
        }

        /// <summary>
        /// This method recursively calculates all terminal symbols for each nonterminal
        /// symbol that might be the beginning of the nonterminal symbol.
        /// </summary>
        /// <param name="first">The output set of first terminals</param>
        /// <param name="grammar">The grammar input</param>
        /// <param name="deduction">The currently deduced grammar rule</param>
        private static void RecursiveCalculateFirst(ref List<TerminalList> first, SyntaxGrammar grammar, SyntaxTreeNonterminal deduction)
        {
            if (-1 != first.FindIndex(x => x.NonTerminal.CompareTo(deduction) == 0))
            {
                return;
            }

            TerminalList termList = new TerminalList(deduction);

            first.Add(termList);

            foreach (var rule in grammar.ProductionRules)
            {
                if (deduction.CompareTo(rule.LeftSide) == 0)
                {
                    if (rule.RightSide[0] is SyntaxTreeTerminal)
                    {
                        termList.Terminals.Add(rule.RightSide[0] as SyntaxTreeTerminal);
                    }
                }
            }

            foreach (var rule in grammar.ProductionRules)
            {
                if (deduction.CompareTo(rule.LeftSide) == 0)
                {
                    if (rule.RightSide[0] is SyntaxTreeNonterminal)
                    {
                        RecursiveCalculateFirst(ref first, grammar, rule.RightSide[0] as SyntaxTreeNonterminal);

                        var firstOfDeduced = first.Find(x => x.NonTerminal.CompareTo(rule.RightSide[0]) == 0);

                        foreach(var terminal in firstOfDeduced.Terminals)
                        {
                            if (-1 == termList.Terminals.FindIndex(x => x.CompareTo(terminal) == 0))
                            {
                                termList.Terminals.Add(terminal);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Calculates the set of terminal symbols subsequent to all nonterminal symbols
        /// in the given grammar.
        /// </summary>
        /// <param name="grammar">The grammar</param>
        /// <param name="firstSet">The first set</param>
        /// <returns>The follow set</returns>
        public static List<TerminalList> CalculateFollowSet(SyntaxGrammar grammar, List<TerminalList> firstSet)
        {
            List<TerminalList> followSet = new List<TerminalList>();

            followSet.Add(new TerminalList(grammar.ProductionRules[0].LeftSide));
            followSet[0].Terminals.Add(new SyntaxTreeTerminal(new ExtremalToken()));

            foreach (var nt in grammar.Nonterminals)
            {
                bool all = true;
                int index = 0;

                while(all && index < grammar.ProductionRules.Count)
                {
                    if (grammar.ProductionRules[index].LeftSide.CompareTo(nt) == 0)
                    {
                        all = grammar.ProductionRules[index].RightSide.Last() is SyntaxTreeTerminal;
                    }

                    ++index;
                }

                if (all)
                {
                    RecursiveCalculateFollow(ref followSet, grammar, firstSet, nt);
                }
            }

            return followSet;
        }

        /// <summary>
        /// Recursively calculates the follows set of the given grammar.
        /// </summary>
        /// <param name="follow">The follow set to be calculated</param>
        /// <param name="grammar">The input grammar</param>
        /// <param name="first">The first set</param>
        /// <param name="nt">The currently checked nonterminal</param>
        /// <returns>The currently generated follow set</returns>
        private static TerminalList RecursiveCalculateFollow(ref List<TerminalList> follow, SyntaxGrammar grammar, List<TerminalList> first, SyntaxTreeNonterminal nt)
        {
            var tList = follow.Find(x => x.NonTerminal.CompareTo(nt) == 0);
            if (null != tList)
            {
                return tList;
            }

            TerminalList termList = new TerminalList(nt);
            follow.Add(termList);

            foreach (var rule in grammar.ProductionRules)
            {
                for (int i = 0; i < rule.RightSide.Count; ++i)
                {
                    if (rule.RightSide[i].CompareTo(nt) == 0)
                    {
                        if (i + 1 == rule.RightSide.Count)
                        {
                            if (rule.LeftSide.CompareTo(nt) != 0)
                            {
                                var fList = RecursiveCalculateFollow(ref follow, grammar, first, rule.LeftSide);

                                foreach (var t in fList.Terminals)
                                {
                                    termList.AddTerminal(t);
                                }
                            }
                        }
                        else
                        {
                            if (rule.RightSide[i + 1] is SyntaxTreeTerminal)
                            {
                                termList.AddTerminal(rule.RightSide[i + 1] as SyntaxTreeTerminal);
                            }
                            else if (rule.RightSide[i + 1] is SyntaxTreeNonterminal)
                            {
                                SyntaxTreeNonterminal syntaxTreeNonterminal = rule.RightSide[i + 1] as SyntaxTreeNonterminal;

                                var firstSet = first.Find(x => x.NonTerminal.CompareTo(syntaxTreeNonterminal) == 0);

                                foreach (var t in firstSet.Terminals)
                                {
                                    termList.AddTerminal(t);
                                }
                            }
                            else
                            {
                                throw new System.Exception("Invalid syntax tree element.");
                            }
                        }
                    }
                }
            }

            return termList;
        }

    }

}
