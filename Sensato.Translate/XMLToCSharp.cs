using Sensato.Translate.Entities;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;


namespace Sensato.Translate
{
    public class XMLToCSharp
    {
        public static string TranslateToCSharp(csXML model)
        {
            XmlDocument xmlDocument = SerializeToXML(model);
            string csSharpModel = SerializeToCSharp(xmlDocument);
            return string.Empty;
        }

        private static XmlDocument SerializeToXML(csXML model)
        {
            XmlDocument newFile = new XmlDocument();  // document creation
            XmlDeclaration declaration = newFile.CreateXmlDeclaration(model.version, model.encoding, null);
            XmlElement heading = newFile.DocumentElement; //xml declaration
            newFile.InsertBefore(declaration, heading);

            XmlElement document = newFile.CreateElement("document",string.Empty); // document initial structure, where the parts are declared to 
            newFile.AppendChild(document);

            XmlElement references = newFile.CreateElement("references", string.Empty);
            XmlElement namespaces = newFile.CreateElement("namespace", string.Empty);

            if (model.document.references.Count>0) // in the first level it's declared the tag 'References', after verifying that this attribute exists
            {   
                document.AppendChild(references);

                for (var i=0; i < model.document.references.Count; i++)
                {
                    XmlElement usings = newFile.CreateElement("using",string.Empty); // declaration of the child from 'References' tag 
                    XmlAttribute usingAttr = newFile.CreateAttribute("name");
                    usingAttr.Value = model.document.references[i].name;
                    usings.Attributes.Append(usingAttr);
                    references.AppendChild(usings);
                }
            }

            if (model.document.csNamespace.name != null) // at the same level that 'References', 'Namespace' tag is added after it. But first we have to verify if the attribute to namespace exists
            {
                document.InsertAfter(namespaces, references);
                XmlAttribute namespaceAttr = newFile.CreateAttribute("name"); // then we add the name of the namespace
                namespaceAttr.Value = model.document.csNamespace.name;
                namespaces.Attributes.Append(namespaceAttr);

                if (model.document.csNamespace.Classes.Count >0) // inside of the 'Namespace' tag, we have to validate if one or more clases exists, it it's true there are inserted
                {
                    foreach (var item in model.document.csNamespace.Classes)
                    {
                        XmlElement classes = newFile.CreateElement("class", string.Empty);
                        XmlAttribute classAttrInheritance = newFile.CreateAttribute("inheritance");
                        classAttrInheritance.Value = item.inheritance;
                        classes.Attributes.Append(classAttrInheritance);

                        XmlAttribute classAttrModifiers = newFile.CreateAttribute("modifiers");
                        classAttrModifiers.Value = item.modifiers;
                        classes.Attributes.Append(classAttrModifiers);

                        XmlAttribute classAttrName = newFile.CreateAttribute("name");
                        classAttrName.Value = item.name;
                        classes.Attributes.Append(classAttrName);

                        XmlAttribute classAttrPartial = newFile.CreateAttribute("partial");
                        classAttrPartial.Value = item.partial;
                        classes.Attributes.Append(classAttrPartial);

                        namespaces.AppendChild(classes);
                    }
                    //for (var i=0; i < model.document.csNamespace.Classes.Count; i++)
                    //{
                    //    for (var j=0; j<model.document.csNamespace.Classes.Capacity; j++)
                    //    {
                    //        XmlElement classes = newFile.CreateElement("class", string.Empty); // declaration of the child from 'Namespace' tag

                    //        XmlAttribute classAttrInheritance = newFile.CreateAttribute("inheritance"); // for each attribute from 'Classes', a sentence is defines to insert the name and value of the property
                    //        classAttrInheritance.Value = model.document.csNamespace.Classes[i].inheritance[j].ToString();
                    //        classes.Attributes.Append(classAttrInheritance);

                    //        XmlAttribute classAttrModifiers = newFile.CreateAttribute("modifiers");
                    //        classAttrModifiers.Value = model.document.csNamespace.Classes[i].modifiers[j].ToString();
                    //        classes.Attributes.Append(classAttrModifiers);

                    //        XmlAttribute classAttrName = newFile.CreateAttribute("name");
                    //        classAttrName.Value = model.document.csNamespace.Classes[i].name[j].ToString();
                    //        classes.Attributes.Append(classAttrName);

                    //        XmlAttribute classAttrPartial = newFile.CreateAttribute("name");
                    //        classAttrPartial.Value = model.document.csNamespace.Classes[i].partial[j].ToString();
                    //        classes.Attributes.Append(classAttrPartial);

                    //        namespaces.AppendChild(classes);
                    //    }
                    //}
                }
            }

            newFile.Save("C:/Users/Carolina Martinez/Desktop/FinalSample.xml");
            
            return newFile;
        }

        private static string SerializeToCSharp(XmlDocument document)
        {
            return string.Empty;
        }
    }
}
