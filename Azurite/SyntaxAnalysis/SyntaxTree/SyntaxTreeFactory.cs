
using Azurite.LexicalParser;

namespace Azurite.SyntaxAnalysis.SyntaxTree
{

    /// <summary>
    /// Factory class to instantiate Syntax Tree elements.
    /// </summary>
    public static class SyntaxTreeFactory
    {

        /// <summary>
        /// Instantiates a non terminal symbol object.
        /// </summary>
        /// <param name="name">The name of the nonterminal</param>
        /// <returns>The object instantiated</returns>
        public static SyntaxTreeElement CreateNonterminal(string name)
        {
            return new SyntaxTreeNonterminal(name);
        }

        /// <summary>
        /// Instantiates a terminal symbol object.
        /// </summary>
        /// <param name="token">The token of the terminal symbol</param>
        /// <returns>The object instantiated</returns>
        public static SyntaxTreeElement CreateTerminal(Token token)
        {
            return new SyntaxTreeTerminal(token);
        }

    }
}
