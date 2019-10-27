using System;
using System.Collections.Generic;
using Azurite.SyntaxAnalysis;
using Azurite.SyntaxAnalysis.Grammar;
using Azurite.SyntaxAnalysis.ParseConfiguration;
using Azurite.SyntaxAnalysis.ParseSets;
using Azurite.SyntaxAnalysis.SyntaxParsingTable;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AzuriteTest.SyntaAnalysis
{

    /// <summary>
    /// Unit test to test the construction of SLR1 parsing tables.
    /// </summary>
    [TestClass]
    public class ParseTableBuild_IT
    {
        
        /// <summary>
        /// Tests whether the parsing table generation works as expected.
        /// </summary>
        [TestMethod]
        public void ParseTableBuild_1()
        {
            SyntaxGrammar grammar = new SyntaxGrammar();

            grammar.AddSimpleRule('E', "BB");
            grammar.AddSimpleRule('B', "cB");
            grammar.AddSimpleRule('B', "d");

            grammar.AddZerothState();

            ParsingSets sets = new ParsingSets(grammar);

            SLR1Configuration slr1config = new SLR1Configuration(grammar);

            ParsingTable table = new ParsingTable(grammar, slr1config, sets.FirstSet);

            Assert.AreEqual(7, table.Table.Count);
        }

    }
}
