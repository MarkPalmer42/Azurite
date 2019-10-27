
using System.Collections.Generic;

namespace Azurite.SyntaxAnalysis.SyntaxTree
{

    /// <summary>
    /// Represents a nonterminal symbol in the syntax tree.
    /// </summary>
    public class SyntaxTreeNonterminal : SyntaxTreeElement
    {

        /// <summary>
        /// The name of the nonterminal symbol.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The children of the nonterminal symbol in the syntax tree.
        /// </summary>
        public List<SyntaxTreeElement> Children { get; set; }

        /// <summary>
        /// The constructor of the nonterminal syntax tree element.
        /// </summary>
        /// <param name="name">The name of the nonterminal element</param>
        public SyntaxTreeNonterminal(string name)
        {
            Children = new List<SyntaxTreeElement>();
            Name = name;
        }

        /// <summary>
        /// Adds a child element to the nonterminal.
        /// </summary>
        /// <param name="ste">The child element.</param>
        void AddChild(SyntaxTreeElement ste)
        {
            Children.Add(ste);
        }

        /// <summary>
        /// Compares two nonterminal elements.
        /// </summary>
        /// <param name="obj">The object to compare to</param>
        /// <returns>0 if equal, 1 if not equal, -1 in case of incorrect object type</returns>
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
