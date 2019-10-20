
using Azurite.SyntaxAnalysis.Grammar;
using Azurite.SyntaxAnalysis.SyntaxParsingTable;
using System.Collections.Generic;

namespace Azurite.SyntaxAnalysis
{

    /// <summary>
    /// Factory to generate first and follow sets based on the input grammar.
    /// </summary>
    public class FirstFollowFactory
    {

        public static List<TerminalList> CalculateFirstSet(List<GrammarRule> grammar)
        {
            List<TerminalList> firstSet = new List<TerminalList>();

            return firstSet;
        }

        public static List<TerminalList> CalculateFollowSet(List<GrammarRule> grammar, List<TerminalList> firstSet)
        {
            List<TerminalList> followSet = new List<TerminalList>();

            return followSet;
        }

    }

}
