
using Azurite.SyntaxAnalysis.Grammar;
using Azurite.SyntaxAnalysis;
using System;
using System.Collections.Generic;
using Azurite.LexicalParser;
using Azurite.SyntaxAnalysis.SyntaxTree;

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
                SyntaxAnalyzer analyzer = new SyntaxAnalyzer("../../syntax_analysis_rules.xml", "../../syntax_analysis.xsd");

                SyntaxGrammar grammar = new SyntaxGrammar();

                grammar.AddSimpleRule('S', "E");
                grammar.AddSimpleRule('E', "E+T");
                grammar.AddSimpleRule('E', "T");
                grammar.AddSimpleRule('T', "T*F");
                grammar.AddSimpleRule('T', "F");
                grammar.AddSimpleRule('F', "(E)");
                grammar.AddSimpleRule('F', "a");

                SyntaxAnalyzer syntaxAnalysis = new SyntaxAnalyzer(grammar);

                string input = "a*a+a*(a*a+(a))";

                List<Token> tokens = new List<Token>();

                foreach (var i in input)
                {
                    tokens.Add(new Token(i.ToString(), 0));
                }

                var syntaxtree = syntaxAnalysis.AnalyzeSyntax(tokens);

                WriteSyntaxTree(syntaxtree);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured: " + e.Message);
            }
            
            Console.Read();
        }
    }
}
