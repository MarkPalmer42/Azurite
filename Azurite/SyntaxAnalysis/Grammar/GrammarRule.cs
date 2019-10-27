
using Azurite.SyntaxAnalysis.SyntaxTree;
using System;
using System.Collections.Generic;

namespace Azurite.SyntaxAnalysis.Grammar
{

    /// <summary>
    /// Represents a grammar rule that only has a nonterminal on the left side.
    /// </summary>
    public class GrammarRule : IComparable
    {

        /// <summary>
        /// The left side of the grammar rule.
        /// </summary>
        public SyntaxTreeNonterminal LeftSide { get; private set; }

        /// <summary>
        /// The right side of the grammar rule. A list of terminals and nonterminals.
        /// </summary>
        public List<SyntaxTreeElement> RightSide { get; private set; }

        /// <summary>
        /// Constuctor of the grammar rule.
        /// </summary>
        public GrammarRule()
        {
            RightSide = new List<SyntaxTreeElement>();
        }

        /// <summary>
        /// Constuctor of the grammar rule.
        /// </summary>
        /// <param name="leftside">The left side of the rule</param>
        /// <param name="rightside">The right side of the rule</param>
        public GrammarRule(SyntaxTreeNonterminal leftside, List<SyntaxTreeElement> rightside)
        {
            LeftSide = leftside;
            RightSide = rightside;
        }

        /// <summary>
        /// Compares two grammar rules.
        /// </summary>
        /// <param name="obj">The obj to compare to</param>
        /// <returns>0 if equal, 1 if not equal, -1 in case of incorrect obj type</returns>
        public int CompareTo(object obj)
        {
            GrammarRule s = obj as GrammarRule;

            if (null != s)
            {
                if (LeftSide.CompareTo(s.LeftSide) != 0 || RightSide.Count != s.RightSide.Count)
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
