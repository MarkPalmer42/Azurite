
using System;

namespace Azurite.SyntaxAnalysis.SyntaxTree
{

    /// <summary>
    /// Represents a terminal or a nonterminal symbol.
    /// Also represents an element in the syntax tree.
    /// </summary>
    public abstract class SyntaxTreeElement : IEquatable<SyntaxTreeElement>
    {

        /// <summary>
        /// Parent element in the syntax tree if it exists.
        /// </summary>
        public SyntaxTreeElement Parent { get; private set; }

        /// <summary>
        /// Constructor of the SyntaxTreeElement.
        /// </summary>
        public SyntaxTreeElement()
        {
            Parent = null;
        }

        /// <summary>
        /// Sets the parent of the SyntaxTreeElement.
        /// </summary>
        /// <param name="parent">The parent SyntaxTreeElement</param>
        public void SetParent(SyntaxTreeElement parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Compares two SyntaxTreeElement objects.
        /// </summary>
        /// <param name="obj">The object to compare to</param>
        /// <returns>True if equal, false otherwise</returns>
        public abstract bool Equals(SyntaxTreeElement other);

    }
}
