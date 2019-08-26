
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

        public List<Token> Parse(string input)
        {
            List<Token> tokenList = new List<Token>();
            
            int i = 0;

            automataList.ForEach(x => x.Reset());

            while (i < input.Length)
            {
                automataList.ForEach(x => x.Feed(input[i]));

                bool allDeclined = automataList.TrueForAll(x => x.CurrentState == AutomataStateType.Declined);

                if (allDeclined)
                {
                    var lastAccept = automataList.Find(x => x.PreviousState == AutomataStateType.Accepted);

                    if (default(IAutomata) == lastAccept)
                    {
                        throw new System.Exception("Parse error on input character " + input[i]);
                    }

                    if (!lastAccept.IsDiscarded())
                    {
                        tokenList.Add(lastAccept.CreateToken(0));
                    }
                    
                    automataList.ForEach(x => x.Reset());
                }
                else
                {
                    ++i;
                }
            }

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
