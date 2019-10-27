
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
        /// <param name="e">The object to compare to</param>
        /// <returns>True if equal, false otherwise</returns>
        public override bool Equals(Token t)
        {
            if (null == t)
            {
                return false;
            }

            return t is ExtremalToken;
        }

        /// <summary>
        /// Compares two extremal tokens.
        /// All extremal tokens are the same.
        /// </summary>
        /// <param name="obj">The object to compare to</param>
        /// <returns>True if equal, false otherwise</returns>
        public override bool Equals(object obj) => Equals(obj as ExtremalToken);

        /// <summary>
        /// Calculates the has code for the object.
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode() => 0;

    }
}
