
using System.Collections.Generic;

namespace Azurite.LexicalParser
{

    /// <summary>
    /// This class represent an automata with pre defined state transitions.
    /// </summary>
    public class StatedAutomata : GeneralAutomata
    {

        /// <summary>
        /// The state transitions of the automata.
        /// </summary>
        private List<AutomataStateDefinition> states = new List<AutomataStateDefinition>();

        /// <summary>
        /// Constructor of the stated automata.
        /// </summary>
        /// <param name="tokenInd">The identifier of the token type</param>
        /// <param name="tokenName">The name of the token type</param>
        public StatedAutomata(int tokenInd, string tokenName)
            : base(tokenInd, tokenName)
        {

        }

        /// <summary>
        /// Adds a state to the Stated Automata object.
        /// </summary>
        /// <param name="state">The state to be added</param>
        public void AddStateDefinition(AutomataStateDefinition state)
        {
            states.Add(state);
        }

        /// <summary>
        /// Checks whether the setup is correct or not.
        /// All state ids must match their state's index and each target
        /// value must be a valid state id
        /// </summary>
        /// <returns>True if correct, false otherwise</returns>
        public bool CheckIntegrity()
        {
            bool correct = true;
            int i = 0, j;

            while(correct && i < states.Count)
            {
                correct &= states[i].Id == i;

                j = 0;

                while (correct && j < states[i].StateTransitions.Count)
                {
                    var target = states[i].StateTransitions[j].Target;

                    correct &= target >= 0 && target < states.Count;

                    ++j;
                }

                ++i;
            }

            return correct;
        }

        /// <summary>
        /// Feeds one character to the automata.
        /// The automata then decides what state transition should be
        /// performed and whether the character should be accepted or not.
        /// </summary>
        /// <param name="c">The input character</param>
        public override void Feed(char c)
        {
            if (AutomataStateType.Declined != CurrentState)
            {
                parsedText += c;

                var it = states[stateNum].StateTransitions.Find(x => x.IsCharAccepted(c));

                if (null != it)
                {
                    stateNum = it.Target;
                    CurrentState = states[stateNum].StateType;
                }
                else
                {
                    CurrentState = AutomataStateType.Declined;
                }
            }
        }

    }
}
