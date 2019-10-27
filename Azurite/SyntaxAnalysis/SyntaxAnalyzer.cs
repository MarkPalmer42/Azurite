
using Azurite.LexicalParser;
using Azurite.SyntaxAnalysis.SyntaxTree;
using Azurite.SyntaxAnalysis.SyntaxParsingTable;
using System.Collections.Generic;
using Azurite.SyntaxAnalysis.Grammar;
using Azurite.SyntaxAnalysis.ParseSets;
using Azurite.SyntaxAnalysis.ParseConfiguration;

namespace Azurite.SyntaxAnalysis
{

    /// <summary>
    /// The syntax analysis class that analyses the input list of tokens
    /// and constructs a syntax tree based on the input.
    /// </summary>
    public class SyntaxAnalyzer
    {

        /// <summary>
        /// The table used for parsing.
        /// </summary>
        ParsingTable parsingTable = null;

        /// <summary>
        /// The rules of the grammar.
        /// </summary>
        SyntaxGrammar parsingRules = new SyntaxGrammar();

        /// <summary>
        /// Generates and stores the first and follow sets.
        /// </summary>
        ParsingSets sets = null;

        /// <summary>
        /// Contains the SLR1 configuration sets.
        /// </summary>
        SLR1Configuration slr1Config = null;

        /// <summary>
        /// Constructs the SyntaxAnalysis class based on the given XML file.
        /// </summary>
        public SyntaxAnalyzer(string xmlPath, string xsdPath, List<string> parsingElements)
        {
            XMLSyntaxParserReader reader = new XMLSyntaxParserReader();

            SyntaxGrammar grammar = reader.ReadGrammar(xmlPath, xsdPath, parsingElements);

            grammar.AddZerothState();

            parsingRules = grammar;

            sets = new ParsingSets(grammar);

            slr1Config = new SLR1Configuration(grammar);

            parsingTable = new ParsingTable(grammar, slr1Config, sets.FollowSet);
        }

        /// <summary>
        /// Constructs the SyntaxAnalysis class.
        /// </summary>
        public SyntaxAnalyzer(SyntaxGrammar grammar)
        {
            grammar.AddZerothState();

            parsingRules = grammar;

            sets = new ParsingSets(grammar);

            slr1Config = new SLR1Configuration(grammar);

            parsingTable = new ParsingTable(grammar, slr1Config, sets.FollowSet);
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

            tokens.Add(new ExtremalToken());

            while(!accepted && index < tokens.Count)
            {
                SyntaxTreeTerminal stt = new SyntaxTreeTerminal(tokens[index]);

                currentColumn = parsingTable.Header.FindIndex(x => x.CompareTo(stt) == 0);

                ParsingTableElement element = parsingTable.Table[currentRow][currentColumn];

                if (element.ElementType == ParsingTableElementType.shift)
                {
                    elemStack.Push(new ParsingStackElement(stt, element.Value));
                    currentRow = element.Value;
                    ++index;
                }
                else if (element.ElementType == ParsingTableElementType.reduce)
                {
                    GrammarRule rule = parsingRules.ProductionRules[element.Value];

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

                    int ruleLocation = parsingTable.Header.FindIndex(x => x.CompareTo(rule.LeftSide) == 0);

                    if (parsingTable.Table[currentRow][ruleLocation].ElementType != ParsingTableElementType.jump)
                    {
                        throw new System.Exception("Syntax error: invalid parsing table.");
                    }

                    currentRow = parsingTable.Table[currentRow][ruleLocation].Value;

                    elemStack.Push(new ParsingStackElement(nt, currentRow));
                }
                else if (element.ElementType == ParsingTableElementType.accept)
                {
                    if (elemStack.Count != 2 || elemStack.Peek().Element.CompareTo(parsingRules.ProductionRules[1].LeftSide) != 0)
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
