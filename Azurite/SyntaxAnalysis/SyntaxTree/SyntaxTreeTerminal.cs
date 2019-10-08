
using Azurite.LexicalParser;

namespace Azurite.SyntaxAnalysis.SyntaxTree
{

    public class SyntaxTreeTerminal : SyntaxTreeElement
    {

        public Token SyntaxToken { get; private set; }

        public SyntaxTreeTerminal(Token token)
        {
            SyntaxToken = token;
        }

        public override int CompareTo(object obj)
        {
            SyntaxTreeTerminal nt = obj as SyntaxTreeTerminal;

            if (null == nt)
            {
                return -1;
            }
            else
            {
                Token t = nt.SyntaxToken;
                return (t.Text == SyntaxToken.Text && t.TokenType == SyntaxToken.TokenType) ? 0 : 1;
            }
        }

    }

}
