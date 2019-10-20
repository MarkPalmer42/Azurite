
using Azurite.SyntaxAnalysis.SyntaxTree;

namespace Azurite.SyntaxAnalysis
{

    /// <summary>
    /// The parsing stack element used to parse with an SLR1 parsing table.
    /// </summary>
    public class ParsingStackElement
    {

        /// <summary>
        /// Terminal or nonterminal symbol.
        /// </summary>
        public SyntaxTreeElement Element { get; private set; }

        /// <summary>
        /// The value attached to the current stack element.
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// Constructor of the Parsing Satck element
        /// </summary>
        /// <param name="ste">Terminal or nonterminal symbol</param>
        /// <param name="val">The value</param>
        public ParsingStackElement(SyntaxTreeElement ste, int val)
        {
            Element = ste;
            Value = val;
        }

    }
}
