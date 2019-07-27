
using Azurite.LexicalParser;
using System;

namespace Azurite
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Parser parser = new Parser("../../lexical_parser_rules.xml", "../../lexical_parser.xsd");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured: " + e.Message);
            }
            
            Console.Read();
        }
    }
}
