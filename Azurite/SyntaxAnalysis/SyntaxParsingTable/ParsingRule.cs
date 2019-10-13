using Azurite.SyntaxAnalysis.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azurite.SyntaxAnalysis.SyntaxParsingTable
{
    public class ParsingRule : IComparable
    {

        public SyntaxTreeNonterminal LeftSide { get; set; }

        public List<SyntaxTreeElement> RightSide { get; set; }

        public ParsingRule()
        {
            RightSide = new List<SyntaxTreeElement>();
        }

        public int CompareTo(object obj)
        {
            ParsingRule s = obj as ParsingRule;

            if (null != s)
            {
                if (LeftSide.CompareTo(s.LeftSide) != 0 && RightSide.Count != s.RightSide.Count)
                {
                    return 1;
                }
                else
                {
                    bool found = false;
                    int i = 0;

                    while (!found && i < RightSide.Count)
                    {
                        found = RightSide[i].CompareTo(s.RightSide[i]) != 0;
                        ++i;
                    }

                    return found ? 1 : 0;
                }
            }
            else
            {
                return -1;
            }
        }

    }
}
