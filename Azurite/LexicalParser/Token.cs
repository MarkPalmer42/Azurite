
namespace Azurite.LexicalParser
{
    /// <summary>
    /// The token type.
    /// </summary>
    public class Token
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
        /// Constructor of the token class.
        /// </summary>
        /// <param name="text">The parsed text</param>
        /// <param name="line">The line the token was parsed from</param>
        public Token(string text, int line)
        {
            Text = text;
            Line = line;
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

    }
}
