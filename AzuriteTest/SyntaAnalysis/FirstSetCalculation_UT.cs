using System;
using System.Collections.Generic;
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
    public class FirstSetCalculation_UT
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

            ParsingSets sets = new ParsingSets(grammar);

            Assert.AreEqual(3, sets.FirstSet.Count);

            Assert.AreEqual("S", sets.FirstSet[0].NonTerminal.Name);
            Assert.AreEqual("E", sets.FirstSet[1].NonTerminal.Name);
            Assert.AreEqual("B", sets.FirstSet[2].NonTerminal.Name);

            Assert.AreEqual(2, sets.FirstSet[0].Terminals.Count);
            Assert.AreEqual(2, sets.FirstSet[1].Terminals.Count);
            Assert.AreEqual(2, sets.FirstSet[2].Terminals.Count);

            Assert.AreNotEqual(-1, sets.FirstSet[0].Terminals.FindIndex(x => x.SyntaxToken.Text == "d"));
            Assert.AreNotEqual(-1, sets.FirstSet[0].Terminals.FindIndex(x => x.SyntaxToken.Text == "c"));

            Assert.AreNotEqual(-1, sets.FirstSet[1].Terminals.FindIndex(x => x.SyntaxToken.Text == "d"));
            Assert.AreNotEqual(-1, sets.FirstSet[1].Terminals.FindIndex(x => x.SyntaxToken.Text == "c"));

            Assert.AreNotEqual(-1, sets.FirstSet[2].Terminals.FindIndex(x => x.SyntaxToken.Text == "d"));
            Assert.AreNotEqual(-1, sets.FirstSet[2].Terminals.FindIndex(x => x.SyntaxToken.Text == "c"));
        }
    }
}
