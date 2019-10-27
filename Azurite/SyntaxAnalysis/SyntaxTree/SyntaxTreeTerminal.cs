
using Azurite.LexicalParser;
using System;

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
        /// <param name="e">The object to compare to</param>
        /// <returns>True if equal, false otherwise</returns>
        public override bool Equals(SyntaxTreeElement e)
        {
            SyntaxTreeTerminal t = e as SyntaxTreeTerminal;
            return t != null && SyntaxToken.Equals(t.SyntaxToken);
        }

        /// <summary>
        /// Compares two terminal elements.
        /// </summary>
        /// <param name="obj">The object to compare to</param>
        /// <returns>True if equal, false otherwise</returns>
        public override bool Equals(object obj) => Equals(obj as SyntaxTreeTerminal);

        /// <summary>
        /// Calculates the has code for the object.
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode() => SyntaxToken.GetHashCode();

    }

}
