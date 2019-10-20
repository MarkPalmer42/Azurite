
using Azurite.SyntaxAnalysis.SyntaxTree;
using System.Collections.Generic;

namespace Azurite.SyntaxAnalysis.SyntaxParsingTable
{

    /// <summary>
    /// A parsing table that is generated based on the grammar.
    /// </summary>
    public class ParsingTable
    {

        /// <summary>
        /// Header of the parsing table, it contains terminals and nonterminals
        /// and also the extermal symbol (which represent the end of file).
        /// </summary>
        public List<SyntaxTreeElement> parsingTableHeader { get; set; }

        /// <summary>
        /// The parsing table.
        /// </summary>
        public List<List<ParsingTableElement>> parsingTable { get; set; }

        /// <summary>
        /// Constructor of the parsing table.
        /// </summary>
        public ParsingTable()
        {
            parsingTableHeader = new List<SyntaxTreeElement>();

            parsingTable = new List<List<ParsingTableElement>>();
        }

        /// <summary>
        /// Adds a header to the parsing table.
        /// </summary>
        /// <param name="terminal">Terminal or nonterminal symbol to add</param>
        public void AddHeader(SyntaxTreeElement terminal)
        {
            /* Throw exception in case the entire parsing table is already generated. */
            if (parsingTable.Count > 0)
            {
                throw new System.Exception("Unable to modify header after a row has been added.");
            }

            parsingTableHeader.Add(terminal);
        }

        /// <summary>
        /// Adds a row to the parsing table.
        /// </summary>
        public void AddRow()
        {
            parsingTable.Add(new List<ParsingTableElement>());

            for (int i = 0; i < parsingTableHeader.Count; ++i)
            {
                parsingTable[parsingTable.Count - 1].Add(new ParsingTableElement(ParsingTableElementType.error));
            }
        }

    }
}
