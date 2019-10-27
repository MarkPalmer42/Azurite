
using Azurite.LexicalParser;

namespace Azurite.SyntaxAnalysis
{

    /// <summary>
    /// This class represents the exremal token used for syntax analysis.
    /// The extremal token represents the end of file (EOF).
    /// </summary>
    public class ExtremalToken : Token
    {

        /// <summary>
        /// Construction of extremal token.
        /// </summary>
        public ExtremalToken() : base("$", 0)
        {

        }

        /// <summary>
        /// Compares two extremal tokens.
        /// All extremal tokens are the same.
        /// </summary>
        /// <param name="obj">The object to compare to</param>
        /// <returns>True if equal, false otherwise</returns>
        public override bool Equals(Token t)
        {
            return t is ExtremalToken;
        }

    }
}
