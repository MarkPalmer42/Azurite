
using System;
using System.Xml;
using System.Xml.Schema;

namespace Azurite.LexicalParser
{
    public class Parser
    {

        public Parser(string xmlPath, string xsdPath)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
            settings.Schemas.Add(null, xsdPath);
            
            XmlReader reader = XmlReader.Create(xmlPath, settings);
            
            while (reader.Read())
            {
                
            }
        }
        
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

    }

}
