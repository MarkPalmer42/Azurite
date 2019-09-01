
using Azurite.LexicalParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace AzuriteTest.LexicalParser
{
    /// <summary>
    /// This unit test class is meant to test the stateless automata.
    /// </summary>
    [TestClass]
    public class StatelessAutomata_UT
    {

        /// <summary>
        /// This method performs positive and negative tests on the stateless automata.
        /// </summary>
        [TestMethod]
        public void StatelessAutomata_Test()
        {
            StatelessAutomata st = new StatelessAutomata(0, "st");

            st.AddAcceptedText("text");
            st.AddAcceptedText("text22");

            List<string> accepted = new List<string>() { "text", "text22" };
            List<string> declined = new List<string>() { "text23", "teb", "abc" };

            Action<StatelessAutomata, List<string>, AutomataStateType> test = (s, l, t) =>
            {
                foreach (var str in l)
                {
                    s.Reset();

                    foreach (var c in str)
                    {
                        s.Feed(c);
                    }

                    Assert.AreEqual(t, st.CurrentState);

                    if (AutomataStateType.Accepted == t)
                    {
                        Assert.AreEqual(str, st.CreateToken(15).Text);
                        Assert.AreEqual(0, st.CreateToken(15).TokenType);
                        Assert.AreEqual("st", st.CreateToken(15).TokenTypeName);
                    }
                }
            };

            test(st, accepted, AutomataStateType.Accepted);
            test(st, declined, AutomataStateType.Declined);
        }

    }
}
