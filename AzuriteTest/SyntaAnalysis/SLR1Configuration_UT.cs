using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Azurite.SyntaxAnalysis.Grammar;
using Azurite.SyntaxAnalysis.ParseSets;
using Azurite.SyntaxAnalysis.ParseConfiguration;
using Azurite.SyntaxAnalysis.SyntaxTree;
using System.Linq;

namespace AzuriteTest.SyntaAnalysis
{

    /// <summary>
    /// Unit teswt of SLR1 configuration generation.
    /// </summary>
    [TestClass]
    public class SLR1Configuration_UT
    {

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void SLR1Configuration_Test1()
        {
            SyntaxGrammar grammar = new SyntaxGrammar();

            grammar.AddSimpleRule('S', "E");
            grammar.AddSimpleRule('E', "Bd");
            grammar.AddSimpleRule('B', "Ec");
            grammar.AddSimpleRule('E', "c");
            grammar.AddSimpleRule('B', "d");

            grammar.AddZerothState();

            SLR1Configuration config = new SLR1Configuration(grammar);

            Assert.AreEqual(8, config.Config.Count);

            List<List<string>> expectedConfig = new List<List<string>>();

            expectedConfig.Add(new List<string>() { "ZERO0;.S", "S;.E", "E;.Bd", "E;.c", "B;.Ec", "B;.d" }); /* 0 */
            expectedConfig.Add(new List<string>() { "ZERO0;S." });  /* 1 */
            expectedConfig.Add(new List<string>() { "S;E.", "B;E.c" }); /* 2 */
            expectedConfig.Add(new List<string>() { "E;B.d" }); /* 3 */
            expectedConfig.Add(new List<string>() { "B;Ec." }); /* 4 */
            expectedConfig.Add(new List<string>() { "E;c." });  /* 5 */
            expectedConfig.Add(new List<string>() { "B;d." });  /* 6 */
            expectedConfig.Add(new List<string>() { "E;Bd." }); /* 7 */

            List<List<int>> expectedIndices = new List<List<int>>();

            expectedIndices.Add(new List<int>() { 1, 2, 3, 2, 4, 5 });
            expectedIndices.Add(new List<int>() { -1 });
            expectedIndices.Add(new List<int>() { -1, 4 });
            expectedIndices.Add(new List<int>() { 7 });
            expectedIndices.Add(new List<int>() { -1 });
            expectedIndices.Add(new List<int>() { -1 });
            expectedIndices.Add(new List<int>() { -1 });
            expectedIndices.Add(new List<int>() { -1 });

            List<int> indices = new List<int>();

            for (int i = 0; i < expectedConfig.Count; ++i)
            {
                int currentIndex = MatchConfigElement(config, expectedConfig[i]);
                indices.Add(currentIndex);

                for (int j = 0; j < expectedIndices.Count; ++j)
                {
                    for (int k = 0; k < expectedIndices[j].Count; ++k)
                    {
                        if (expectedIndices[j][k] == i)
                        {
                            expectedIndices[j][k] = currentIndex;
                        }
                    }
                }

                Assert.AreNotEqual(-1, currentIndex);
            }

            for (int i = 0; i < expectedIndices.Count; ++i)
            {
                Assert.AreNotEqual(-1, MatchConfigIndex(config, expectedIndices[i]));
            }
        }

        public int MatchConfigIndex(SLR1Configuration config, List<int> indices)
        {
            int i = 0;
            bool found = false;

            indices.Sort();

            while (!found && i < config.Config.Count)
            {
                List<int> cmp = new List<int>();

                foreach (var j in config.Config[i])
                {
                    cmp.Add(j.Target);
                }
                
                cmp.Sort();

                found = CompareLists(indices, cmp);

                ++i;
            }

            return found ? i - 1 : -1;
        }

        public int MatchConfigElement(SLR1Configuration config, List<string> expected)
        {
            int i = 0;
            bool found = false;

            while (!found && i < config.Config.Count)
            {
                bool all = config.Config[i].Count == expected.Count;
                int j = 0;

                while (all && j < config.Config[i].Count)
                {
                    bool f2 = false;
                    int k = 0;

                    while (!f2 && k < expected.Count)
                    {
                        f2 = MatchElement(config.Config[i][j], expected[k]);
                        ++k;
                    }

                    all = f2;
                    ++j;
                }

                found = all;
                ++i;
            }

            return found ? i - 1 : -1;
        }

        public bool MatchElement(SLR1Item item, string expected)
        {
            if (expected.Contains(";"))
            {
                var splt = expected.Split(';');

                var leftSide = splt[0];
                var rightSide = splt[1];

                int ind = rightSide.IndexOf('.');

                if (-1 == ind)
                {
                    return false;
                }

                rightSide = rightSide.Replace(".", "");

                var lhs = new SyntaxTreeNonterminal(leftSide);
                var rhs = new List<SyntaxTreeElement>();

                foreach (var r in rightSide)
                {
                    if (Char.IsUpper(r))
                    {
                        rhs.Add(new SyntaxTreeNonterminal(r.ToString()));
                    }
                    else
                    {
                        rhs.Add(new SyntaxTreeTerminal(new Azurite.LexicalParser.Token(r.ToString(), 0)));
                    }
                }

                GrammarRule rule = new GrammarRule(lhs, rhs);

                return rule.Equals(item.Rule) && item.State == ind;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Compares two lists of integers.
        /// </summary>
        /// <param name="lst1">First list</param>
        /// <param name="lst2">Second list</param>
        /// <returns>True if equal, false otherwise</returns>
        private bool CompareLists(List<int> lst1, List<int> lst2)
        {
            var firstMinusSecond = lst1.Except(lst2).ToList();
            var secondMinusFirst = lst2.Except(lst1).ToList();

            return !firstMinusSecond.Any() && !secondMinusFirst.Any();
        }

    }
}
