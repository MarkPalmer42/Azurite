
using System;

namespace Azurite.LexicalParser
{
    /// <summary>
    /// The token type.
    /// </summary>
    public class Token : IEquatable<Token>
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
        /// <param name="e">The object to compare to</param>
        /// <returns>True if the two tokens are equal, false otherwise</returns>
        public virtual bool Equals(Token t)
        {
            if (null == t)
            {
                return false;
            }

            if (IsTextRelevant)
            {
                return t.Text == Text && t.TokenType == TokenType;
            }
            else
            {
                return t.TokenType == TokenType;
            }
        }

        /// <summary>
        /// Compares this token instance to an object.
        /// </summary>
        /// <param name="obj">The object to compare to</param>
        /// <returns>True if the two tokens are equal, false otherwise</returns>
        public override bool Equals(object obj) => Equals(obj as Token);

        /// <summary>
        /// Calculates the has code for the object.
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode()
        {
            if (IsTextRelevant)
            {
                return (Text, TokenType).GetHashCode();
            }
            else
            {
                return TokenType.GetHashCode();
            }
        }

    }
}
