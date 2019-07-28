
using System.Collections.Generic;

namespace Azurite.LexicalParser
{
    /// <summary>
    /// Stateless automata that accepts a given list of strings
    /// and nothing else.
    /// </summary>
    public class StatelessAutomata : GeneralAutomata
    {

        /// <summary>
        /// The list of accepted strings.
        /// </summary>
        List<string> acceptedTexts = new List<string>();

        /// <summary>
        /// The constructor of the stateless automata.
        /// </summary>
        /// <param name="tokenInd">The identifier of the token type</param>
        /// <param name="tokenName">The name of the token type</param>
        public StatelessAutomata(int tokenInd, string tokenName)
            : base(tokenInd, tokenName)
        {

        }

        /// <summary>
        /// Adds a string to the list of accepted texts.
        /// </summary>
        /// <param name="text">The string to be added</param>
        public void AddAcceptedText(string text)
        {
            acceptedTexts.Add(text);
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

                int ind = acceptedTexts.FindIndex(x => x.StartsWith(parsedText));

                if (-1 == ind)
                {
                    CurrentState = AutomataStateType.Declined;
                }
                else if (acceptedTexts[ind] == parsedText)
                {
                    CurrentState = AutomataStateType.Accepted;
                }
                else
                {
                    CurrentState = AutomataStateType.Undefined;
                }
            }
        }

    }
}
