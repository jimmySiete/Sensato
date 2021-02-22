﻿using Sensato.Translate.Entities;
using Sensato.Translate.Resources;
using System;
using System.Linq;
using System.Reflection;
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
            XmlElement finalClass = newFile.CreateElement("class", string.Empty);

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

                if (model.document.csNamespace.Classes.Count > 0) // inside of the 'Namespace' tag, we have to validate if one or more clases exists, it it's true there are inserted
                {
                    foreach (var item in model.document.csNamespace.Classes) // for each attribute from 'Classes', a sentence is defines to insert the name and value of the property
                    {
                        XmlAttribute classAttrInheritance = newFile.CreateAttribute("inheritance");
                        classAttrInheritance.Value = item.inheritance;
                        finalClass.Attributes.Append(classAttrInheritance);

                        XmlAttribute classAttrModifiers = newFile.CreateAttribute("modifiers");
                        classAttrModifiers.Value = item.modifiers;
                        finalClass.Attributes.Append(classAttrModifiers);

                        XmlAttribute classAttrName = newFile.CreateAttribute("name");
                        classAttrName.Value = item.name;
                        finalClass.Attributes.Append(classAttrName);

                        XmlAttribute classAttrPartial = newFile.CreateAttribute("partial");
                        classAttrPartial.Value = item.partial;
                        finalClass.Attributes.Append(classAttrPartial);

                        namespaces.AppendChild(finalClass);
                    }

                    for (var i = 0; i < model.document.csNamespace.Classes.Count(); i++)
                    {
                        if (model.document.csNamespace.Classes[i].constructors.Count > 0)
                        {
                            foreach (var item in model.document.csNamespace.Classes[i].constructors)
                            {
                                /// codigo para añadir etiqueta de tipo csConstructor
                            }

                        } else
                        if (model.document.csNamespace.Classes[i].lines.Count > 0)
                        {
                            var anotherLineCaster = (csVar)model.document.csNamespace.Classes[i].lines.First();
                            switch (anotherLineCaster.GetType().Name)
                            { 
                                case "csVar":
                                    foreach (var item in model.document.csNamespace.Classes)
                                    {
                                        /// codigo para agregar las variables/// falta construir el switchcase 
                                        for (var j = 0; j < model.document.csNamespace.Classes[i].lines.Capacity; j++) // prodecure used to create and insert the attributes from each existant variable, constructor or method
                                        {
                                            var lineCaster = (csVar)model.document.csNamespace.Classes[i].lines[j];
                                            XmlElement variables = newFile.CreateElement("var", string.Empty);

                                            XmlAttribute attrName = newFile.CreateAttribute("name");
                                            attrName.Value = lineCaster.name;
                                            variables.Attributes.Append(attrName);

                                            XmlAttribute attrModifier = newFile.CreateAttribute("modifier");
                                            attrModifier.Value = lineCaster.modifier;
                                            variables.Attributes.Append(attrModifier);

                                            XmlAttribute attrStatic = newFile.CreateAttribute("isStatic");
                                            attrStatic.Value = lineCaster.isStatic.ToString();
                                            variables.Attributes.Append(attrStatic);

                                            XmlAttribute attrValue = newFile.CreateAttribute("value");
                                            attrValue.Value = lineCaster.value.ToString();
                                            variables.Attributes.Append(attrValue);

                                            XmlAttribute attrType = newFile.CreateAttribute("type");
                                            attrType.Value = lineCaster.type;
                                            variables.Attributes.Append(attrType);

                                            finalClass.AppendChild(variables);
                                        }
                                    }
                                    break;

                                case "csMethods":
                                    break;
                            }
                        } 
                    
                    }
                }

            }
            return newFile;
        }

        private static string SerializeToCSharp(XmlDocument document)
        {
            if (document.DocumentElement.ChildNodes.Count > 0)
            {
                if (document.DocumentElement.FirstChild.Name == "references")
                {
                    for (var i=0; i < document.DocumentElement.FirstChild.ChildNodes.Count; i++)
                    {
                        string references = TemplatesCollection.ReferenceTemplate;
                        references = String.Format(references,document.DocumentElement.FirstChild.ChildNodes[i].Attributes.GetNamedItem("name").Value);
                        Console.WriteLine(references);
                    }
                    //foreach (var item in document.DocumentElement.FirstChild)
                    //{
                    //    string references = TemplatesCollection.ReferenceTemplate;
                    //    references = String.Format(references);
                    //    Console.WriteLine(references);
                    //}
                }
                var elements = document.ChildNodes;
                Console.WriteLine(elements.Item(0).LocalName);
            }

            string classOrClasses = TemplatesCollection.ClassTemplate;
            classOrClasses = String.Format(classOrClasses,"TipoDeMetodo","nombreClase","funciones");

            string namespaces = TemplatesCollection.NamespaceTemplate;
            namespaces = String.Format(namespaces, "nombreDelNamespace", classOrClasses);

            return string.Empty;  // se reemplaza con la string chida
        }
    }
}
