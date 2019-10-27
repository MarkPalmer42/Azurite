
using System;

namespace Azurite.LexicalParser
{
    /// <summary>
    /// The token type.
    /// </summary>
    public class Token : IComparable
    {

        /// <summary>
        /// The parsed text.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// The line the token was parsed from.
        /// </summary>
        public int Line { get; private set; }

        /// <summary>
        /// The index of the token type in the list
        /// of all token types.
        /// </summary>
        public int TokenType { get; private set; }

        /// <summary>
        /// The name of the token type.
        /// </summary>
        public string TokenTypeName { get; private set; }

        /// <summary>
        /// True if the text is relevant during comparison.
        /// </summary>
        public bool IsTextRelevant { get; set; }

        /// <summary>
        /// Constructor of the token class.
        /// </summary>
        /// <param name="text">The parsed text</param>
        /// <param name="line">The line the token was parsed from</param>
        public Token(string text, int line)
        {
            Text = text;
            Line = line;

            IsTextRelevant = true;
        }

        /// <summary>
        /// Sets the type of the token
        /// </summary>
        /// <param name="type">Index of the token type</param>
        /// <param name="typeName">Name of the token type.</param>
        public void SetTokenType(int type, string typeName)
        {
            TokenType = type;
            TokenTypeName = typeName;
        }

        /// <summary>
        /// Compares this token instance to an object.
        /// </summary>
        /// <param name="obj">The object to compare to</param>
        /// <returns>0 if equal, 1 if not equal, -1 if the object is not a Token</returns>
        public virtual int CompareTo(object obj)
        {
            Token t = obj as Token;

            if (null != t)
            {
                if (IsTextRelevant)
                {
                    return (t.Text == Text && t.TokenType == TokenType) ? 0 : 1;
                }
                else
                {
                    return t.TokenType == TokenType ? 0 : 1;
                }
            }
            else
            {
                return -1;
            }
        }

    }
}
