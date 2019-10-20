
using Azurite.LexicalParser;
using Azurite.SyntaxAnalysis.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Azurite.SyntaxAnalysis.Grammar
{

    /// <summary>
    /// A syntax grammar which is a set of grammar rules (production rules).
    /// </summary>
    public class SyntaxGrammar
    {

        /// <summary>
        /// List of grammar (production) rules.
        /// </summary>
        public List<GrammarRule> ProductionRules { get; private set; }

        /// <summary>
        /// Construction of the grammar object.
        /// </summary>
        public SyntaxGrammar()
        {
            ProductionRules = new List<GrammarRule>();
        }

        /// <summary>
        /// Adds a simple grammar rule to the grammar.
        /// A simple rule only contains one letter nonterminals and terminals.
        /// Nonterminals are identified with upper letters and terminals with lower letters.
        /// </summary>
        /// <param name="leftside">Left side of the rule</param>
        /// <param name="rightside">Right side of the rule</param>
        public void AddSimpleRule(char leftside, string rightside)
        {
            if (!Char.IsLetter(leftside) || !rightside.All(x => Char.IsLetter(x)))
            {
                throw new Exception("Cannot create grammar rule with the given input.");
            }

            SyntaxTreeNonterminal left = new SyntaxTreeNonterminal(leftside.ToString());

            List<SyntaxTreeElement> right = new List<SyntaxTreeElement>();

            foreach (char c in rightside)
            {
                if (Char.IsUpper(c))
                {
                    right.Add(new SyntaxTreeNonterminal(c.ToString()));
                }
                else
                {
                    right.Add(new SyntaxTreeTerminal(new Token(c.ToString(), 0)));
                }
            }

            ProductionRules.Add(new GrammarRule(left, right));
        }

        /// <summary>
        /// Adds a rule to the grammar.
        /// </summary>
        /// <param name="rule">The rule to be added</param>
        public void AddRule(GrammarRule rule)
        {
            if (null == rule)
            {
                throw new Exception("Cannot add null rule to a grammar.");
            }

            ProductionRules.Add(rule);
        }

        /// <summary>
        /// Decides whether the grammar is valid or not.
        /// A grammar is valid if it is not empty and there is only one rule
        /// with the start symbol and all nonterminals have at least one production rule.
        /// </summary>
        /// <returns>True if valid, false otherwise</returns>
        public bool IsValid()
        {
            if (ProductionRules.Count == 0)
            {
                return false;
            }

            SyntaxTreeNonterminal startSymbol = ProductionRules[0].LeftSide;

            if (1 != ProductionRules.Count(x => x.LeftSide.CompareTo(startSymbol) == 0))
            {
                return false;
            }

            foreach (var rule in ProductionRules)
            {
                foreach (var rhs in rule.RightSide)
                {
                    if (rhs is SyntaxTreeNonterminal)
                    {
                        if (-1 == ProductionRules.FindIndex(x => x.LeftSide.CompareTo(rhs) == 0))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

    }
}
