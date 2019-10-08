
using Azurite.LexicalParser;
using Azurite.SyntaxAnalysis.SyntaxTree;
using System;
using System.Collections.Generic;
using System.IO;

namespace Azurite
{
    class Program
    {

        static void WriteSyntaxTreeElement(SyntaxTreeElement element, string padding)
        {
            SyntaxTreeNonterminal nt = element as SyntaxTreeNonterminal;
            SyntaxTreeTerminal t = element as SyntaxTreeTerminal;

            if (null != nt)
            {
                Console.WriteLine(padding + "NT " + nt.Name);

                padding += "\t";

                foreach (var c in nt.Children)
                {
                    WriteSyntaxTreeElement(c, padding);
                }
            }
            else if (null != t)
            {
                Console.WriteLine(padding + "T " + t.SyntaxToken.TokenTypeName + " " + t.SyntaxToken.Text);
            }
            else
            {
                throw new Exception("Invalid element in syntax tree.");
            }
        }

        static void Main(string[] args)
        {
            try
            {
                /*Parser parser = new Parser("../../lexical_parser_rules.xml", "../../lexical_parser.xsd");

                string exampleProgram = File.ReadAllText("../../example_program.txt");

                var tokens = parser.Parse(exampleProgram);

                foreach (var t in tokens)
                {
                    Console.WriteLine("Name: " + t.Text + ", type: " + t.TokenType);
                }*/

                SyntaxAnalysis.SyntaxAnalysis syntaxAnalysis = new SyntaxAnalysis.SyntaxAnalysis();

                List<Token> list = new List<Token>();

                list.Add(new Token("a", 0));
                list.Add(new Token("*", 0));
                list.Add(new Token("(", 0));
                list.Add(new Token("a", 0));
                list.Add(new Token("+", 0));
                list.Add(new Token("a", 0));
                list.Add(new Token("*", 0));
                list.Add(new Token("a", 0));
                list.Add(new Token(")", 0));

                WriteSyntaxTreeElement(syntaxAnalysis.AnalyzeSyntax(list), "");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured: " + e.Message);
            }
            
            Console.Read();
        }
    }
}
