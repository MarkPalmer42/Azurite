
using Azurite.SyntaxAnalysis.Grammar;
using Azurite.SyntaxAnalysis.SyntaxParsingTable;
using Azurite.SyntaxAnalysis.SyntaxTree;
using System.Collections.Generic;

namespace Azurite.SyntaxAnalysis
{

    /// <summary>
    /// Factory class to create SLR1 parsing tables.
    /// </summary>
    public static class SLR1ParseTableFactory
    {

        /// <summary>
        /// Builds a parse table based on the grammar provided.
        /// </summary>
        /// <param name="grammar">The grammar</param>
        /// <returns>A parsing table</returns>
        public static ParsingTable BuildParseTable(SyntaxGrammar grammar, List<TerminalList> firstSet)
        {
            if (!grammar.IsValid())
            {
                throw new System.Exception("Cannot build parse table with invalid input grammar.");
            }

            ParsingTable table = new ParsingTable();
            
            

            return table;
        }

    }

}
