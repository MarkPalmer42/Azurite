
using Azurite.LexicalParser;

namespace Azurite.SyntaxAnalysis.SyntaxTree
{

    /// <summary>
    /// Represents a terminal symbol in the syntax tree.
    /// </summary>
    public class SyntaxTreeTerminal : SyntaxTreeElement
    {

        /// <summary>
        /// The token that belongs to the syntax tree terminal symbol.
        /// </summary>
        public Token SyntaxToken { get; private set; }

        /// <summary>
        /// The constructor of the terminal element.
        /// </summary>
        /// <param name="token">The token of the terminal element</param>
        public SyntaxTreeTerminal(Token token)
        {
            SyntaxToken = token;
        }

        /// <summary>
        /// Compares two terminal elements.
        /// </summary>
        /// <param name="obj">The object to compare to</param>
        /// <returns>0 if equal, 1 if not equal, -1 in case of incorrect object type</returns>
        public override int CompareTo(object obj)
        {
            SyntaxTreeTerminal nt = obj as SyntaxTreeTerminal;

            if (null == nt)
            {
                return -1;
            }
            else
            {
                return SyntaxToken.CompareTo(nt.SyntaxToken);
            }
        }

    }

}
