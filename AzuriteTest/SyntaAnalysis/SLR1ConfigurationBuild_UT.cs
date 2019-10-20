
using Azurite.SyntaxAnalysis;
using Azurite.SyntaxAnalysis.Grammar;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AzuriteTest.SyntaAnalysis
{

    /// <summary>
    /// Unit test for the construction of the SLR1 configuration.
    /// </summary>
    [TestClass]
    public class SLR1ConfigurationBuild_UT
    {

        /// <summary>
        /// Tests whether the configuration construction works correctly.
        /// </summary>
        [TestMethod]
        public void ConfigurationBuild_1()
        {
            SyntaxGrammar grammar = new SyntaxGrammar();

            grammar.AddSimpleRule('E', "BB");
            grammar.AddSimpleRule('B', "cB");
            grammar.AddSimpleRule('B', "d");

            List<List<SLR1Item>> configuration = SLR1ConfigurationFactory.CreateConfiguration(grammar);

            Assert.AreEqual(7, configuration.Count);

            Assert.AreEqual("E", configuration[0][1].Rule.LeftSide.Name);
            Assert.AreEqual(0, configuration[0][1].State);

            Assert.AreEqual("B", configuration[0][2].Rule.LeftSide.Name);
            Assert.AreEqual(0, configuration[0][2].State);

            Assert.AreEqual("B", configuration[0][3].Rule.LeftSide.Name);
            Assert.AreEqual(0, configuration[0][3].State);
        }

    }
}
