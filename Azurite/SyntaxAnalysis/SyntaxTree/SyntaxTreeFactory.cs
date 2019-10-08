
using Azurite.LexicalParser;

namespace Azurite.SyntaxAnalysis.SyntaxTree
{
    public static class SyntaxTreeFactory
    {

        public static SyntaxTreeElement CreateNonterminal(string name)
        {
            return new SyntaxTreeNonterminal(name);
        }

        public static SyntaxTreeElement CreateTerminal(Token token)
        {
            return new SyntaxTreeTerminal(token);
        }

    }
}
