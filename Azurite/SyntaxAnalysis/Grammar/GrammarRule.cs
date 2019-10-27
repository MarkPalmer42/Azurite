
using Azurite.SyntaxAnalysis.SyntaxTree;
using System;
using System.Collections.Generic;

namespace Azurite.SyntaxAnalysis.Grammar
{

    /// <summary>
    /// Represents a grammar rule that only has a nonterminal on the left side.
    /// </summary>
    public class GrammarRule : IEquatable<GrammarRule>
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
        /// <returns>True if equals, false otherwise</returns>
        public bool Equals(GrammarRule s)
        {
            if (!LeftSide.Equals(s.LeftSide) || RightSide.Count != s.RightSide.Count)
            {
                return false;
            }
            else
            {
                bool all = true;
                int i = 0;

                while (all && i < RightSide.Count)
                {
                    all = RightSide[i].Equals(s.RightSide[i]);
                    ++i;
                }

                return all ? true : false;
            }
        }
    }
}
