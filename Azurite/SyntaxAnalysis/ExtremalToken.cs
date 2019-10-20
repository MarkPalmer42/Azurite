using Azurite.LexicalParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <returns>0 if obj is ExtremalToken, -1 otherwise</returns>
        public override int CompareTo(object obj)
        {
            return (obj is ExtremalToken) ? 0 : -1;
        }

    }
}
