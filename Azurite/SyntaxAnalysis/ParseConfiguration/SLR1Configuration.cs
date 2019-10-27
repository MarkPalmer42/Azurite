
using Azurite.SyntaxAnalysis.Grammar;
using Azurite.SyntaxAnalysis.SyntaxTree;
using System.Collections.Generic;
using System.Linq;

namespace Azurite.SyntaxAnalysis.ParseConfiguration
{

    /// <summary>
    /// Generates the SLR1 configuration sets and stores them.
    /// </summary>
    public class SLR1Configuration
    {

        /// <summary>
        /// Contains all SLR1 configuration sets.
        /// </summary>
        public List<List<SLR1Item>> Config { get; private set; }

        /// <summary>
        /// Creates an SLR1 configuration based on the given grammar.
        /// </summary>
        /// <param name="grammar">The grammar</param>
        public SLR1Configuration(SyntaxGrammar grammar)
        {
            Config = new List<List<SLR1Item>>();

            List<SLR1Item> startState = new List<SLR1Item>();

            startState.Add(new SLR1Item(grammar.ProductionRules[0], 0));

            RecursiveConfigBuild(grammar, startState);
        }

        /// <summary>
        /// Recursive method to generate the extensions of the grammar rules.
        /// </summary>
        /// <param name="grammar">The grammar</param>
        /// <param name="ruleSets">The extended grammar rules</param>
        /// <param name="extendable">The current rule to be extended</param>
        /// <returns>The index of the newly created extended gramar rule set</returns>
        int RecursiveConfigBuild(SyntaxGrammar grammar, List<SLR1Item> extendable = null)
        {
            if (null != extendable)
            {
                int idx = Config.FindIndex(x => x[0].Equals(extendable[0]));

                if (-1 != idx)
                {
                    return idx;
                }
                else
                {
                    Config.Add(new List<SLR1Item>());

                    foreach (var e in extendable)
                    {
                        Config.Last().Add(e);
                    }
                }
            }

            int currentRuleSet = Config.Count - 1;

            int currentRuleIndex = 0;

            while (currentRuleIndex < Config[currentRuleSet].Count)
            {
                var currentRule = Config[currentRuleSet][currentRuleIndex];

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
                            if (rule.LeftSide.Equals(nextSymbol))
                            {
                                SLR1Item extendedRuleSet = new SLR1Item(rule, 0);

                                int idx = Config[currentRuleSet].FindIndex(x => x.Equals(extendedRuleSet));

                                if (-1 == idx)
                                {
                                    Config[currentRuleSet].Add(extendedRuleSet);
                                }
                            }
                        }
                    }
                }

                ++currentRuleIndex;
            }

            foreach (var s in Config[currentRuleSet])
            {
                SyntaxTreeElement nextSymbol = null;

                if (s.Rule.RightSide.Count > s.State)
                {
                    nextSymbol = s.Rule.RightSide[s.State];
                }

                if (null != nextSymbol)
                {
                    List<SLR1Item> nextRuleSet = new List<SLR1Item>();

                    foreach (var s2 in Config[currentRuleSet])
                    {
                        if (s2.Rule.RightSide.Count > s2.State)
                        {
                            if (nextSymbol.Equals(s2.Rule.RightSide[s2.State]))
                            {
                                nextRuleSet.Add(new SLR1Item(s2.Rule, s2.State + 1));
                            }
                        }
                    }

                    s.Target = RecursiveConfigBuild(grammar, nextRuleSet);
                }
            }

            return currentRuleSet;
        }

    }
}
