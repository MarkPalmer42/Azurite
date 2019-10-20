using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azurite.SyntaxAnalysis.SyntaxParsingTable
{
    public class ParsingTableElement
    {

        public ParsingTableElementType ElementType { get; private set; }

        public int Value { get; set; }

        public ParsingTableElement(ParsingTableElementType elemType)
        {
            ElementType = elemType;
            Value = 0;
        }

        public ParsingTableElement(ParsingTableElementType elemType, int value)
        {
            ElementType = elemType;
            Value = value;
        }

    }
}
