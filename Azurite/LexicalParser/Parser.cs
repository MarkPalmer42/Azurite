
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
        
    }

}
