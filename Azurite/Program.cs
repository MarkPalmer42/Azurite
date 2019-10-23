
using Azurite.SyntaxAnalysis.Grammar;
using Azurite.SyntaxAnalysis;
using System;
using System.Collections.Generic;
using Azurite.LexicalParser;

namespace Azurite
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                SyntaxGrammar grammar = new SyntaxGrammar();

                grammar.AddSimpleRule('S', "E");
                grammar.AddSimpleRule('E', "E+T");
                grammar.AddSimpleRule('E', "T");
                grammar.AddSimpleRule('T', "T*F");
                grammar.AddSimpleRule('T', "F");
                grammar.AddSimpleRule('F', "E");
                grammar.AddSimpleRule('F', "a");

                SyntaxAnalyzer syntaxAnalysis = new SyntaxAnalyzer(grammar);

                string input = "a*a+a";

                List<Token> tokens = new List<Token>();

                foreach (var i in input)
                {
                    tokens.Add(new Token(i.ToString(), 0));
                }

                var syntaxtree = syntaxAnalysis.AnalyzeSyntax(tokens);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured: " + e.Message);
            }
            
            Console.Read();
        }
    }
}
