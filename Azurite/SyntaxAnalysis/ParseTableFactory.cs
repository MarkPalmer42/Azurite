using Azurite.SyntaxAnalysis.ParseTable;
using Azurite.SyntaxAnalysis.SyntaxParsingTable;
using Azurite.SyntaxAnalysis.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azurite.SyntaxAnalysis
{

    public static class ParseTableFactory
    {

        public static ParsingTable BuildParseTable(List<ParsingRule> grammar)
        {
            if (grammar.Count < 1)
            {
                throw new System.Exception("Cannot build parse table without input rules.");
            }

            ParsingTable table = new ParsingTable();

            List<List<RuleSet>> parseStates = new List<List<RuleSet>>();

            ParsingRule rule = new ParsingRule();

            rule.LeftSide = new SyntaxTreeNonterminal("ZEROETHSTATE");
            rule.RightSide.Add(grammar[0].LeftSide);

            ParseState startState = new ParseState(rule, 0);

            RuleSet startSet = new RuleSet(startState);

            RecursiveTableBuild(grammar, ref parseStates, startSet);

            return table;
        }

        static int RecursiveTableBuild(List<ParsingRule> grammar, ref List<List<RuleSet>> ruleSets, RuleSet extendable = null)
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
                    ruleSets.Add(new List<RuleSet>());
                    ruleSets[ruleSets.Count - 1].Add(extendable);
                }
            }

            int currentRuleSet = ruleSets.Count - 1;

            int currentRuleIndex = 0;

            while (currentRuleIndex < ruleSets[currentRuleSet].Count)
            {
                var currentRule = ruleSets[currentRuleSet][currentRuleIndex];

                SyntaxTreeElement nextSymbol = null;

                if (currentRule.Rules.Rule.RightSide.Count > currentRule.Rules.State)
                {
                    nextSymbol = currentRule.Rules.Rule.RightSide[currentRule.Rules.State];
                }

                if (null != nextSymbol)
                {
                    if (nextSymbol is SyntaxTreeNonterminal)
                    {
                        foreach (var rule in grammar)
                        {
                            if (0 == rule.LeftSide.CompareTo(nextSymbol))
                            {
                                RuleSet extendedRuleSet = new RuleSet(new ParseState(rule, 0));

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

                if (s.Rules.Rule.RightSide.Count > s.Rules.State)
                {
                    nextSymbol = s.Rules.Rule.RightSide[s.Rules.State];
                }

                if (null != nextSymbol)
                {
                    RuleSet nextRuleSet = new RuleSet(new ParseState(s.Rules.Rule, s.Rules.State + 1));
                    s.TargetSet = RecursiveTableBuild(grammar, ref ruleSets, nextRuleSet);
                }
            }

            return currentRuleSet;
        }

    }

}
