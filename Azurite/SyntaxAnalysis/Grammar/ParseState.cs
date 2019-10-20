
using Azurite.SyntaxAnalysis.SyntaxParsingTable;
using System;

namespace Azurite.SyntaxAnalysis.ParseTable
{

    public class ParseState : IComparable
    {

        public ParsingRule Rule { get; set; }

        public int State { get; set; }

        public ParseState(ParsingRule rule, int state)
        {
            Rule = rule;
            State = state;
        }

        public int CompareTo(object obj)
        {
            ParseState s = obj as ParseState;

            if (null != s)
            {
                if (Rule.CompareTo(s.Rule) == 0 && State == s.State)
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
