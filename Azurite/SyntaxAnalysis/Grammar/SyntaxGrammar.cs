
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
        /// List of nonterminals in the grammar.
        /// </summary>
        public List<SyntaxTreeNonterminal> Nonterminals { get; private set; }

        /// <summary>
        /// List of terminals in the grammar.
        /// </summary>
        public List<SyntaxTreeTerminal> Terminals { get; private set; }

        /// <summary>
        /// True if the zeroth state exists, false otherwise.
        /// </summary>
        private bool zerothStateAdded = false;

        /// <summary>
        /// Construction of the grammar object.
        /// </summary>
        public SyntaxGrammar()
        {
            ProductionRules = new List<GrammarRule>();
            Nonterminals = new List<SyntaxTreeNonterminal>();
            Terminals = new List<SyntaxTreeTerminal>();
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
            if (!Char.IsLetter(leftside) || !Char.IsUpper(leftside))
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

            AddRule(new GrammarRule(left, right));
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

            AddToNonTerminals(rule.LeftSide);
            AddRightSideToLists(rule.RightSide);
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

        /// <summary>
        /// Adds a Nonterminal to the list if it is not added already.
        /// </summary>
        /// <param name="nt">The nonterminal to be added</param>
        private void AddToNonTerminals(SyntaxTreeNonterminal nt)
        {
            if (-1 == Nonterminals.FindIndex(x => x.CompareTo(nt) == 0))
            {
                Nonterminals.Add(nt);
            }
        }

        /// <summary>
        /// Adds a list of terminals and nonterminals to the internal lists
        /// if they are not added already.
        /// </summary>
        /// <param name="rhs">List of syntax tree elements.</param>
        private void AddRightSideToLists(List<SyntaxTreeElement> rhs)
        {
            foreach (var r in rhs)
            {
                SyntaxTreeNonterminal nt = r as SyntaxTreeNonterminal;
                SyntaxTreeTerminal t = r as SyntaxTreeTerminal;

                if (null != nt)
                {
                    AddToNonTerminals(nt);
                }
                else if (null != t)
                {
                    if (-1 == Terminals.FindIndex(x => x.CompareTo(t) == 0))
                    {
                        Terminals.Add(t);
                    }
                }
                else
                {
                    throw new Exception("Cannot add invalid element type to list.");
                }
            }
        }

        /// <summary>
        /// Adds a zeroth state to the grammar production rules.
        /// </summary>
        public void AddZerothState()
        {
            if (!zerothStateAdded)
            {
                int i = 0;
                SyntaxTreeNonterminal nt = null;

                do
                {
                    nt = new SyntaxTreeNonterminal("ZERO" + i.ToString());
                }
                while (-1 != ProductionRules.FindIndex(x => x.LeftSide.CompareTo(nt) == 0));

                List<SyntaxTreeElement> elements = new List<SyntaxTreeElement>();

                elements.Add(ProductionRules[0].LeftSide);

                ProductionRules.Insert(0, new GrammarRule(nt, elements));

                zerothStateAdded = true;
            }
        }

    }
}
