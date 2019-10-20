
using Azurite.LexicalParser;
using Azurite.SyntaxAnalysis.SyntaxTree;
using Azurite.SyntaxAnalysis.SyntaxParsingTable;
using System.Collections.Generic;
using System;
using Azurite.SyntaxAnalysis.Grammar;

namespace Azurite.SyntaxAnalysis
{

    /// <summary>
    /// The syntax analysis class that analyses the input list of tokens
    /// and constructs a syntax tree based on the input.
    /// </summary>
    public class SyntaxAnalysis
    {

        /// <summary>
        /// The table used for parsing.
        /// </summary>
        ParsingTable parsingTable = new ParsingTable();

        /// <summary>
        /// The rules of the grammar.
        /// </summary>
        List<GrammarRule> parsingRules = new List<GrammarRule>();

        /// <summary>
        /// The follow set that contains all subsequent terminal symbol of
        /// a nontermianl symbol. Calculated for all nonterminals.
        /// </summary>
        List<TerminalList> follows = new List<TerminalList>();

        /// <summary>
        /// The first set that contains all the terminals that can be the
        /// beginning of a nonterminal symbol. Calculated for all nonterminals.
        /// </summary>
        List<TerminalList> firsts = new List<TerminalList>();

        /// <summary>
        /// Constructs the SyntaxAnalysis class.
        /// </summary>
        public SyntaxAnalysis()
        {
            /*for (int i = 0; i < 3; ++i)
            {
                parsingRules.Add(new GrammarRule());
            }

            parsingRules[0].LeftSide = new SyntaxTreeNonterminal("S'");
            parsingRules[0].RightSide.Add(new SyntaxTreeNonterminal("B"));
            parsingRules[0].RightSide.Add(new SyntaxTreeNonterminal("B"));

            parsingRules[1].LeftSide = new SyntaxTreeNonterminal("B");
            parsingRules[1].RightSide.Add(new SyntaxTreeTerminal(new Token("c", 0)));
            parsingRules[1].RightSide.Add(new SyntaxTreeNonterminal("B"));

            parsingRules[2].LeftSide = new SyntaxTreeNonterminal("B");
            parsingRules[2].RightSide.Add(new SyntaxTreeTerminal(new Token("d", 0)));

            parsingTable = SLR1ParseTableFactory.BuildParseTable(parsingRules);*/
        }

        /// <summary>
        /// Analyses the syntax of the given list of tokens and constructs a Syntax Tree.
        /// </summary>
        /// <param name="tokens">The list of tokens</param>
        /// <returns>The syntax tree generated</returns>
        public SyntaxTreeElement AnalyzeSyntax(List<Token> tokens)
        {
            int currentRow = 0;
            int currentColumn = 0;
            int index = 0;
            bool accepted = false;

            Stack<ParsingStackElement> elemStack = new Stack<ParsingStackElement>();

            elemStack.Push(new ParsingStackElement(null, 0));

            tokens.Add(new Token("$", 0));

            while(!accepted && index < tokens.Count)
            {
                SyntaxTreeTerminal stt = new SyntaxTreeTerminal(tokens[index]);

                currentColumn = parsingTable.parsingTableHeader.FindIndex(x => x.CompareTo(stt) == 0);

                ParsingTableElement element = parsingTable.parsingTable[currentRow][currentColumn];

                if (element.ElementType == ParsingTableElementType.shift)
                {
                    elemStack.Push(new ParsingStackElement(stt, element.Value));
                    currentRow = element.Value;
                    ++index;
                }
                else if (element.ElementType == ParsingTableElementType.reduce)
                {
                    GrammarRule rule = parsingRules[element.Value];

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

                    elemStack.Push(new ParsingStackElement(nt, currentRow));
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
