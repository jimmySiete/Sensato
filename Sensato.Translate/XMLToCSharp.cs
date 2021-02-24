using Sensato.Translate.Entities;
using Sensato.Translate.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;


namespace Sensato.Translate
{
    public class XMLToCSharp
    {
        public static string TranslateToCSharp(csXML model)
        {
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

            XmlElement document = newFile.CreateElement("document",string.Empty); // document initial structure, where the parts are declared to specify the structure of the document.
            newFile.AppendChild(document);

            XmlElement references = newFile.CreateElement("references", string.Empty);
            XmlElement namespaces = newFile.CreateElement("namespace", string.Empty);
            XmlElement finalClass = newFile.CreateElement("class", string.Empty);

            if (model.document.references.Count>0) // in the first level it's declared the tag 'References', after verifying that this attribute exists.
            {   
                document.AppendChild(references);

                for (var i=0; i < model.document.references.Count; i++)
                {
                    XmlElement usings = newFile.CreateElement("using",string.Empty); // declaration of the child from 'References' tag. 
                    XmlAttribute usingAttr = newFile.CreateAttribute("name");
                    usingAttr.Value = model.document.references[i].name;
                    usings.Attributes.Append(usingAttr);
                    references.AppendChild(usings);
                }
            }

            if (model.document.csNamespace.name != null) // at the same level that 'References', 'Namespace' tag is added after it. But first we have to verify if the attribute to namespace exists.
            {
                document.InsertAfter(namespaces, references);
                XmlAttribute namespaceAttr = newFile.CreateAttribute("name"); // then we add the name of the namespace.
                namespaceAttr.Value = model.document.csNamespace.name;
                namespaces.Attributes.Append(namespaceAttr);

                if (model.document.csNamespace.Classes.Count > 0) // inside of the 'Namespace' tag, we have to validate if one or more clases exists, it it's true there are inserted.
                {
                    foreach (var item in model.document.csNamespace.Classes) // for each attribute from 'Classes', a sentence is defines to insert the name and value of the property.
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
                                        for (var j = 0; j < model.document.csNamespace.Classes[i].lines.Capacity; j++) // prodecure used to create and insert the attributes from each existant variable, constructor or method.
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

                                            XmlAttribute attrLine = newFile.CreateAttribute("line");
                                            attrLine.Value = lineCaster.line.ToString();
                                            variables.Attributes.Append(attrLine);

                                            XmlAttribute attrGetOrSet = newFile.CreateAttribute("getterOrSetter");
                                            attrGetOrSet.Value = lineCaster.getterOrSetter.ToString();
                                            variables.Attributes.Append(attrGetOrSet);

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
            string CSharpContainerCode = "";
            string classes = "";
            string listOfVariables = "";
            string references = "";
            string namespaces = "";

            // The first thing to do is to check if our XML document isn't null or empty.
            if (document != null && document.HasChildNodes)
            {
                var referenceNode = document.SelectSingleNode("//*[local-name()='references']"); // expression used to select a specific node.

                if (referenceNode.ChildNodes.Count > 0) // At this first part of the translation, if we have one or more references, there are listed in a format.
                {
                    for (var item = 0; item < referenceNode.ChildNodes.Count; item++)
                    {
                        references = TemplatesCollection.ReferenceTemplate;
                        references = String.Format(references, referenceNode.ChildNodes[item].Attributes.GetNamedItem("name").Value);
                        Console.WriteLine(references);
                    }
                }

                var namespaceNode = document.SelectSingleNode("//*[local-name()='namespace']"); // expression used to select a specific node.

                if (namespaceNode != null)// after that, if there's a namespace, we have to add it and all it's classes, methods, constructors and variables.
                {
                    namespaces = TemplatesCollection.NamespaceTemplate;
                    if (namespaceNode.HasChildNodes) // another validation to know if namespace has content.
                    {
                        for (var i=0; i < namespaceNode.ChildNodes.Count; i++) // this procedure help us to know how many classes are and if each class has lines of code.
                        {
                            // about structure, we have the constructors listed in here and after that, the methods & lines of code.

                            if (namespaceNode.ChildNodes[i].HasChildNodes)
                            {
                                string typeOfElement = namespaceNode.ChildNodes[i].FirstChild.Name;
                                switch (typeOfElement) // used to write all the line codes or methods given
                                {
                                    case "var":
                                        for (var j = 0; j < namespaceNode.ChildNodes[i].ChildNodes.Count; j++)
                                        {
                                            XmlNode varNode = namespaceNode.ChildNodes[i].ChildNodes[j];
                                            string variables = TemplatesCollection.VariableTemplate;
                                            if (varNode.Attributes.GetNamedItem("getterOrSetter").Value.ToString().ToLower() == "true") // this validation is read when the variable is type class
                                            {
                                                if (varNode.Attributes.GetNamedItem("isStatic").Value.ToString().ToLower() == "true")
                                                {
                                                    variables = String.Format(variables, varNode.Attributes.GetNamedItem("line").Value, varNode.Attributes.GetNamedItem("modifier").Value + " static", varNode.Attributes.GetNamedItem("type").Value, varNode.Attributes.GetNamedItem("name").Value, "{ get; set; }");
                                                    listOfVariables += variables + "\n\r";
                                                }
                                                else
                                                {
                                                    variables = String.Format(variables, varNode.Attributes.GetNamedItem("line").Value, varNode.Attributes.GetNamedItem("modifier").Value, varNode.Attributes.GetNamedItem("type").Value, varNode.Attributes.GetNamedItem("name").Value, "{ get; set; }");
                                                    listOfVariables += variables + "\n\r";
                                                }
                                            }
                                            else
                                            {
                                                if (varNode.Attributes.GetNamedItem("isStatic").Value.ToString().ToLower() == "true")
                                                {
                                                    variables = String.Format(variables, varNode.Attributes.GetNamedItem("line").Value, varNode.Attributes.GetNamedItem("modifier").Value + " static", varNode.Attributes.GetNamedItem("type").Value, varNode.Attributes.GetNamedItem("name").Value, varNode.Attributes.GetNamedItem("value").Value);
                                                    listOfVariables += variables + "\n\r";
                                                }
                                                else
                                                {
                                                    variables = String.Format(variables, varNode.Attributes.GetNamedItem("line").Value, varNode.Attributes.GetNamedItem("modifier").Value, varNode.Attributes.GetNamedItem("type").Value, varNode.Attributes.GetNamedItem("name").Value, varNode.Attributes.GetNamedItem("value").Value);
                                                    listOfVariables += variables + "\n\r";
                                                }
                                            }
                                        }
                                        break;
                                    case "methods":
                                        // por cada metodo imprimir las lineas de codigo que vengan
                                        break;
                                }
                            }
                            classes = TemplatesCollection.ClassTemplate;
                            classes = String.Format(classes, namespaceNode.ChildNodes[i].Attributes.GetNamedItem("modifiers").Value, namespaceNode.ChildNodes[i].Attributes.GetNamedItem("name").Value, listOfVariables);
                        }
                        for (var k=0; k < namespaceNode.ChildNodes.Count; k++ )
                        {
                            namespaces = string.Format(namespaces, namespaceNode.Attributes.GetNamedItem("name").Value, classes);
                            Console.WriteLine(namespaces);
                        }
                    }
                }
            }
            CSharpContainerCode = references + namespaces;
            return CSharpContainerCode;  // se reemplaza con la string chida, que en este caso es CSharpContainerCode
        }
    }
}
