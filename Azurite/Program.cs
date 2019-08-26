
using Azurite.LexicalParser;
using System;
using System.IO;

namespace Azurite
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Parser parser = new Parser("../../lexical_parser_rules.xml", "../../lexical_parser.xsd");

                string exampleProgram = File.ReadAllText("../../example_program.txt");

                var tokens = parser.Parse(exampleProgram);

                foreach (var t in tokens)
                {
                    Console.WriteLine("Name: " + t.Text + ", type: " + t.TokenType);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured: " + e.Message);
            }
            
            Console.Read();
        }
    }
}
