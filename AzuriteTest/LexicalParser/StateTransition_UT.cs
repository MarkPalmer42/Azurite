
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Azurite.LexicalParser;

namespace AzuriteTest.LexicalParser
{
    /// <summary>
    /// A unit test class for the state transition class.
    /// </summary>
    [TestClass]
    public class StateTransition_UT
    {

        /// <summary>
        /// This unit test tests whether the any character
        /// acceptance mode works as expected: any character
        /// should be accepted with a list of exceptions.
        /// </summary>
        [TestMethod]
        public void StateTransition_AnyCharAcceptance()
        {
            StateTransition st = new StateTransition(true, 0);
            
            st.AddException('t');
            st.AddException('X');

            Assert.AreEqual(true, st.IsCharAccepted('a'));
            Assert.AreEqual(true, st.IsCharAccepted('x'));
            Assert.AreEqual(true, st.IsCharAccepted('\t'));
            Assert.AreEqual(false, st.IsCharAccepted('t'));
            Assert.AreEqual(false, st.IsCharAccepted('X'));
        }

        /// <summary>
        /// Thsi unti tests tests the case where a list of
        /// intervals and characters are accepted.
        /// </summary>
        [TestMethod]
        public void StateTransition_ListAcceptance()
        {
            StateTransition st = new StateTransition(false, 0);
            
            st.AddAcceptedChar('t');
            st.AddAcceptedChar('X');
            st.AddAcceptedInterval('a', 'c');

            Assert.AreEqual(true, st.IsCharAccepted('a'));
            Assert.AreEqual(false, st.IsCharAccepted('x'));
            Assert.AreEqual(false, st.IsCharAccepted('\t'));
            Assert.AreEqual(true, st.IsCharAccepted('c'));
            Assert.AreEqual(false, st.IsCharAccepted('d'));
            Assert.AreEqual(true, st.IsCharAccepted('X'));
        }

    }
}
