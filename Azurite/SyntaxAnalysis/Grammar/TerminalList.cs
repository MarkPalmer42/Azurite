
using System.Collections.Generic;
using Azurite.SyntaxAnalysis.SyntaxTree;

namespace Azurite.SyntaxAnalysis.Grammar
{

    public class TerminalList
    {

        public SyntaxTreeNonterminal NonTerminal { get; set; }

        public List<SyntaxTreeTerminal> Terminals { get; set; }

        public TerminalList(SyntaxTreeNonterminal nonTerminal)
        {
            NonTerminal = nonTerminal;
        }

    }

}
