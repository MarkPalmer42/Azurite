using Azurite.LexicalParser;
using Azurite.SyntaxAnalysis.SyntaxTree;
using Azurite.SyntaxAnalysis.SyntaxParsingTable;
using System.Collections.Generic;
using System;

namespace Azurite.SyntaxAnalysis
{
    public class SyntaxAnalysis
    {

        ParsingTable parsingTable = new ParsingTable();

        List<ParsingRule> parsingRules = new List<ParsingRule>();

        public SyntaxAnalysis()
        {
            for (int i = 0; i < 3; ++i)
            {
                parsingRules.Add(new ParsingRule());
            }

            parsingRules[0].LeftSide = new SyntaxTreeNonterminal("S'");
            parsingRules[0].RightSide.Add(new SyntaxTreeNonterminal("B"));
            parsingRules[0].RightSide.Add(new SyntaxTreeNonterminal("B"));

            parsingRules[1].LeftSide = new SyntaxTreeNonterminal("B");
            parsingRules[1].RightSide.Add(new SyntaxTreeTerminal(new Token("c", 0)));
            parsingRules[1].RightSide.Add(new SyntaxTreeNonterminal("B"));

            parsingRules[2].LeftSide = new SyntaxTreeNonterminal("B");
            parsingRules[2].RightSide.Add(new SyntaxTreeTerminal(new Token("d", 0)));

            BuildParseTable();
        }

        public SyntaxTreeElement AnalyzeSyntax(List<Token> tokens)
        {
            int currentRow = 0;
            int currentColumn = 0;
            int index = 0;
            bool accepted = false;

            Stack<ParsingStack> elemStack = new Stack<ParsingStack>();

            elemStack.Push(new ParsingStack(null, 0));

            tokens.Add(new Token("$", 0));

            while(!accepted && index < tokens.Count)
            {
                SyntaxTreeTerminal stt = new SyntaxTreeTerminal(tokens[index]);

                currentColumn = parsingTable.parsingTableHeader.FindIndex(x => x.CompareTo(stt) == 0);

                ParsingTableElement element = parsingTable.parsingTable[currentRow][currentColumn];

                if (element.ElementType == ParsingTableElementType.shift)
                {
                    elemStack.Push(new ParsingStack(stt, element.Value));
                    currentRow = element.Value;
                    ++index;
                }
                else if (element.ElementType == ParsingTableElementType.reduce)
                {
                    ParsingRule rule = parsingRules[element.Value];

                    SyntaxTreeNonterminal nt = new SyntaxTreeNonterminal(rule.LeftSide.Name);

                    for (int i = rule.RightSide.Count - 1; i >= 0; --i)
                    {
                        if (elemStack.Peek().Element.CompareTo(rule.RightSide[i]) == 0)
                        {
                            elemStack.Peek().Element.SetParent(nt);
                            nt.Children.Insert(0, elemStack.Peek().Element);
                            elemStack.Pop();
                        }
                        else
                        {
                            throw new System.Exception("Syntax error: cannot apply syntax rule " + element.Value.ToString());
                        }
                    }

                    currentRow = elemStack.Peek().Value;

                    int ruleLocation = parsingTable.parsingTableHeader.FindIndex(x => x.CompareTo(rule.LeftSide) == 0);

                    if (parsingTable.parsingTable[currentRow][ruleLocation].ElementType != ParsingTableElementType.jump)
                    {
                        throw new System.Exception("Syntax error: invalid parsing table.");
                    }

                    currentRow = parsingTable.parsingTable[currentRow][ruleLocation].Value;

                    elemStack.Push(new ParsingStack(nt, currentRow));
                }
                else if (element.ElementType == ParsingTableElementType.accept)
                {
                    if (elemStack.Count != 2 || elemStack.Peek().Element.CompareTo(parsingRules[1].LeftSide) != 0)
                    {
                        throw new System.Exception("Syntax error: invalid parsing table.");
                    }

                    accepted = true;
                }
                else
                {
                    throw new System.Exception("Syntax error: unexpected token " + tokens[index].Text + " on line " + tokens[index].Line.ToString());
                }
            }
            
            return elemStack.Peek().Element;
        }

