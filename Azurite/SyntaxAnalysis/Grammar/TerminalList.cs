
using System.Collections.Generic;
using Azurite.SyntaxAnalysis.SyntaxTree;

namespace Azurite.SyntaxAnalysis.Grammar
{

    /// <summary>
    /// Terminal list that represents a first or a follow set of a
    /// nonterminal symbol that is generated based on the input grammar.
    /// </summary>
    public class TerminalList
    {

        /// <summary>
        /// The nonterminal symbol.
        /// </summary>
        public SyntaxTreeNonterminal NonTerminal { get; set; }

        /// <summary>
        /// The terminals belonging to the nonterminal.
        /// </summary>
        public List<SyntaxTreeTerminal> Terminals { get; set; }

        /// <summary>
        /// The constructor of the TerminalList.
        /// </summary>
        /// <param name="nonTerminal">The nonterminal</param>
        public TerminalList(SyntaxTreeNonterminal nonTerminal)
        {
            NonTerminal = nonTerminal;
            Terminals = new List<SyntaxTreeTerminal>();
        }

    }

}
