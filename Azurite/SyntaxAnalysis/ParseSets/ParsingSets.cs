
using Azurite.SyntaxAnalysis.Grammar;
using Azurite.SyntaxAnalysis.SyntaxTree;
using System.Collections.Generic;
using System.Linq;

namespace Azurite.SyntaxAnalysis.ParseSets
{

    /// <summary>
    /// A class to generate and store first and follow sets used for the parsing process.
    /// </summary>
    public class ParsingSets
    {

        /// <summary>
        /// The first set that contains all the terminals that can be the
        /// beginning of a nonterminal symbol. Calculated for all nonterminals.
        /// </summary>
        public List<TerminalList> FirstSet { get; private set; }

        /// <summary>
        /// The follow set that contains all subsequent terminal symbol of
        /// a nontermianl symbol. Calculated for all nonterminals.
        /// </summary>
        public List<TerminalList> FollowSet { get; private set; }

        /// <summary>
        /// Constructs the parsing sets.
        /// </summary>
        /// <param name="grammar">The input grammar</param>
        public ParsingSets(SyntaxGrammar grammar)
        {
            FirstSet = new List<TerminalList>();
            FollowSet = new List<TerminalList>();

            CalculateFirstSet(grammar);
            CalculateFollowSet(grammar);
        }

        /// <summary>
        /// This method calculates the first sets for all nonterminal symbol
        /// in the given grammar.
        /// </summary>
        /// <param name="grammar">The grammar</param>
        /// <returns>List of first sets</returns>
        private void CalculateFirstSet(SyntaxGrammar grammar)
        {
            List<SyntaxTreeNonterminal> deduced = new List<SyntaxTreeNonterminal>();

            RecursiveCalculateFirst(grammar, grammar.ProductionRules[0].LeftSide);
        }

        /// <summary>
        /// This method recursively calculates all terminal symbols for each nonterminal
        /// symbol that might be the beginning of the nonterminal symbol.
        /// </summary>
        /// <param name="first">The output set of first terminals</param>
        /// <param name="grammar">The grammar input</param>
        /// <param name="deduction">The currently deduced grammar rule</param>
        private void RecursiveCalculateFirst(SyntaxGrammar grammar, SyntaxTreeNonterminal deduction)
        {
            if (-1 != FirstSet.FindIndex(x => x.NonTerminal.CompareTo(deduction) == 0))
            {
                return;
            }

            TerminalList termList = new TerminalList(deduction);

            FirstSet.Add(termList);

            foreach (var rule in grammar.ProductionRules)
            {
                if (deduction.CompareTo(rule.LeftSide) == 0)
                {
                    for (int i = 0; i < rule.RightSide.Count; ++i)
                    {
                        if (rule.RightSide[i] is SyntaxTreeTerminal)
                        {
                            termList.AddTerminal(rule.RightSide[i] as SyntaxTreeTerminal);
                        }
                    }
                }
            }

            foreach (var rule in grammar.ProductionRules)
            {
                if (deduction.CompareTo(rule.LeftSide) == 0)
                {
                    for (int i = 0; i < rule.RightSide.Count; ++i)
                    {
                        if (rule.RightSide[i] is SyntaxTreeNonterminal)
                        {
                            RecursiveCalculateFirst(grammar, rule.RightSide[i] as SyntaxTreeNonterminal);

                            var firstOfDeduced = FirstSet.Find(x => x.NonTerminal.CompareTo(rule.RightSide[i]) == 0);

                            foreach (var terminal in firstOfDeduced.Terminals)
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
        }

        /// <summary>
        /// Calculates the set of terminal symbols subsequent to all nonterminal symbols
        /// in the given grammar.
        /// </summary>
        /// <param name="grammar">The grammar</param>
        /// <param name="firstSet">The first set</param>
        /// <returns>The follow set</returns>
        private void CalculateFollowSet(SyntaxGrammar grammar)
        {
            FollowSet.Add(new TerminalList(grammar.ProductionRules[0].LeftSide));
            FollowSet[0].Terminals.Add(new SyntaxTreeTerminal(new ExtremalToken()));

            foreach (var nt in grammar.Nonterminals)
            {
                RecursiveCalculateFollow(grammar, nt);
            }
        }

        /// <summary>
        /// Recursively calculates the follows set of the given grammar.
        /// </summary>
        /// <param name="follow">The follow set to be calculated</param>
        /// <param name="grammar">The input grammar</param>
        /// <param name="first">The first set</param>
        /// <param name="nt">The currently checked nonterminal</param>
        /// <returns>The currently generated follow set</returns>
        private TerminalList RecursiveCalculateFollow(SyntaxGrammar grammar, SyntaxTreeNonterminal nt)
        {
            var tList = FollowSet.Find(x => x.NonTerminal.CompareTo(nt) == 0);

            if (null != tList)
            {
                return tList;
            }

            TerminalList termList = new TerminalList(nt);
            FollowSet.Add(termList);

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
                                var fList = RecursiveCalculateFollow(grammar, rule.LeftSide);

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

                                var firstSet = FirstSet.Find(x => x.NonTerminal.CompareTo(syntaxTreeNonterminal) == 0);

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
