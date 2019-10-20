

namespace Azurite.SyntaxAnalysis.SyntaxParsingTable
{

    /// <summary>
    /// The ParsingTableElement class represents a cell in the parsing table.
    /// </summary>
    public class ParsingTableElement
    {

        /// <summary>
        /// The type of the cell.
        /// </summary>
        public ParsingTableElementType ElementType { get; set; }

        /// <summary>
        /// The value belonging to the cell.
        /// Can only be interpreted with the value of the ElementType.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// The constuctor of the ParsingTableElement.
        /// </summary>
        /// <param name="elemType">Type of the parsing table cell</param>
        public ParsingTableElement(ParsingTableElementType elemType)
        {
            ElementType = elemType;
            Value = 0;
        }

        /// <summary>
        /// The constuctor of the ParsingTableElement.
        /// </summary>
        /// <param name="elemType">Type of the parsing table cell</param>
        /// <param name="value">The value belonging to the cell</param>
        public ParsingTableElement(ParsingTableElementType elemType, int value)
        {
            ElementType = elemType;
            Value = value;
        }

    }
}
