
using System.Collections.Generic;

namespace Azurite.LexicalParser
{
    /// <summary>
    /// A definition of an automata state.
    /// Used for state automatas to store the states and
    /// their possible transitions.
    /// </summary>
    public class AutomataStateDefinition
    {

        /// <summary>
        /// The identifier of this automata state.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The list of the possible state transitions.
        /// </summary>
        public List<StateTransition> StateTransitions { get; private set; }

        /// <summary>
        /// The automata state type.
        /// </summary>
        public AutomataStateType StateType { get; private set; }

        /// <summary>
        /// The constructor of the automata state definition.
        /// </summary>
        public AutomataStateDefinition(int id, AutomataStateType stateType)
        {
            Id = id;
            StateType = stateType;
            StateTransitions = new List<StateTransition>();
        }

        /// <summary>
        /// Adds a state transition to the list of transitions.
        /// </summary>
        /// <param name="st">The state transition to be added</param>
        public void AddStateTransition(StateTransition st)
        {
            StateTransitions.Add(st);
        }

    }
}
