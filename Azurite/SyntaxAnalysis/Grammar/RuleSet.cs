using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azurite.SyntaxAnalysis.ParseTable
{
    public class RuleSet : IComparable
    {
        public ParseState Rules { get; set; }

        public int TargetSet { get; set; }

        public RuleSet(ParseState parseState)
        {
            Rules = parseState;
            TargetSet = -1;
        }

        public int CompareTo(object obj)
        {
            RuleSet s = obj as RuleSet;

            if (null != s)
            {
                if (Rules.CompareTo(s.Rules) == 0)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return -1;
            }
        }

    };
}
