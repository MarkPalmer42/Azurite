using Azurite.SyntaxAnalysis.SyntaxTree;
using System.Collections.Generic;

namespace Azurite.SyntaxAnalysis.SyntaxParsingTable
{
    class ParsingTable
    {

        public List<SyntaxTreeElement> parsingTableHeader { get; set; }

        public List<List<ParsingTableElement>> parsingTable { get; set; }

        public ParsingTable()
        {
            parsingTableHeader = new List<SyntaxTreeElement>();

            parsingTable = new List<List<ParsingTableElement>>();
        }

        public void AddHeader(SyntaxTreeElement terminal)
        {
            if (parsingTable.Count > 0)
            {
                throw new System.Exception("Unable to modify header after a row has been added.");
            }

            parsingTableHeader.Add(terminal);
        }

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
