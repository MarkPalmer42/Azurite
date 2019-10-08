using Azurite.SyntaxAnalysis.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azurite.SyntaxAnalysis.SyntaxParsingTable
{
    public class ParsingRule
    {

        public SyntaxTreeNonterminal LeftSide { get; set; }

        public List<SyntaxTreeElement> RightSide { get; set; }

        public ParsingRule()
        {
            RightSide = new List<SyntaxTreeElement>();
        }

    }
}
