using System;
using Azurite.SyntaxAnalysis.Grammar;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AzuriteTest.SyntaAnalysis
{
    [TestClass]
    public class Grammar_UT
    {

        [TestMethod]
        public void SimpleGrammar_Construction()
        {
            SyntaxGrammar g = new SyntaxGrammar();

            g.AddSimpleRule('S', "EE");
            g.AddSimpleRule('E', "ab");

            Assert.AreEqual(2, g.ProductionRules.Count);
            Assert.AreEqual("S", g.ProductionRules[0].LeftSide.Name);
            Assert.AreEqual("E", g.ProductionRules[1].LeftSide.Name);

            Assert.AreEqual(2, g.ProductionRules[0].RightSide.Count);
            Assert.AreEqual(2, g.ProductionRules[1].RightSide.Count);
        }

        [TestMethod]
        public void SimpleGrammar_Validation_1()
        {
            SyntaxGrammar g1 = new SyntaxGrammar();

            g1.AddSimpleRule('S', "EE");
            g1.AddSimpleRule('E', "ab");

            SyntaxGrammar g2 = new SyntaxGrammar();

            g2.AddSimpleRule('E', "BB");
            g2.AddSimpleRule('B', "cB");
            g2.AddSimpleRule('B', "d");

            SyntaxGrammar g3 = new SyntaxGrammar();

            g3.AddSimpleRule('X', "Yz");
            g3.AddSimpleRule('Y', "bZ");
            g3.AddSimpleRule('Y', "");
            g3.AddSimpleRule('Z', "");

            Assert.AreEqual(true, g1.IsValid());
            Assert.AreEqual(true, g2.IsValid());
            Assert.AreEqual(true, g3.IsValid());
        }

        [TestMethod]
        public void SimpleGrammar_Validation_2()
        {
            SyntaxGrammar g1 = new SyntaxGrammar();

            g1.AddSimpleRule('S', "EE");
            g1.AddSimpleRule('S', "b");
            g1.AddSimpleRule('E', "ab");

            SyntaxGrammar g2 = new SyntaxGrammar();

            g2.AddSimpleRule('E', "BB");
            g2.AddSimpleRule('B', "cD");
            g2.AddSimpleRule('B', "d");

            Assert.AreEqual(false, g1.IsValid());
            Assert.AreEqual(false, g2.IsValid());
        }

    }
}
