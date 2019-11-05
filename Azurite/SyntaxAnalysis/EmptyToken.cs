
using Azurite.LexicalParser;

namespace Azurite.SyntaxAnalysis
{

    /// <summary>
    /// This class represents the empty token used for syntax analysis.
    /// The empty token can be used to represent that nothing can be deduced from a rule.
    /// </summary>
    public class EmptyToken : Token
    {

        /// <summary>
        /// Construction of extremal token.
        /// </summary>
        public EmptyToken() : base("ß", 0)
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

            return t is EmptyToken;
        }

        /// <summary>
        /// Compares two extremal tokens.
        /// All extremal tokens are the same.
        /// </summary>
        /// <param name="obj">The object to compare to</param>
        /// <returns>True if equal, false otherwise</returns>
        public override bool Equals(object obj) => Equals(obj as EmptyToken);

        /// <summary>
        /// Calculates the has code for the object.
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode() => 0;

    }
}
