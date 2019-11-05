using System;
using System.Collections.Generic;
using System.Linq;
using Azurite.LexicalParser;
using Azurite.SyntaxAnalysis;
using Azurite.SyntaxAnalysis.Grammar;
using Azurite.SyntaxAnalysis.ParseSets;
using Azurite.SyntaxAnalysis.SyntaxParsingTable;
using Azurite.SyntaxAnalysis.SyntaxTree;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AzuriteTest.SyntaAnalysis
{

    /// <summary>
    /// A unit test to test the calculation of first sets.
    /// </summary>
    [TestClass]
    public class FirstFollowSetCalculation_UT
    {

        /// <summary>
        /// Tests if the first set calculation work in case of cross-reference in SLR1 grammar.
        /// </summary>
        [TestMethod]
        public void FirstSetGeneration_1()
        {
            SyntaxGrammar grammar = new SyntaxGrammar();

            grammar.AddSimpleRule('S', "E");
            grammar.AddSimpleRule('E', "Bd");
            grammar.AddSimpleRule('B', "Ec");
            grammar.AddSimpleRule('E', "c");
            grammar.AddSimpleRule('B', "d");

            grammar.AddZerothState();

            ParsingSets sets = new ParsingSets(grammar);

            Assert.AreEqual(4, sets.FollowSet.Count);

            var zero = sets.FirstSet.Find(x => x.NonTerminal.Name == "ZERO0");
            var s = sets.FirstSet.Find(x => x.NonTerminal.Name == "S");
            var e = sets.FirstSet.Find(x => x.NonTerminal.Name == "E");
            var b = sets.FirstSet.Find(x => x.NonTerminal.Name == "B");

            Assert.AreNotEqual(null, zero);
            Assert.AreNotEqual(null, s);
            Assert.AreNotEqual(null, e);
            Assert.AreNotEqual(null, b);

            Assert.AreEqual(true, CompareLists(zero, new List<string>() { "c", "d" }));
            Assert.AreEqual(true, CompareLists(s, new List<string>() { "c", "d" }));
            Assert.AreEqual(true, CompareLists(e, new List<string>() { "c", "d" }));
            Assert.AreEqual(true, CompareLists(b, new List<string>() { "c", "d" }));
        }

        /// <summary>
        /// Tests if the first set calculation works.
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

            var zero = sets.FirstSet.Find(x => x.NonTerminal.Name == "ZERO0");
            var s = sets.FirstSet.Find(x => x.NonTerminal.Name == "S");
            var e = sets.FirstSet.Find(x => x.NonTerminal.Name == "E");
            var b = sets.FirstSet.Find(x => x.NonTerminal.Name == "B");

            Assert.AreNotEqual(null, zero);
            Assert.AreNotEqual(null, s);
            Assert.AreNotEqual(null, e);
            Assert.AreNotEqual(null, b);

            Assert.AreEqual(true, CompareLists(zero, new List<string>() { "c", "d" }));
            Assert.AreEqual(true, CompareLists(s, new List<string>() { "c", "d" }));
            Assert.AreEqual(true, CompareLists(e, new List<string>() { "c", "d" }));
            Assert.AreEqual(true, CompareLists(b, new List<string>() { "c", "d" }));
        }

        /// <summary>
        /// Tests if the first set calculation works.
        /// </summary>
        [TestMethod]
        public void FirstSetGeneration_3()
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

            var zero = sets.FirstSet.Find(x => x.NonTerminal.Name == "ZERO0");
            var s = sets.FirstSet.Find(x => x.NonTerminal.Name == "S");
            var e = sets.FirstSet.Find(x => x.NonTerminal.Name == "E");
            var t = sets.FirstSet.Find(x => x.NonTerminal.Name == "T");
            var f = sets.FirstSet.Find(x => x.NonTerminal.Name == "F");

            Assert.AreNotEqual(null, zero);
            Assert.AreNotEqual(null, s);
            Assert.AreNotEqual(null, e);
            Assert.AreNotEqual(null, t);
            Assert.AreNotEqual(null, f);

            Assert.AreEqual(true, CompareLists(zero, new List<string>() { "a", "(" }));
            Assert.AreEqual(true, CompareLists(s, new List<string>() { "a", "(" }));
            Assert.AreEqual(true, CompareLists(e, new List<string>() { "a", "(" }));
            Assert.AreEqual(true, CompareLists(t, new List<string>() { "a", "(" }));
            Assert.AreEqual(true, CompareLists(f, new List<string>() { "a", "(" }));
        }

        /// <summary>
        /// Tests if the first set calculation works.
        /// </summary>
        [TestMethod]
        public void FirstSetGeneration_4()
        {
            SyntaxGrammar grammar = new SyntaxGrammar();

            grammar.AddSimpleRule('S', "sE");
            grammar.AddSimpleRule('E', "BD");
            grammar.AddSimpleRule('B', "bB");
            grammar.AddSimpleRule('D', "dD");

            grammar.AddZerothState();

            ParsingSets sets = new ParsingSets(grammar);

            Assert.AreEqual(5, sets.FollowSet.Count);

            var zero = sets.FirstSet.Find(x => x.NonTerminal.Name == "ZERO0");
            var s = sets.FirstSet.Find(x => x.NonTerminal.Name == "S");
            var e = sets.FirstSet.Find(x => x.NonTerminal.Name == "E");
            var b = sets.FirstSet.Find(x => x.NonTerminal.Name == "B");
            var d = sets.FirstSet.Find(x => x.NonTerminal.Name == "D");

            Assert.AreNotEqual(null, zero);
            Assert.AreNotEqual(null, s);
            Assert.AreNotEqual(null, e);
            Assert.AreNotEqual(null, b);
            Assert.AreNotEqual(null, d);

            Assert.AreEqual(true, CompareLists(zero, new List<string>() { "s" }));
            Assert.AreEqual(true, CompareLists(s, new List<string>() { "s" }));
            Assert.AreEqual(true, CompareLists(e, new List<string>() { "b" }));
            Assert.AreEqual(true, CompareLists(b, new List<string>() { "b" }));
            Assert.AreEqual(true, CompareLists(d, new List<string>() { "d" }));
        }

        /// <summary>
        /// Tests if the first set calculation works.
        /// </summary>
        [TestMethod]
        public void FirstSetGeneration_5()
        {
            SyntaxGrammar grammar = new SyntaxGrammar();

            grammar.AddSimpleRule('S', "EBD");
            grammar.AddSimpleRule('E', "eE");
            grammar.AddSimpleRule('B', "");
            grammar.AddSimpleRule('D', "dD");

            grammar.AddZerothState();

            ParsingSets sets = new ParsingSets(grammar);

            Assert.AreEqual(5, sets.FollowSet.Count);

            var zero = sets.FirstSet.Find(x => x.NonTerminal.Name == "ZERO0");
            var s = sets.FirstSet.Find(x => x.NonTerminal.Name == "S");
            var e = sets.FirstSet.Find(x => x.NonTerminal.Name == "E");
            var b = sets.FirstSet.Find(x => x.NonTerminal.Name == "B");
            var d = sets.FirstSet.Find(x => x.NonTerminal.Name == "D");

            Assert.AreNotEqual(null, zero);
            Assert.AreNotEqual(null, s);
            Assert.AreNotEqual(null, e);
            Assert.AreNotEqual(null, b);
            Assert.AreNotEqual(null, d);

            Assert.AreEqual(true, CompareLists(zero, new List<string>() { "e" }));
            Assert.AreEqual(true, CompareLists(s, new List<string>() { "e" }));
            Assert.AreEqual(true, CompareLists(e, new List<string>() { "e" }));
            Assert.AreEqual(true, CompareLists(b, new List<string>() { "ß" }));
            Assert.AreEqual(true, CompareLists(d, new List<string>() { "d" }));
        }

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
            Assert.AreEqual(true, CompareLists(e, new List<string>() { "$", "c" }));
            Assert.AreEqual(true, CompareLists(b, new List<string>() { "d" }));
        }

        /// <summary>
        /// Tests if the follow set calculation work in case of cross-reference in SLR1 grammar.
        /// </summary>
        [TestMethod]
        public void FirstSetGeneration_2()
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

        /// <summary>
        /// Tests if the follow set calculation works.
        /// </summary>
        [TestMethod]
        public void FollowSetGeneration_4()
        {
            SyntaxGrammar grammar = new SyntaxGrammar();

            grammar.AddSimpleRule('S', "EBD");
            grammar.AddSimpleRule('E', "eE");
            grammar.AddSimpleRule('B', "");
            grammar.AddSimpleRule('D', "dD");

            grammar.AddZerothState();

            ParsingSets sets = new ParsingSets(grammar);

            Assert.AreEqual(5, sets.FollowSet.Count);

            var zero = sets.FollowSet.Find(x => x.NonTerminal.Name == "ZERO0");
            var s = sets.FollowSet.Find(x => x.NonTerminal.Name == "S");
            var e = sets.FollowSet.Find(x => x.NonTerminal.Name == "E");
            var b = sets.FollowSet.Find(x => x.NonTerminal.Name == "B");
            var d = sets.FollowSet.Find(x => x.NonTerminal.Name == "D");

            Assert.AreNotEqual(null, zero);
            Assert.AreNotEqual(null, s);
            Assert.AreNotEqual(null, e);
            Assert.AreNotEqual(null, b);
            Assert.AreNotEqual(null, d);

            Assert.AreEqual(true, CompareLists(zero, new List<string>() { "$" }));
            Assert.AreEqual(true, CompareLists(s, new List<string>() { "$" }));
            Assert.AreEqual(true, CompareLists(e, new List<string>() { "d" }));
            Assert.AreEqual(true, CompareLists(b, new List<string>() { "d" }));
            Assert.AreEqual(true, CompareLists(d, new List<string>() { "$" }));
        }

        /// <summary>
        /// Compares a terminal list to a list of strings.
        /// </summary>
        /// <param name="list1">Terminal list</param>
        /// <param name="strList">String list</param>
        /// <returns>True if equal, false otherwise</returns>
        private bool CompareLists(TerminalList list1, List<string> strList)
        {
            TerminalList list2 = new TerminalList(null);

            foreach (var s in strList)
            {
                if ("$" == s)
                {
                    list2.AddTerminal(new SyntaxTreeTerminal(new ExtremalToken()));
                }
                else if ("ß" == s)
                {
                    list2.AddTerminal(new SyntaxTreeTerminal(new EmptyToken()));
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
