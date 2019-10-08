
using System;

namespace Azurite.SyntaxAnalysis.SyntaxTree
{
    public abstract class SyntaxTreeElement : IComparable
    {

        public SyntaxTreeElement Parent { get; private set; }

        public SyntaxTreeElement()
        {
            Parent = null;
        }

        public void SetParent(SyntaxTreeElement parent)
        {
            Parent = parent;
        }

        public abstract int CompareTo(object obj);

    }
}
