
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
        public static List<TerminalList> CalculateFirstSet(List<GrammarRule> grammar)
        {
            List<TerminalList> firstSet = new List<TerminalList>();
            List<SyntaxTreeNonterminal> deduced = new List<SyntaxTreeNonterminal>();

            RecursiveCalculateFirst(ref firstSet, grammar, grammar[0].LeftSide);

            return firstSet;
        }

        /// <summary>
        /// This method recursively calculates all terminal symbols for each nonterminal
        /// symbol that might be the beginning of the nonterminal symbol.
        /// </summary>
        /// <param name="first">The output set of first terminals</param>
        /// <param name="grammar">The grammar input</param>
        /// <param name="deduction">The currently deduced grammar rule</param>
        private static void RecursiveCalculateFirst(ref List<TerminalList> first, List<GrammarRule> grammar, SyntaxTreeNonterminal deduction)
        {
            if (-1 != first.FindIndex(x => x.NonTerminal.CompareTo(deduction) == 0))
            {
                return;
            }

            TerminalList termList = new TerminalList(deduction);

            first.Add(termList);

            foreach (var rule in grammar)
            {
                if (deduction.CompareTo(rule.LeftSide) == 0)
                {
                    if (rule.RightSide[0] is SyntaxTreeTerminal)
                    {
                        termList.Terminals.Add(rule.RightSide[0] as SyntaxTreeTerminal);
                    }
                }
            }

            foreach (var rule in grammar)
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

        public static List<TerminalList> CalculateFollowSet(List<GrammarRule> grammar, List<TerminalList> firstSet)
        {
            List<TerminalList> followSet = new List<TerminalList>();

            return followSet;
        }

    }

}
