using Azurite.LexicalParser;
using Azurite.SyntaxAnalysis.SyntaxTree;
using Azurite.SyntaxAnalysis.SyntaxParsingTable;
using System.Collections.Generic;
using System;
using Azurite.SyntaxAnalysis.Grammar;

namespace Azurite.SyntaxAnalysis
{
    public class SyntaxAnalysis
    {

        ParsingTable parsingTable = new ParsingTable();

        List<ParsingRule> parsingRules = new List<ParsingRule>();

        List<TerminalList> follows = new List<TerminalList>();

        List<TerminalList> firsts = new List<TerminalList>();

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

            parsingTable = ParseTableFactory.BuildParseTable(parsingRules);
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
        
    }
}
