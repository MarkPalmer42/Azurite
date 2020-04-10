
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
            RecursiveCalculateFirst(grammar, grammar.ProductionRules[0].LeftSide);
        }

        /// <summary>
        /// This method recursively calculates all terminal symbols for each nonterminal
        /// symbol that might be the beginning of the nonterminal symbol.
        /// </summary>
        /// <param name="first">The output set of first terminals</param>
        /// <param name="grammar">The grammar input</param>
        /// <param name="deduction">The currently deduced grammar rule</param>
        private TerminalList RecursiveCalculateFirst(SyntaxGrammar grammar, SyntaxTreeNonterminal deduction)
        {
            TerminalList termList = FirstSet.Find(x => x.NonTerminal.Equals(deduction));

            if (null != termList)
            {
                return termList;
            }

            termList = new TerminalList(deduction);

            FirstSet.Add(termList);

            foreach (var rule in grammar.ProductionRules)
            {
                if (deduction.Equals(rule.LeftSide))
                {
                    if (0 == rule.RightSide.Count)
                    {
                        termList.AddTerminal(new SyntaxTreeTerminal(new EmptyToken()));
                    }
                    else
                    {
                        if (rule.RightSide[0] is SyntaxTreeTerminal)
                        {
                            termList.AddTerminal(rule.RightSide[0] as SyntaxTreeTerminal);
                        }
                    }
                }
            }

            foreach (var rule in grammar.ProductionRules)
            {
                if (deduction.Equals(rule.LeftSide))
                {
                    if (0 < rule.RightSide.Count && rule.RightSide[0] is SyntaxTreeNonterminal)
                    {
                        if (!deduction.Equals(rule.RightSide[0]))
                        {
                            var firstOfDeduced = RecursiveCalculateFirst(grammar, rule.RightSide[0] as SyntaxTreeNonterminal);

                            foreach (var terminal in firstOfDeduced.Terminals)
                            {
                                termList.AddTerminal(terminal);
                            }
                        }
                    }

                    for(int i = 1; i < rule.RightSide.Count; ++i)
                    {
                        if (rule.RightSide[i] is SyntaxTreeNonterminal)
                        {
                            RecursiveCalculateFirst(grammar, rule.RightSide[i] as SyntaxTreeNonterminal);
                        }
                    }
                }
            }

            return termList;
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
            var tList = FollowSet.Find(x => x.NonTerminal.Equals(nt));

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
                    if (rule.RightSide[i].Equals(nt))
                    {
                        bool emptyToken = false;
                        int j = i + 1;

                        do
                        {
                            if (j == rule.RightSide.Count)
                            {
                                if (!rule.LeftSide.Equals(nt))
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
                                var rhs = rule.RightSide[j] as SyntaxTreeTerminal;

                                if (null != rhs)
                                {
                                    termList.AddTerminal(rhs);
                                }
                                else if (rule.RightSide[j] is SyntaxTreeNonterminal)
                                {
                                    SyntaxTreeNonterminal syntaxTreeNonterminal = rule.RightSide[j] as SyntaxTreeNonterminal;

                                    var firstSet = FirstSet.Find(x => x.NonTerminal.Equals(syntaxTreeNonterminal));

                                    emptyToken = false;

                                    foreach (var t in firstSet.Terminals)
                                    {
                                        if (t.SyntaxToken is EmptyToken)
                                        {
                                            emptyToken = true;
                                        }
                                        else
                                        {
                                            termList.AddTerminal(t);
                                        }
                                    }
                                }
                                else
                                {
                                    throw new System.Exception("Invalid syntax tree element.");
                                }
                            }

                            if (emptyToken)
                            {
                                ++j;
                            }
                        }
                        while (emptyToken && j < rule.RightSide.Count);
                    }
                }
            }

            return termList;
        }

    }

}
