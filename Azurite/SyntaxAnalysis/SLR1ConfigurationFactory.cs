
using Azurite.SyntaxAnalysis.Grammar;
using Azurite.SyntaxAnalysis.SyntaxParsingTable;
using Azurite.SyntaxAnalysis.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azurite.SyntaxAnalysis
{
    public static class SLR1ConfigurationFactory
    {

        /// <summary>
        /// Creates an SLR1 configuration based on the given grammar.
        /// </summary>
        /// <param name="grammar">The grammar</param>
        /// <returns>The configuration</returns>
        public static List<List<SLR1Item>> CreateConfiguration(SyntaxGrammar grammar)
        {
            List<List<SLR1Item>> parseStates = new List<List<SLR1Item>>();

            SLR1Item startState = new SLR1Item(grammar.ProductionRules[0], 0);

            RecursiveConfigBuild(grammar, ref parseStates, startState);

            return parseStates;
        }

        /// <summary>
        /// Recursive method to generate the extensions of the grammar rules.
        /// </summary>
        /// <param name="grammar">The grammar</param>
        /// <param name="ruleSets">The extended grammar rules</param>
        /// <param name="extendable">The current rule to be extended</param>
        /// <returns>The index of the newly created extended gramar rule set</returns>
        static int RecursiveConfigBuild(SyntaxGrammar grammar, ref List<List<SLR1Item>> ruleSets, SLR1Item extendable = null)
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
                        foreach (var rule in grammar.ProductionRules)
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
                    s.Target = RecursiveConfigBuild(grammar, ref ruleSets, nextRuleSet);
                }
            }

            return currentRuleSet;
        }

    }
}
