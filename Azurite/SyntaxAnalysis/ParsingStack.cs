using Azurite.SyntaxAnalysis.SyntaxTree;

namespace Azurite.SyntaxAnalysis
{
    public class ParsingStack
    {

        public SyntaxTreeElement Element { get; private set; }

        public int Value { get; private set; }

        public ParsingStack(SyntaxTreeElement ste, int val)
        {
            Element = ste;
            Value = val;
        }

    }
}
