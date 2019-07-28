
namespace Azurite.LexicalParser
{

    /// <summary>
    /// This is an interface for the automata implementations.
    /// </summary>
    interface IAutomata
    {

        /// <summary>
        /// This property stores the previous state of the automata.
        /// </summary>
        AutomataStateType PreviousState { get; }

        /// <summary>
        /// This property stores the current state of the automata.
        /// </summary>
        AutomataStateType CurrentState { get; }

        /// <summary>
        /// Feeds one character to the automata.
        /// The automata then decides what state transition should be
        /// performed and whether the character should be accepted or not.
        /// </summary>
        /// <param name="c">The input character</param>
        void Feed(char c);

        /// <summary>
        /// Resets the state of the automata.
        /// </summary>
        void Reset();

        /// <summary>
        /// Creates a token with the appropriate token type with the given
        /// line number.
        /// </summary>
        /// <param name="line">The line the token was parsed from</param>
        /// <returns>The constructed token</returns>
        Token CreateToken(int line);

    }
}