        class ParseState : IComparable
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

        class RuleSet : IComparable
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

        void BuildParseTable()
        {
            if (parsingRules.Count < 1)
            {
                throw new System.Exception("Cannot build parse table without input rules.");
            }

            List<List<RuleSet>> parseStates = new List<List<RuleSet>>();

            ParsingRule rule = new ParsingRule();

            rule.LeftSide = new SyntaxTreeNonterminal("ZEROETHSTATE");
            rule.RightSide.Add(parsingRules[0].LeftSide);

            ParseState startState = new ParseState(rule, 0);

            RuleSet startSet = new RuleSet(startState);

            RecursiveTableBuild(ref parseStates, startSet);

            foreach (var i in parseStates)
            {
                foreach (var j in i)
                {
                    Console.Write("Target: " + j.TargetSet.ToString() + " ");
                    Console.Write("State: " + j.Rules.State.ToString() + " ");
                    Console.Write("\t" + j.Rules.Rule.LeftSide.Name);
                    Console.Write("\t->\t");

                    foreach (var k in j.Rules.Rule.RightSide)
                    {
                        SyntaxTreeTerminal t = k as SyntaxTreeTerminal;
                        SyntaxTreeNonterminal nt = k as SyntaxTreeNonterminal;

                        if (null != t)
                        {
                            Console.Write(t.SyntaxToken.Text + "\t");
                        }
                        else if (null != nt)
                        {
                            Console.Write(nt.Name + "\t");
                        }
                    }

                    Console.WriteLine("");
                }

                Console.WriteLine("");
            }
        }

        int RecursiveTableBuild(ref List<List<RuleSet>> ruleSets, RuleSet extendable = null)
        {
            if (null != extendable)
            {
                int idx = ruleSets.FindIndex(x => x[0].CompareTo(extendable) == 0);

                if (-1 != idx)
                {
                    return idx;
                }
                else
                {
                    ruleSets.Add(new List<RuleSet>());
                    ruleSets[ruleSets.Count - 1].Add(extendable);
                }
            }

            int currentRuleSet = ruleSets.Count - 1;

            int currentRuleIndex = 0;

            while (currentRuleIndex < ruleSets[currentRuleSet].Count)
            {
                var currentRule = ruleSets[currentRuleSet][currentRuleIndex];

                SyntaxTreeElement nextSymbol = null;

                if (currentRule.Rules.Rule.RightSide.Count > currentRule.Rules.State)
                {
                    nextSymbol = currentRule.Rules.Rule.RightSide[currentRule.Rules.State];
                }

                if (null != nextSymbol)
                {
                    if (nextSymbol is SyntaxTreeNonterminal)
                    {
                        foreach (var rule in parsingRules)
                        {
                            if (0 == rule.LeftSide.CompareTo(nextSymbol))
                            {
                                RuleSet extendedRuleSet = new RuleSet(new ParseState(rule, 0));

                                int idx = ruleSets[currentRuleSet].FindIndex(x => x.CompareTo(extendedRuleSet) == 0);

                                if (-1 == idx)
                                {
                                    ruleSets[currentRuleSet].Add(extendedRuleSet);
                                }
                            }
                        }
                    }
                }

                ++currentRuleIndex;
            }

            foreach (var s in ruleSets[currentRuleSet])
            {
                SyntaxTreeElement nextSymbol = null;

                if (s.Rules.Rule.RightSide.Count > s.Rules.State)
                {
                    nextSymbol = s.Rules.Rule.RightSide[s.Rules.State];
                }

                if (null != nextSymbol)
                {
                    RuleSet nextRuleSet = new RuleSet(new ParseState(s.Rules.Rule, s.Rules.State + 1));
                    s.TargetSet = RecursiveTableBuild(ref ruleSets, nextRuleSet);
                }
            }

            return currentRuleSet;
        }

    }
}
