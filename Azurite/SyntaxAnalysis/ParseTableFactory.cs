
using Azurite.SyntaxAnalysis.ParseTable;
using Azurite.SyntaxAnalysis.SyntaxParsingTable;
using Azurite.SyntaxAnalysis.SyntaxTree;
using System.Collections.Generic;

namespace Azurite.SyntaxAnalysis
{

    /// <summary>
    /// Factory class to create SLR1 parsing tables.
    /// </summary>
    public static class SLR1ParseTableFactory
    {

        /// <summary>
        /// Builds a parse table based on the grammar provided.
        /// </summary>
        /// <param name="grammar">The grammar</param>
        /// <returns>A parsing table</returns>
        public static ParsingTable BuildParseTable(List<GrammarRule> grammar)
        {
            if (grammar.Count < 1)
            {
                throw new System.Exception("Cannot build parse table without input rules.");
            }

            ParsingTable table = new ParsingTable();

            List<List<SLR1Item>> parseStates = new List<List<SLR1Item>>();

            GrammarRule rule = new GrammarRule();

            rule.LeftSide = new SyntaxTreeNonterminal("ZEROETHSTATE");
            rule.RightSide.Add(grammar[0].LeftSide);

            SLR1Item startState = new SLR1Item(rule, 0);

            RecursiveTableBuild(grammar, ref parseStates, startState);

            return table;
        }

        /// <summary>
        /// Recursive method to generate the extensions of the grammar rules.
        /// </summary>
        /// <param name="grammar">The grammar</param>
        /// <param name="ruleSets">The extended grammar rules</param>
        /// <param name="extendable">The current rule to be extended</param>
        /// <returns>The index of the newly created extended gramar rule set</returns>
        static int RecursiveTableBuild(List<GrammarRule> grammar, ref List<List<SLR1Item>> ruleSets, SLR1Item extendable = null)
        {
            if (null != extendable)
            {
                int idx = ruleSets.FindIndex(x => x[0].CompareTo(extendable) == 0);

                if (-1 != idx)
                {
                    return idx;
                }
                else
                {
                    ruleSets.Add(new List<SLR1Item>());
                    ruleSets[ruleSets.Count - 1].Add(extendable);
                }
            }

            int currentRuleSet = ruleSets.Count - 1;

            int currentRuleIndex = 0;

            while (currentRuleIndex < ruleSets[currentRuleSet].Count)
            {
                var currentRule = ruleSets[currentRuleSet][currentRuleIndex];

                SyntaxTreeElement nextSymbol = null;

                if (currentRule.Rule.RightSide.Count > currentRule.State)
                {
                    nextSymbol = currentRule.Rule.RightSide[currentRule.State];
                }

                if (null != nextSymbol)
                {
                    if (nextSymbol is SyntaxTreeNonterminal)
                    {
                        foreach (var rule in grammar)
                        {
                            if (0 == rule.LeftSide.CompareTo(nextSymbol))
                            {
                                SLR1Item extendedRuleSet = new SLR1Item(rule, 0);

                                int idx = ruleSets[currentRuleSet].FindIndex(x => x.CompareTo(extendedRuleSet) == 0);

                                if (-1 == idx)
                                {
                                    ruleSets[currentRuleSet].Add(extendedRuleSet);
                                }
                            }
                        }
                    }
                }

                ++currentRuleIndex;
            }

            foreach (var s in ruleSets[currentRuleSet])
            {
                SyntaxTreeElement nextSymbol = null;

                if (s.Rule.RightSide.Count > s.State)
                {
                    nextSymbol = s.Rule.RightSide[s.State];
                }

                if (null != nextSymbol)
                {
                    SLR1Item nextRuleSet = new SLR1Item(s.Rule, s.State + 1);
                    s.Target = RecursiveTableBuild(grammar, ref ruleSets, nextRuleSet);
                }
            }

            return currentRuleSet;
        }

    }

}
