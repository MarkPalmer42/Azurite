
using System;
using System.Collections.Generic;

namespace Azurite.SyntaxAnalysis.SyntaxTree
{

    public class SyntaxTreeNonterminal : SyntaxTreeElement
    {

        public string Name { get; private set; }

        public List<SyntaxTreeElement> Children { get; set; }

        public SyntaxTreeNonterminal(string name)
        {
            Children = new List<SyntaxTreeElement>();
            Name = name;
        }

        void AddChild(SyntaxTreeElement ste)
        {
            Children.Add(ste);
        }

        public override int CompareTo(object obj)
        {
            SyntaxTreeNonterminal nt = obj as SyntaxTreeNonterminal;

            if (null == nt)
            {
                return -1;
            }
            else
            {
                return (Name == nt.Name) ? 0 : 1;
            }
        }
    }

}
