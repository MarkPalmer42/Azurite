
namespace Azurite.SyntaxAnalysis.SyntaxParsingTable
{

    /// <summary>
    /// This enum represents the 5 possible states in every cell of the
    /// SLR1 parsing table.
    /// </summary>
    public enum ParsingTableElementType
    {
        error,
        shift,
        reduce,
        accept,
        jump
    }

}
