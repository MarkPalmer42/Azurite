using Azurite.LexicalParser;
using Azurite.SyntaxAnalysis.Grammar;
using Azurite.SyntaxAnalysis.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace Azurite.SyntaxAnalysis
{
    public class XMLSyntaxParserReader
    {

        List<SyntaxTreeNonterminal> nonTerminals = new List<SyntaxTreeNonterminal>();

        SyntaxGrammar grammar = new SyntaxGrammar();
        
        public SyntaxGrammar ReadGrammar(string xmlPath, string xsdPath)
        {
            XmlDocument xDoc = new XmlDocument();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
            settings.Schemas.Add(null, xsdPath);
            settings.IgnoreWhitespace = true;

            XmlReader reader = XmlReader.Create(xmlPath, settings);
            
            xDoc.Load(reader);

            xDoc.Validate(ValidationCallBack);

            ReadNonterminals(xDoc.GetElementsByTagName("nonterminals")[0]);
            ReadGrammarRules(xDoc.SelectNodes("azurite/syntax/rules/rule"));

            return grammar;
        }

        /// <summary>
        /// The callback method for the XML validation.
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="args">Event arguments</param>
        private void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Error)
            {
                throw new Exception("XML exception occured: " + args.Message);
            }
            else
            {
                Console.WriteLine("Warning: " + args.Message);
            }
        }

        private void ReadNonterminals(XmlNode node)
        {
            for (int i = 0; i < node.ChildNodes.Count; ++i)
            {
                nonTerminals.Add(new SyntaxTreeNonterminal(node.ChildNodes[i].InnerText));
            }
        }

        private void ReadGrammarRules(XmlNodeList nodeList)
        {
            foreach (XmlNode n in nodeList)
            {
                SyntaxTreeNonterminal leftSide = ReadLeftSide(n.ChildNodes[0].ChildNodes[0]);
                List<SyntaxTreeElement> rightSide = ReadRightSide(n.ChildNodes[1]);

                GrammarRule rule = new GrammarRule(leftSide, rightSide);

                grammar.AddRule(rule);
            }
        }

        private SyntaxTreeNonterminal ReadLeftSide(XmlNode leftSide)
        {
            string ntName = leftSide.InnerText;

            if (null == leftSide.Attributes || null == leftSide.Attributes["type"] || "nonterminal" != leftSide.Attributes["type"].Value)
            {
                throw new Exception("A left side element must be one nonterminal.");
            }

            return ReadNonterminal(leftSide);
        }

        private SyntaxTreeNonterminal ReadNonterminal(XmlNode node)
        {
            int idx = nonTerminals.FindIndex(x => x.Name == node.InnerText);

            if (-1 != idx)
            {
                return new SyntaxTreeNonterminal(node.InnerText);
            }
            else
            {
                throw new Exception("Unrecognized nonterminal symbol " + node.InnerText);
            }
        }

        private SyntaxTreeTerminal ReadTerminal(XmlNode node)
        {
            if (node.ChildNodes.Count < 1 || node.ChildNodes[0].Name != "terminal")
            {
                throw new Exception("Rule does not contain a terminal.");
            }

            if (node.ChildNodes[0].Attributes == null || node.ChildNodes[0].Attributes["type"] == null)
            {
                throw new Exception("A token must have a type.");
            }

            Token t = new Token(node.ChildNodes[0].InnerText, 0);

            t.SetTokenType(0, node.ChildNodes[0].Attributes["type"].Value);

            return new SyntaxTreeTerminal(t);
        }

        private List<SyntaxTreeElement> ReadRightSide(XmlNode rightSide)
        {
            List<SyntaxTreeElement> elems = new List<SyntaxTreeElement>();

            foreach (XmlNode elem in rightSide.ChildNodes)
            {
                if (null == elem.Attributes || null == elem.Attributes["type"])
                {
                    throw new Exception("A left side element must be one nonterminal.");
                }

                if ("nonterminal" == elem.Attributes["type"].Value)
                {
                    elems.Add(ReadNonterminal(elem));
                }
                else if ("terminal" == elem.Attributes["type"].Value)
                {
                    elems.Add(ReadTerminal(elem));
                }
                else
                {
                    throw new Exception("Invalid attribute value.");
                }
            }

            return elems;
        }

    }
}
