using Sensato.Translate.Entities;
using Sensato.Translate.Resources;
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
            //String Ejemplo = FirstTemplate.Ejemplo;
            //Ejemplo = String.Format(Ejemplo, "public", "Ejemplo","var x= 0;"); // en el metodo serialze to csSharp

            //string namespaces = FirstTemplate.NamespaceTemplate;
            //namespaces = String.Format(namespaces, "Sensato.Translate", Ejemplo);

            XmlDocument xmlDocument = SerializeToXML(model);
            string csSharpModel = SerializeToCSharp(xmlDocument);
            return string.Empty; // esta se reemplaza por csSharpModel
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
                    foreach (var item in model.document.csNamespace.Classes) // for each attribute from 'Classes', a sentence is defines to insert the name and value of the property
                    {
                        XmlElement classes = newFile.CreateElement("class", string.Empty); // declaration of the child from 'Namespace' tag

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
                }
            }
            newFile.Save("C:/Users/Carolina Martinez/Desktop/VarDuplicateDocument.xml");
            return newFile;
        }

        private static string SerializeToCSharp(XmlDocument document)
        {
            if (document.DocumentElement.HasChildNodes)
            {
                var elements = document.DocumentElement.FirstChild;
                Console.WriteLine(elements);
            }
            string classOrClasses = TemplatesCollection.ClassTemplate;
            classOrClasses = String.Format(classOrClasses,"TipoDeMetodo","nombreClase","funciones");

            string namespaces = TemplatesCollection.NamespaceTemplate;
            namespaces = String.Format(namespaces, "nombreDelNamespace", classOrClasses);

            return string.Empty;
        }
    }
}
