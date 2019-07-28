
using Azurite.LexicalParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace AzuriteTest.LexicalParser
{
    /// <summary>
    /// This integration test class is meant to test the stated automata.
    /// The other objects are not mocked, therefore it is considered an 
    /// integration test.
    /// </summary>
    [TestClass]
    public class StatedAutomata_IT
    {

        /// <summary>
        /// Tests whether the CheckIntegrity method correctly
        /// checks the indices of the state definitions.
        /// </summary>
        [TestMethod]
        public void StatedAutomata_CheckIntegrity_1()
        {
            StatedAutomata st1 = new StatedAutomata(0, "token1");
            StatedAutomata st2 = new StatedAutomata(1, "token2");
            StatedAutomata st3 = new StatedAutomata(1, "token2");

            st1.AddStateDefinition(new AutomataStateDefinition(0, AutomataStateType.Undefined));
            st1.AddStateDefinition(new AutomataStateDefinition(1, AutomataStateType.Undefined));

            st2.AddStateDefinition(new AutomataStateDefinition(-1, AutomataStateType.Undefined));
            st2.AddStateDefinition(new AutomataStateDefinition(1, AutomataStateType.Undefined));

            st3.AddStateDefinition(new AutomataStateDefinition(0, AutomataStateType.Undefined));
            st3.AddStateDefinition(new AutomataStateDefinition(2, AutomataStateType.Undefined));

            Assert.AreEqual(true, st1.CheckIntegrity());
            Assert.AreEqual(false, st2.CheckIntegrity());
            Assert.AreEqual(false, st3.CheckIntegrity());
        }

        /// <summary>
        /// Tests if the CheckIntegrity method correctly checks if the
        /// target values are inside the allowed boundary.
        /// </summary>
        [TestMethod]
        public void StatedAutomata_CheckIntegrity_2()
        {
            StateTransition trans0 = new StateTransition(true, 0);
            trans0.Target = 0;
            AutomataStateDefinition asd0 = new AutomataStateDefinition(0, AutomataStateType.Undefined);
            asd0.AddStateTransition(trans0);
            StatedAutomata st0 = new StatedAutomata(0, "token1");
            st0.AddStateDefinition(asd0);

            StateTransition trans1 = new StateTransition(true, 0);
            trans1.Target = 0;
            AutomataStateDefinition asd1 = new AutomataStateDefinition(-1, AutomataStateType.Undefined);
            asd1.AddStateTransition(trans1);
            StatedAutomata st1 = new StatedAutomata(0, "token1");
            st1.AddStateDefinition(asd1);

            StateTransition trans2 = new StateTransition(true, 0);
            trans2.Target = 0;
            AutomataStateDefinition asd2 = new AutomataStateDefinition(1, AutomataStateType.Undefined);
            asd2.AddStateTransition(trans2);
            StatedAutomata st2 = new StatedAutomata(0, "token1");
            st2.AddStateDefinition(asd2);

            Assert.AreEqual(true, st0.CheckIntegrity());
            Assert.AreEqual(false, st1.CheckIntegrity());
            Assert.AreEqual(false, st2.CheckIntegrity());
        }

        /// <summary>
        /// This test tests a real use case where an integer
        /// automata is created, negative and positive integers are accepted.
        /// </summary>
        [TestMethod]
        public void IntegerAutomata_Test()
        {
            AutomataStateDefinition state0 = new AutomataStateDefinition(0, AutomataStateType.Undefined);
            AutomataStateDefinition state1 = new AutomataStateDefinition(1, AutomataStateType.Undefined);
            AutomataStateDefinition state2 = new AutomataStateDefinition(2, AutomataStateType.Accepted);
            AutomataStateDefinition state3 = new AutomataStateDefinition(3, AutomataStateType.Accepted);

            StateTransition trans0_0 = new StateTransition(false, 1);
            StateTransition trans0_1 = new StateTransition(false, 2);
            StateTransition trans0_2 = new StateTransition(false, 3);

            StateTransition trans1 = new StateTransition(false, 3);
            StateTransition trans2 = new StateTransition(false, 3);
            StateTransition trans3 = new StateTransition(false, 3);

            trans0_0.AddAcceptedChar('-');
            trans0_1.AddAcceptedChar('0');
            trans0_2.AddAcceptedInterval('1', '9');

            trans1.AddAcceptedInterval('1', '9');
            trans2.AddAcceptedInterval('1', '9');
            trans3.AddAcceptedInterval('0', '9');

            state0.AddStateTransition(trans0_0);
            state0.AddStateTransition(trans0_1);
            state0.AddStateTransition(trans0_2);

            state1.AddStateTransition(trans1);
            state2.AddStateTransition(trans2);
            state3.AddStateTransition(trans3);

            StatedAutomata st = new StatedAutomata(0, "integer");

            st.AddStateDefinition(state0);
            st.AddStateDefinition(state1);
            st.AddStateDefinition(state2);
            st.AddStateDefinition(state3);

            Assert.AreEqual(true, st.CheckIntegrity());

            List<string> accepted = new List<string>(){ "12", "149", "-25", "0", "-42" };
            List<string> declined = new List<string>(){ "00", "-0", "abc" };
            
            Action<StatedAutomata, List<string>, AutomataStateType> test = (s, l, t) =>
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
                        Assert.AreEqual("integer", st.CreateToken(15).TokenTypeName);
                    }
                }
            };

            test(st, accepted, AutomataStateType.Accepted);
            test(st, declined, AutomataStateType.Declined);
        }

    }
}
