
using Azurite.LexicalParser;
using Azurite.SyntaxAnalysis;
using Azurite.SyntaxAnalysis.Grammar;
using Azurite.SyntaxAnalysis.SyntaxParsingTable;
using Azurite.SyntaxAnalysis.SyntaxTree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

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

            List<TerminalList> terminalLists = new List<TerminalList>();

            List<TerminalList> firstSet = FirstFollowFactory.CalculateFirstSet(grammar);
            terminalLists = FirstFollowFactory.CalculateFollowSet(grammar, firstSet);

            Assert.AreEqual(3, terminalLists.Count);

            Assert.AreEqual("S", terminalLists[0].NonTerminal.Name);
            Assert.AreEqual("E", terminalLists[1].NonTerminal.Name);
            Assert.AreEqual("B", terminalLists[2].NonTerminal.Name);

            Assert.AreEqual(1, terminalLists[0].Terminals.Count);
            Assert.AreEqual(2, terminalLists[1].Terminals.Count);
            Assert.AreEqual(1, terminalLists[2].Terminals.Count);

            Assert.AreNotEqual(-1, terminalLists[0].Terminals[0].SyntaxToken.CompareTo(new ExtremalToken()) == 0);

            Assert.AreNotEqual(-1, terminalLists[1].Terminals.FindIndex(x => x.SyntaxToken.Text == "c"));
            Assert.AreNotEqual(-1, terminalLists[1].Terminals.FindIndex(x => x.SyntaxToken.CompareTo(new ExtremalToken()) == 0));

            Assert.AreNotEqual(-1, terminalLists[2].Terminals[0].SyntaxToken.Text == "d");
        }

    }
}
