using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azurite.LexicalParser
{

    /// <summary>
    /// An abstract class that implements methods needed for
    /// each final implementation of the IAutomata interface.
    /// </summary>
    public abstract class GeneralAutomata : IAutomata
    {

        /// <summary>
        /// The current state of the automata.
        /// The starting state is always 0.
        /// </summary>
        protected int stateNum;

        /// <summary>
        /// The index of the token's type.
        /// </summary>
        protected int tokenType;

        /// <summary>
        /// The name of the token's type.
        /// </summary>
        protected string tokenTypeName;

        /// <summary>
        /// The text that was supplied until the automata declined it.
        /// </summary>
        protected string parsedText;

        /// <summary>
        /// A member for the property CurrentState.
        /// </summary>
        private AutomataStateType currentState;

        /// <summary>
        /// This property stores the previous state of the automata.
        /// </summary>
        public AutomataStateType PreviousState { get; private set; }

        /// <summary>
        /// This property stores the current state of the automata.
        /// </summary>
        public AutomataStateType CurrentState
        {
            get
            {
                return currentState;
            }
            set
            {
                /* The previous state should be assigned with the current. */
                PreviousState = currentState;
                currentState = value;
            }
        }

        /// <summary>
        /// The constructor of the abstract general automata class.
        /// </summary>
        /// <param name="tokenIndex">The index of the token type</param>
        /// <param name="tokenName">The name of the token type</param>
        public GeneralAutomata(int tokenIndex, string tokenName)
        {
            tokenType = tokenIndex;
            tokenTypeName = tokenName;
        }

        /// <summary>
        /// Feeds one character to the automata.
        /// The automata then decides what state transition should be
        /// performed and whether the character should be accepted or not.
        /// 
        /// This remains abstract because there are more than one types of
        /// automatas that need to have their own acceptance criteria.
        /// </summary>
        /// <param name="c">The input character</param>
        public abstract void Feed(char c);

        /// <summary>
        /// Resets the state of the automata.
        /// </summary>
        public void Reset()
        {
            parsedText = "";
            stateNum = 0;
            CurrentState = AutomataStateType.Undefined;
            PreviousState = AutomataStateType.Undefined;
        }

        /// <summary>
        /// Creates a token with the appropriate token type with the given
        /// line number.
        /// </summary>
        /// <param name="line">The line the token was parsed from</param>
        /// <returns>The constructed token</returns>
        public Token CreateToken(int line)
        {
            Token t;

            if (AutomataStateType.Declined == CurrentState)
            {
                t = new Token(parsedText.Substring(0, parsedText.Count() - 1), line);
            }
            else
            {
                t = new Token(parsedText, line);
            }
            
            t.SetTokenType(tokenType, tokenTypeName);

            return t;

        }

        /// <summary>
        /// Decides whether the currently recognized token should be discarded.
        /// </summary>
        /// <returns>True if should be discarded, false otherwise</returns>
        public bool IsDiscarded()
        {
            return -1 == tokenType;
        }

    }
}
