
using Azurite.LexicalParser;
using Azurite.SyntaxAnalysis.Grammar;
using Azurite.SyntaxAnalysis.SyntaxParsingTable;
using Azurite.SyntaxAnalysis.SyntaxTree;
using System.Collections.Generic;

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

            foreach (var rule in grammar.ProductionRules)
            {
                if (-1 == followSet.FindIndex(x => x.NonTerminal.CompareTo(rule.LeftSide) == 0))
                {
                    TerminalList termList = new TerminalList(rule.LeftSide);
                    followSet.Add(termList);

                    foreach(var r in grammar.ProductionRules)
                    {
                        for(int i = 0; i < r.RightSide.Count; ++i)
                        {
                            if (0 == r.RightSide[i].CompareTo(rule.LeftSide))
                            {
                                if (i + 1 < r.RightSide.Count)
                                {
                                    var nt = r.RightSide[i + 1] as SyntaxTreeNonterminal;
                                    var t = r.RightSide[i + 1] as SyntaxTreeTerminal;

                                    if (null != nt)
                                    {
                                        int firstIdx = firstSet.FindIndex(x => x.NonTerminal.CompareTo(nt) == 0);

                                        if (-1 == firstIdx)
                                        {
                                            throw new System.Exception("Cannot generate follow set without correct first set.");
                                        }

                                        foreach (var x in firstSet[firstIdx].Terminals)
                                        {
                                            termList.AddTerminal(x);
                                        }
                                    }
                                    else if (null != t)
                                    {
                                        termList.AddTerminal(t);
                                    }
                                    else
                                    {
                                        throw new System.Exception("Cannot calculate follow set based on invalid grammar.");
                                    }
                                }
                                else
                                {
                                    SyntaxTreeTerminal extremal = new SyntaxTreeTerminal(new Token("$", 0));

                                    termList.AddTerminal(extremal);
                                }
                            }
                        }
                    }
                }
            }

            /* Adding an extremal element for the staring rule, it always follow the starting rule. */
            followSet[0].Terminals.Add(new SyntaxTreeTerminal(new Token("$", 0)));

            return followSet;
        }

    }

}
