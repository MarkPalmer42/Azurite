
using Azurite.LexicalParser;
using Azurite.SyntaxAnalysis;
using Azurite.SyntaxAnalysis.Grammar;
using Azurite.SyntaxAnalysis.ParseSets;
using Azurite.SyntaxAnalysis.SyntaxParsingTable;
using Azurite.SyntaxAnalysis.SyntaxTree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AzuriteTest.SyntaAnalysis
{

    /// <summary>
    /// A unit test to test the calculation of follow sets.
    /// </summary>
    [TestClass]
    public class FollowSetCalculation_UT
    {

        /// <summary>
        /// Tests if the follow set calculation work in case of cross-reference in SLR1 grammar.
        /// </summary>
        [TestMethod]
        public void FollowSetGeneration_1()
        {
            SyntaxGrammar grammar = new SyntaxGrammar();

            grammar.AddSimpleRule('S', "E");
            grammar.AddSimpleRule('E', "Bd");
            grammar.AddSimpleRule('B', "Ec");
            grammar.AddSimpleRule('E', "c");
            grammar.AddSimpleRule('B', "d");

            ParsingSets sets = new ParsingSets(grammar);

            Assert.AreEqual(3, sets.FollowSet.Count);

            Assert.AreEqual("S", sets.FollowSet[0].NonTerminal.Name);
            Assert.AreEqual("E", sets.FollowSet[1].NonTerminal.Name);
            Assert.AreEqual("B", sets.FollowSet[2].NonTerminal.Name);

            Assert.AreEqual(1, sets.FollowSet[0].Terminals.Count);
            Assert.AreEqual(2, sets.FollowSet[1].Terminals.Count);
            Assert.AreEqual(1, sets.FollowSet[2].Terminals.Count);

            /*Assert.AreNotEqual(-1, sets.FollowSet[0].Terminals[0].SyntaxToken.CompareTo(new ExtremalToken()) == 0);

            Assert.AreNotEqual(-1, sets.FollowSet[1].Terminals.FindIndex(x => x.SyntaxToken.Text == "c"));
            Assert.AreNotEqual(-1, sets.FollowSet[1].Terminals.FindIndex(x => x.SyntaxToken.CompareTo(new ExtremalToken()) == 0));*/

            Assert.AreNotEqual(-1, sets.FollowSet[2].Terminals[0].SyntaxToken.Text == "d");
        }

        /// <summary>
        /// Tests if the follow set calculation work in case of cross-reference in SLR1 grammar.
        /// </summary>
        [TestMethod]
        public void FollowSetGeneration_2()
        {
            SyntaxGrammar grammar = new SyntaxGrammar();

            grammar.AddSimpleRule('S', "E");
            grammar.AddSimpleRule('E', "BB");
            grammar.AddSimpleRule('B', "cB");
            grammar.AddSimpleRule('B', "d");

            grammar.AddZerothState();

            ParsingSets sets = new ParsingSets(grammar);

            Assert.AreEqual(4, sets.FollowSet.Count);

            var zero = sets.FollowSet.Find(x => x.NonTerminal.Name == "ZERO0");
            var s = sets.FollowSet.Find(x => x.NonTerminal.Name == "S");
            var e = sets.FollowSet.Find(x => x.NonTerminal.Name == "E");
            var b = sets.FollowSet.Find(x => x.NonTerminal.Name == "B");

            Assert.AreNotEqual(null, zero);
            Assert.AreNotEqual(null, s);
            Assert.AreNotEqual(null, e);
            Assert.AreNotEqual(null, b);

            Assert.AreEqual(true, CompareLists(zero, new List<string>() { "$" }));
            Assert.AreEqual(true, CompareLists(s, new List<string>() { "$" }));
            Assert.AreEqual(true, CompareLists(e, new List<string>() { "$" }));
            Assert.AreEqual(true, CompareLists(b, new List<string>() { "$", "c", "d" }));
        }

        /// <summary>
        /// Tests if the follow set calculation work in case of cross-reference in SLR1 grammar.
        /// </summary>
        [TestMethod]
        public void FollowSetGeneration_3()
        {
            SyntaxGrammar grammar = new SyntaxGrammar();

            grammar.AddSimpleRule('S', "E");
            grammar.AddSimpleRule('E', "E+T");
            grammar.AddSimpleRule('E', "T");
            grammar.AddSimpleRule('T', "T*F");
            grammar.AddSimpleRule('T', "F");
            grammar.AddSimpleRule('F', "(E)");
            grammar.AddSimpleRule('F', "a");
            
            grammar.AddZerothState();

            ParsingSets sets = new ParsingSets(grammar);

            Assert.AreEqual(5, sets.FollowSet.Count);

            var zero = sets.FollowSet.Find(x => x.NonTerminal.Name == "ZERO0");
            var s = sets.FollowSet.Find(x => x.NonTerminal.Name == "S");
            var e = sets.FollowSet.Find(x => x.NonTerminal.Name == "E");
            var t = sets.FollowSet.Find(x => x.NonTerminal.Name == "T");
            var f = sets.FollowSet.Find(x => x.NonTerminal.Name == "F");

            Assert.AreNotEqual(null, zero);
            Assert.AreNotEqual(null, s);
            Assert.AreNotEqual(null, e);
            Assert.AreNotEqual(null, t);
            Assert.AreNotEqual(null, f);
            
            Assert.AreEqual(true, CompareLists(zero, new List<string>() { "$" }));
            Assert.AreEqual(true, CompareLists(s, new List<string>() { "$" }));
            Assert.AreEqual(true, CompareLists(e, new List<string>() { "$", ")", "+" }));
            Assert.AreEqual(true, CompareLists(t, new List<string>() { "$", ")", "*", "+" }));
            Assert.AreEqual(true, CompareLists(f, new List<string>() { "$", ")", "*", "+" }));
        }

        private bool CompareLists(TerminalList list1, List<string> strList)
        {
            TerminalList list2 = new TerminalList(null);

            foreach (var s in strList)
            {
                if ("$" == s)
                {
                    list2.AddTerminal(new SyntaxTreeTerminal(new ExtremalToken()));
                }
                else
                {
                    list2.AddTerminal(new SyntaxTreeTerminal(new Token(s, 0)));
                }
            }

            var firstMinusSecond = list1.Terminals.Except(list2.Terminals).ToList();
            var secondMinusFirst = list2.Terminals.Except(list1.Terminals).ToList();

            return !firstMinusSecond.Any() && !secondMinusFirst.Any();
        }

    }
}
