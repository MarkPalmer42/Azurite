
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;

namespace Azurite.LexicalParser
{

    /// <summary>
    /// This class reads the XML files that contain the parser information.
    /// </summary>
    class XMLParserReader
    {

        /// <summary>
        /// The list of the token names.
        /// </summary>
        public List<string> ElementNames { get; private set; }

        /// <summary>
        /// The list of the automatas parsed from the XML.
        /// </summary>
        List<IAutomata> automataList = new List<IAutomata>();

        /// <summary>
        /// The possible automata state types.
        /// </summary>
        readonly List<string> automataStateTypes = new List<string>() { "start", "undefined", "accepted" };

        /// <summary>
        /// Reads an XML parser and retrieves the list of the automatas.
        /// </summary>
        /// <param name="xmlPath">The path of the XML file</param>
        /// <param name="xsdPath">The path of the XSD file</param>
        /// <returns></returns>
        public List<IAutomata> ReadXML(string xmlPath, string xsdPath)
        {
            ElementNames = new List<string>();

            automataList.Clear();
            ElementNames.Clear();

            XmlDocument xDoc = new XmlDocument();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
            settings.Schemas.Add(null, xsdPath);

            XmlReader reader = XmlReader.Create(xmlPath, settings);

            xDoc.Load(reader);

            xDoc.Validate(ValidationCallBack);

            ReadElements(xDoc.GetElementsByTagName("elements")[0]);
            ReadAutomata(xDoc.SelectNodes("azurite/lexical/automatas/automata"));

            return automataList;
        }

        /// <summary>
        /// Reads the list of elements, the types of tokens.
        /// </summary>
        /// <param name="node">The Xml Node containing the elements node</param>
        private void ReadElements(XmlNode node)
        {
            for (int i = 0; i < node.ChildNodes.Count; ++i)
            {
                ElementNames.Add(node.ChildNodes[i].InnerText);
            }
        }

        /// <summary>
        /// Reads an automata.
        /// </summary>
        /// <param name="node">The XmlNodeList containing the automatas element.</param>
        private void ReadAutomata(XmlNodeList node)
        {
            foreach (XmlNode n in node)
            {
                string type = n.Attributes["type"].Value;
                string name = n.Attributes["name"].Value;
                int outputIndex = -1;

                if (null != n.Attributes["output"])
                {
                    string output = n.Attributes["output"].Value;

                    outputIndex = ElementNames.FindIndex(x => x == output);

                    if (-1 == outputIndex)
                    {
                        throw new Exception("Invalid output " + output);
                    }
                }

                if ("stateless" == type)
                {
                    StatelessAutomata st = new StatelessAutomata(outputIndex, name);

                    foreach (XmlNode n2 in n.ChildNodes[0].ChildNodes)
                    {
                        st.AddAcceptedText(n2.InnerText);
                    }

                    automataList.Add(st);
                }
                else if ("stated" == type)
                {
                    XmlNode tmpNode = SelectFirst(n);

                    if (null != tmpNode)
                    {
                        ReadStatedAutomata(tmpNode.ChildNodes, outputIndex, name);
                    }
                }
                else
                {
                    throw new Exception("Invalid automata type " + type);
                }
            }
        }

        /// <summary>
        /// Reads a stated automata.
        /// </summary>
        /// <param name="nList">The node list containing the automata element</param>
        /// <param name="outputIndex">The index of the generated token</param>
        /// <param name="name">The name of the automata</param>
        private void ReadStatedAutomata(XmlNodeList nList, int outputIndex, string name)
        {
            StatedAutomata st = new StatedAutomata(outputIndex, name);

            foreach (XmlNode n2 in nList)
            {
                int id = int.Parse(n2.Attributes["id"].Value);
                string stateType = n2.Attributes["type"].Value;
                int stIndex = automataStateTypes.FindIndex(x => x == stateType);
                AutomataStateType sType = (AutomataStateType)stIndex;

                AutomataStateDefinition asd = new AutomataStateDefinition(id, sType);

                XmlNode tmpNode = SelectFirst(n2);

                if (null != tmpNode)
                {
                    foreach (XmlNode n3 in tmpNode.ChildNodes)
                    {
                        int target = int.Parse(n3.Attributes["target"].Value);
                        bool any = !n3.HasChildNodes;

                        StateTransition t = new StateTransition(any, target);

                        foreach (XmlNode n4 in n3.ChildNodes)
                        {
                            if (n4.NodeType != XmlNodeType.Element)
                            {
                                continue;
                            }

                            if (0 != n4.Attributes.Count)
                            {
                                string from = n4.Attributes["from"].Value;
                                string to = n4.Attributes["to"].Value;

                                if (null == from || null == to)
                                {
                                    throw new Exception("Ranges are not defined.");
                                }
                                else if (1 != from.Length || 1 != to.Length)
                                {
                                    throw new Exception("Invalid range values defined.");
                                }
                                else
                                {
                                    t.AddAcceptedInterval(from[0], to[0]);
                                }
                            }
                            else
                            {
                                t.AddAcceptedChar(n4.InnerText[0]);
                            }
                        }

                        asd.AddStateTransition(t);
                    }
                }

                st.AddStateDefinition(asd);
            }

            automataList.Add(st);
        }

        /// <summary>
        /// This method select the first Element type node from the given node's children.
        /// </summary>
        /// <param name="n">The given node</param>
        /// <returns>The first element type, null if it does not exists</returns>
        private XmlNode SelectFirst(XmlNode n)
        {
            bool found = false;
            int i = 0;

            while (!found && i < n.ChildNodes.Count)
            {
                found = n.ChildNodes[i].NodeType == XmlNodeType.Element;
                ++i;
            }

            return found ? n.ChildNodes[i - 1] : null;
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

    }

}
