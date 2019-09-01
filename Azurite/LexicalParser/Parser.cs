
using System.Collections.Generic;

namespace Azurite.LexicalParser
{

    /// <summary>
    /// The lexical parser.
    /// </summary>
    public class Parser
    {

        /// <summary>
        /// The list of the automatas.
        /// </summary>
        List<IAutomata> automataList;

        /// <summary>
        /// Constructor of the parser class.
        /// </summary>
        /// <param name="xmlPath">The path of the XML file</param>
        /// <param name="xsdPath">The path of the XSD file</param>
        public Parser(string xmlPath, string xsdPath)
        {
            XMLParserReader reader = new XMLParserReader();

            automataList = reader.ReadXML(xmlPath, xsdPath);
        }

        /// <summary>
        /// Parses the input string and creates a list of tokens.
        /// May throw exception in case of parse error.
        /// </summary>
        /// <param name="input">The input string</param>
        /// <returns>The list of tokens</returns>
        public List<Token> Parse(string input)
        {
            List<Token> tokenList = new List<Token>();
            
            int i = 0;

            /* Resetting all automatas. */
            automataList.ForEach(x => x.Reset());

            while (i < input.Length)
            {
                /* Feed the character to all automatas. */
                automataList.ForEach(x => x.Feed(input[i]));

                /* Check if all automatas rejected the character. */
                bool allDeclined = automataList.TrueForAll(x => x.CurrentState == AutomataStateType.Declined);

                if (allDeclined)
                {
                    /* Find the automata that was previously in accepting state. */
                    var lastAccept = automataList.Find(x => x.PreviousState == AutomataStateType.Accepted);

                    /* In case none of them were accepting, we throw parse error exception. */
                    if (default(IAutomata) == lastAccept)
                    {
                        throw new System.Exception("Parse error on input character " + input[i]);
                    }

                    /* Check if the token needs to be disacrded or not. */
                    if (!lastAccept.IsDiscarded())
                    {
                        tokenList.Add(lastAccept.CreateToken(0));
                    }

                    /* Resetting all automatas. */
                    automataList.ForEach(x => x.Reset());
                }
                else
                {
                    /* Only increment if not all automatas rejected the character,
                     * the character has to be fed again after reset if all automatas rejected it.*/
                    ++i;
                }
            }

            /* We need to find the first currently accepting state automata after traversing. */
            var currentAccept = automataList.Find(x => x.CurrentState == AutomataStateType.Accepted);

            if (default(IAutomata) == currentAccept)
            {
                throw new System.Exception("Parse error on input character " + input[input.Length - 1]);
            }

            if (!currentAccept.IsDiscarded())
            {
                tokenList.Add(currentAccept.CreateToken(0));
            }

            return tokenList;
        }

    }

}
