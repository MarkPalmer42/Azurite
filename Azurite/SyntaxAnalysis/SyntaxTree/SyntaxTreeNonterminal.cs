
using System.Collections.Generic;
using System.Linq;

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
        /// Compares two terminal elements.
        /// </summary>
        /// <param name="e">The object to compare to</param>
        /// <returns>True if equal, false otherwise</returns>
        public override bool Equals(SyntaxTreeElement e)
        {
            SyntaxTreeNonterminal nt = e as SyntaxTreeNonterminal;

            if (null == nt)
            {
                return false;
            }
            else
            {
                return Name == nt.Name && Children.SequenceEqual(nt.Children);
            }
        }

        /// <summary>
        /// Compares two terminal elements.
        /// </summary>
        /// <param name="obj">The object to compare to</param>
        /// <returns>True if equal, false otherwise</returns>
        public override bool Equals(object obj) => Equals(obj as SyntaxTreeNonterminal);

        /// <summary>
        /// Calculates the has code for the object.
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode() => (Name, Children).GetHashCode();

    }

}
