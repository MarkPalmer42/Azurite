using Azurite.LexicalParser;
using Azurite.SyntaxAnalysis.SyntaxTree;
using Azurite.SyntaxAnalysis.SyntaxParsingTable;
using System.Collections.Generic;

namespace Azurite.SyntaxAnalysis
{
    public class SyntaxAnalysis
    {

        ParsingTable parsingTable = new ParsingTable();

        List<ParsingRule> parsingRules = new List<ParsingRule>();

        public SyntaxAnalysis()
        {
            parsingTable.AddHeader(new SyntaxTreeTerminal(new Token("$", 0)));

            parsingTable.AddHeader(new SyntaxTreeTerminal(new Token("(", 0)));
            parsingTable.AddHeader(new SyntaxTreeTerminal(new Token(")", 0)));
            parsingTable.AddHeader(new SyntaxTreeTerminal(new Token("*", 0)));
            parsingTable.AddHeader(new SyntaxTreeTerminal(new Token("+", 0)));
            parsingTable.AddHeader(new SyntaxTreeTerminal(new Token("a", 0)));
            parsingTable.AddHeader(new SyntaxTreeNonterminal("E"));
            parsingTable.AddHeader(new SyntaxTreeNonterminal("F"));
            parsingTable.AddHeader(new SyntaxTreeNonterminal("S"));
            parsingTable.AddHeader(new SyntaxTreeNonterminal("T"));

            for (int i = 0; i <= 12; ++i)
            {
                parsingTable.AddRow();
            }

            parsingTable.parsingTable[2][0] = new ParsingTableElement(ParsingTableElementType.reduce, 1);
            parsingTable.parsingTable[3][0] = new ParsingTableElement(ParsingTableElementType.reduce, 5);
            parsingTable.parsingTable[4][0] = new ParsingTableElement(ParsingTableElementType.accept);
            parsingTable.parsingTable[5][0] = new ParsingTableElement(ParsingTableElementType.reduce, 3);
            parsingTable.parsingTable[6][0] = new ParsingTableElement(ParsingTableElementType.reduce, 7);
            parsingTable.parsingTable[10][0] = new ParsingTableElement(ParsingTableElementType.reduce, 6);
            parsingTable.parsingTable[11][0] = new ParsingTableElement(ParsingTableElementType.reduce, 2);
            parsingTable.parsingTable[12][0] = new ParsingTableElement(ParsingTableElementType.reduce, 4);

            parsingTable.parsingTable[0][1] = new ParsingTableElement(ParsingTableElementType.shift, 1);
            parsingTable.parsingTable[1][1] = new ParsingTableElement(ParsingTableElementType.shift, 1);
            parsingTable.parsingTable[8][1] = new ParsingTableElement(ParsingTableElementType.shift, 1);
            parsingTable.parsingTable[9][1] = new ParsingTableElement(ParsingTableElementType.shift, 1);

            parsingTable.parsingTable[3][2] = new ParsingTableElement(ParsingTableElementType.reduce, 5);
            parsingTable.parsingTable[5][2] = new ParsingTableElement(ParsingTableElementType.reduce, 3);
            parsingTable.parsingTable[6][2] = new ParsingTableElement(ParsingTableElementType.reduce, 7);
            parsingTable.parsingTable[7][2] = new ParsingTableElement(ParsingTableElementType.shift, 10);
            parsingTable.parsingTable[10][2] = new ParsingTableElement(ParsingTableElementType.reduce, 6);
            parsingTable.parsingTable[11][2] = new ParsingTableElement(ParsingTableElementType.reduce, 2);
            parsingTable.parsingTable[12][2] = new ParsingTableElement(ParsingTableElementType.reduce, 4);

            parsingTable.parsingTable[3][3] = new ParsingTableElement(ParsingTableElementType.reduce, 5);
            parsingTable.parsingTable[5][3] = new ParsingTableElement(ParsingTableElementType.shift, 9);
            parsingTable.parsingTable[6][3] = new ParsingTableElement(ParsingTableElementType.reduce, 7);
            parsingTable.parsingTable[10][3] = new ParsingTableElement(ParsingTableElementType.reduce, 6);
            parsingTable.parsingTable[11][3] = new ParsingTableElement(ParsingTableElementType.shift, 9);
            parsingTable.parsingTable[12][3] = new ParsingTableElement(ParsingTableElementType.reduce, 4);

            parsingTable.parsingTable[2][4] = new ParsingTableElement(ParsingTableElementType.shift, 8);
            parsingTable.parsingTable[3][4] = new ParsingTableElement(ParsingTableElementType.reduce, 5);
            parsingTable.parsingTable[5][4] = new ParsingTableElement(ParsingTableElementType.reduce, 3);
            parsingTable.parsingTable[6][4] = new ParsingTableElement(ParsingTableElementType.reduce, 7);
            parsingTable.parsingTable[7][4] = new ParsingTableElement(ParsingTableElementType.shift, 8);
            parsingTable.parsingTable[10][4] = new ParsingTableElement(ParsingTableElementType.reduce, 6);
            parsingTable.parsingTable[11][4] = new ParsingTableElement(ParsingTableElementType.reduce, 2);
            parsingTable.parsingTable[12][4] = new ParsingTableElement(ParsingTableElementType.reduce, 4);

            parsingTable.parsingTable[0][5] = new ParsingTableElement(ParsingTableElementType.shift, 6);
            parsingTable.parsingTable[1][5] = new ParsingTableElement(ParsingTableElementType.shift, 6);
            parsingTable.parsingTable[8][5] = new ParsingTableElement(ParsingTableElementType.shift, 6);
            parsingTable.parsingTable[9][5] = new ParsingTableElement(ParsingTableElementType.shift, 6);

            parsingTable.parsingTable[0][6] = new ParsingTableElement(ParsingTableElementType.jump, 2);
            parsingTable.parsingTable[1][6] = new ParsingTableElement(ParsingTableElementType.jump, 7);

            parsingTable.parsingTable[0][7] = new ParsingTableElement(ParsingTableElementType.jump, 3);
            parsingTable.parsingTable[1][7] = new ParsingTableElement(ParsingTableElementType.jump, 3);
            parsingTable.parsingTable[8][7] = new ParsingTableElement(ParsingTableElementType.jump, 3);
            parsingTable.parsingTable[9][7] = new ParsingTableElement(ParsingTableElementType.jump, 12);

            parsingTable.parsingTable[0][8] = new ParsingTableElement(ParsingTableElementType.jump, 4);

            parsingTable.parsingTable[0][9] = new ParsingTableElement(ParsingTableElementType.jump, 5);
            parsingTable.parsingTable[1][9] = new ParsingTableElement(ParsingTableElementType.jump, 5);
            parsingTable.parsingTable[8][9] = new ParsingTableElement(ParsingTableElementType.jump, 11);

            for (int i = 0; i < 8; ++i)
            {
                parsingRules.Add(new ParsingRule());
            }

            parsingRules[0].LeftSide = new SyntaxTreeNonterminal("S'");
            parsingRules[0].RightSide.Add(new SyntaxTreeNonterminal("S"));

            parsingRules[1].LeftSide = new SyntaxTreeNonterminal("S");
            parsingRules[1].RightSide.Add(new SyntaxTreeNonterminal("E"));

            parsingRules[2].LeftSide = new SyntaxTreeNonterminal("E");
            parsingRules[2].RightSide.Add(new SyntaxTreeNonterminal("E"));
            parsingRules[2].RightSide.Add(new SyntaxTreeTerminal(new Token("+", 0)));
            parsingRules[2].RightSide.Add(new SyntaxTreeNonterminal("T"));

            parsingRules[3].LeftSide = new SyntaxTreeNonterminal("E");
            parsingRules[3].RightSide.Add(new SyntaxTreeNonterminal("T"));

            parsingRules[4].LeftSide = new SyntaxTreeNonterminal("T");
            parsingRules[4].RightSide.Add(new SyntaxTreeNonterminal("T"));
            parsingRules[4].RightSide.Add(new SyntaxTreeTerminal(new Token("*", 0)));
            parsingRules[4].RightSide.Add(new SyntaxTreeNonterminal("F"));

            parsingRules[5].LeftSide = new SyntaxTreeNonterminal("T");
            parsingRules[5].RightSide.Add(new SyntaxTreeNonterminal("F"));

            parsingRules[6].LeftSide = new SyntaxTreeNonterminal("F");
            parsingRules[6].RightSide.Add(new SyntaxTreeTerminal(new Token("(", 0)));
            parsingRules[6].RightSide.Add(new SyntaxTreeNonterminal("E"));
            parsingRules[6].RightSide.Add(new SyntaxTreeTerminal(new Token(")", 0)));

            parsingRules[7].LeftSide = new SyntaxTreeNonterminal("F");
            parsingRules[7].RightSide.Add(new SyntaxTreeTerminal(new Token("a", 0)));
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
