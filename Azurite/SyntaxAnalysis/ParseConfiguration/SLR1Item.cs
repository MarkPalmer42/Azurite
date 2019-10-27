
using Azurite.SyntaxAnalysis.Grammar;
using System;

namespace Azurite.SyntaxAnalysis.ParseConfiguration
{

    /// <summary>
    /// A SLR1Item is a derivation of a grammar rule which is used to
    /// construct SLR parsing tables.
    /// 
    /// An SLR1 item contains a grammar rule of the input SLR1 grammar
    /// with a predefined state that is the result of an extension of the
    /// grammar rule.
    /// 
    /// For example:
    /// Given the grammar rule E -> tT
    /// 
    /// This rule has three extensions depending on the current state (dot).
    /// 
    /// 1.) E -> .tT
    /// 2.) E -> t.T
    /// 3.) E -> tT.
    /// </summary>
    public class SLR1Item : IComparable
    {

        /// <summary>
        /// The grammar rule given.
        /// </summary>
        public GrammarRule Rule { get; set; }

        /// <summary>
        /// The state of the extension.
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// The target SLR1 Item Set that extends the current gramma rule.
        /// </summary>
        public int Target { get; set; }

        /// <summary>
        /// Constructor of the SLR1Item class.
        /// </summary>
        /// <param name="rule">The grammar rule</param>
        /// <param name="state">The state of extension</param>
        public SLR1Item(GrammarRule rule, int state)
        {
            Rule = rule;
            State = state;
            Target = -1;
        }

        /// <summary>
        /// Compares two SLR1ItemSets.
        /// </summary>
        /// <param name="obj">The other object.</param>
        /// <returns>0 if equal, 1 if not equal, -1 in case of incorrect obj type</returns>
        public int CompareTo(object obj)
        {
            SLR1Item s = obj as SLR1Item;

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
