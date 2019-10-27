
using Azurite.SyntaxAnalysis.Grammar;
using Azurite.SyntaxAnalysis;
using System;
using System.Collections.Generic;
using Azurite.LexicalParser;
using Azurite.SyntaxAnalysis.SyntaxTree;
using System.IO;

namespace Azurite
{
    class Program
    {

        static void WriteSyntaxTree(SyntaxTreeElement elem, string padding = "")
        {
            SyntaxTreeNonterminal nt = elem as SyntaxTreeNonterminal;
            SyntaxTreeTerminal t = elem as SyntaxTreeTerminal;

            if (null != nt)
            {
                Console.WriteLine(padding + nt.Name);

                foreach (var c in nt.Children)
                {
                    WriteSyntaxTree(c, padding + "  ");
                }
            }
            else if (null != t)
            {
                Console.WriteLine(padding + t.SyntaxToken.Text);
            }
        }

        static void Main(string[] args)
        {
            try
            {
                Parser lexicalParser = new Parser("../../lexical_parser_rules.xml", "../../lexical_parser.xsd");

                SyntaxAnalyzer analyzer = new SyntaxAnalyzer("../../syntax_analysis_rules.xml", "../../syntax_analysis.xsd");

                string file = File.ReadAllText("../../example_program.txt");

                List<Token> tokens = lexicalParser.Parse(file);

                var syntaxTree = analyzer.AnalyzeSyntax(tokens);

                WriteSyntaxTree(syntaxTree);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured: " + e.Message);
            }
            
            Console.Read();
        }
    }
}
