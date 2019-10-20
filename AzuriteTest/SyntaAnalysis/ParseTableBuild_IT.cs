using System;
using System.Collections.Generic;
using Azurite.SyntaxAnalysis;
using Azurite.SyntaxAnalysis.Grammar;
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

            List<TerminalList> firstSet = FirstFollowFactory.CalculateFirstSet(grammar);
            List<TerminalList> followSet = FirstFollowFactory.CalculateFollowSet(grammar, firstSet);

            List<List<SLR1Item>> configuration = SLR1ConfigurationFactory.CreateConfiguration(grammar);
            
            ParsingTable table = SLR1ParseTableFactory.BuildParseTable(grammar, configuration, followSet);

            Assert.AreEqual(7, table.parsingTable.Count);
        }

    }
}
