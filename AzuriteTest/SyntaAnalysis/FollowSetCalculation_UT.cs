
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
            List<GrammarRule> grammar = new List<GrammarRule>();

            for (int i = 0; i < 5; ++i)
            {
                grammar.Add(new GrammarRule());
            }

            grammar[0].LeftSide = new SyntaxTreeNonterminal("S");
            grammar[0].RightSide = new List<SyntaxTreeElement>();
            grammar[0].RightSide.Add(new SyntaxTreeNonterminal("E"));

            grammar[1].LeftSide = new SyntaxTreeNonterminal("E");
            grammar[1].RightSide = new List<SyntaxTreeElement>();
            grammar[1].RightSide.Add(new SyntaxTreeNonterminal("B"));
            grammar[1].RightSide.Add(new SyntaxTreeTerminal(new Token("d", 0)));

            grammar[2].LeftSide = new SyntaxTreeNonterminal("B");
            grammar[2].RightSide = new List<SyntaxTreeElement>();
            grammar[2].RightSide.Add(new SyntaxTreeNonterminal("E"));
            grammar[2].RightSide.Add(new SyntaxTreeTerminal(new Token("c", 0)));

            grammar[3].LeftSide = new SyntaxTreeNonterminal("E");
            grammar[3].RightSide = new List<SyntaxTreeElement>();
            grammar[3].RightSide.Add(new SyntaxTreeTerminal(new Token("c", 0)));

            grammar[4].LeftSide = new SyntaxTreeNonterminal("B");
            grammar[4].RightSide = new List<SyntaxTreeElement>();
            grammar[4].RightSide.Add(new SyntaxTreeTerminal(new Token("d", 0)));

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

            Assert.AreNotEqual(-1, terminalLists[0].Terminals[0].SyntaxToken.Text == "$");

            Assert.AreNotEqual(-1, terminalLists[1].Terminals.FindIndex(x => x.SyntaxToken.Text == "c"));
            Assert.AreNotEqual(-1, terminalLists[1].Terminals.FindIndex(x => x.SyntaxToken.Text == "$"));

            Assert.AreNotEqual(-1, terminalLists[2].Terminals[0].SyntaxToken.Text == "d");
        }

    }
}
