﻿using Sensato.Translate.Entities;
using Sensato.Translate.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;


namespace Sensato.Translate
{
    public class XMLToCSharp
    {
        public static string TranslateToCSharp(csXML model)
        {
            if (model == null)
                throw new TranslateException(ErrorAndExceptionsCatalog._701_Code, ErrorAndExceptionsCatalog._701_ModelNotFound);

            XmlDocument xmlDocument = SerializeToXML(model);
                string csSharpModel = SerializeToCSharp(xmlDocument);
            return csSharpModel; 
        }

        private static XmlDocument SerializeToXML(csXML model)
        {
            XmlDocument newFile = new XmlDocument();  // document creation
            try
            {
                XMLStructureValidations(model);
                XmlDeclaration declaration = newFile.CreateXmlDeclaration(model.version, model.encoding, null);
                XmlElement heading = newFile.DocumentElement; //xml declaration
                newFile.InsertBefore(declaration, heading);

                XmlElement document = newFile.CreateElement("document", string.Empty); // document initial structure, where the parts are declared to specify the structure of the document.
                newFile.AppendChild(document);

                XmlElement references = newFile.CreateElement("references", string.Empty);
                XmlElement namespaces = newFile.CreateElement("namespace", string.Empty);
                XmlElement finalClass = newFile.CreateElement("class", string.Empty);
                XmlElement constructors = newFile.CreateElement("constructor", string.Empty);

                if (model.document.references.Count > 0) // in the first level it's declared the tag 'References', after verifying that this attribute exists.
                {
                    document.AppendChild(references);

                    for (var i = 0; i < model.document.references.Count; i++)
                    {
                        XmlElement usings = newFile.CreateElement("using", string.Empty); // declaration of the child from 'References' tag. 
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
                            if (model.document.csNamespace.Classes[i].constructors.Count > 0) // we have to declare the list of constructors first, if they are contained in the received model. 
                            {
                                foreach (var item in model.document.csNamespace.Classes[i].constructors) // tag created for each constructor inside the classes.
                                {
                                    XmlAttribute constructorClass = newFile.CreateAttribute("class");
                                    constructorClass.Value = model.document.csNamespace.Classes[i].constructors.FirstOrDefault().classConstructor.name;
                                    constructors.Attributes.Append(constructorClass);

                                    if (model.document.csNamespace.Classes[i].constructors.Count == 1 && model.document.csNamespace.Classes[i].constructors[i].csArguments.Any())   // Rule about constructors: if we have one constructor with parameters
                                    {                                                                                                                                               // then we have to add an empty constructor and the other with all the data. 
                                        var args = model.document.csNamespace.Classes[i].constructors[i].csArguments;
                                        XmlElement emptyConstructor = newFile.CreateElement("constructor", string.Empty);

                                        XmlAttribute constructorNameClass = newFile.CreateAttribute("class");
                                        constructorNameClass.Value = constructorClass.Value;
                                        emptyConstructor.Attributes.Append(constructorNameClass);

                                        XmlAttribute emptyArgs = newFile.CreateAttribute("arguments");
                                        emptyArgs.Value = null;
                                        emptyConstructor.Attributes.Append(emptyArgs);

                                        XmlAttribute emptyLines = newFile.CreateAttribute("lines");
                                        emptyLines.Value = null;
                                        emptyConstructor.Attributes.Append(emptyLines);

                                        finalClass.AppendChild(emptyConstructor);

                                        XmlElement nonEmptyConstructor = newFile.CreateElement("constructor", string.Empty);

                                        XmlAttribute nonEmptyConstructorClass = newFile.CreateAttribute("class");
                                        nonEmptyConstructorClass.Value = constructorClass.Value;
                                        nonEmptyConstructor.Attributes.Append(nonEmptyConstructorClass);

                                        XmlAttribute nonEmptyArgs = newFile.CreateAttribute("arguments");
                                        nonEmptyArgs.Value = ResultantArguments(args);
                                        nonEmptyConstructor.Attributes.Append(nonEmptyArgs);

                                        XmlAttribute constructorLines = newFile.CreateAttribute("lines");
                                        for (var index = 0; index < model.document.csNamespace.Classes[i].constructors.Count; index++) // the same logic is used like in the arguments list.
                                        {
                                            if (model.document.csNamespace.Classes[i].constructors[index].csLines != null)
                                                constructorLines.Value = model.document.csNamespace.Classes[i].constructors[index].csLines.ToString();
                                            else
                                                constructorLines.Value = null;
                                            nonEmptyConstructor.Attributes.Append(constructorLines);
                                        }

                                        finalClass.AppendChild(nonEmptyConstructor);
                                    }
                                    else // when we have more than two constructors, these are printed with its elements.
                                    {
                                        XmlAttribute constructorArguments = newFile.CreateAttribute("arguments");

                                        for (var index = 0; index < model.document.csNamespace.Classes[i].constructors.Count; index++)
                                        {
                                            if (model.document.csNamespace.Classes[i].constructors[index].csArguments != null)
                                                constructorArguments.Value = ResultantArguments(model.document.csNamespace.Classes[i].constructors[index].csArguments); // iterated arguments are added to the corresponding tag.
                                            else
                                                constructorArguments.Value = null;
                                            constructors.Attributes.Append(constructorArguments);
                                        }

                                        XmlAttribute constructorLines = newFile.CreateAttribute("lines");
                                        for (var index = 0; index < model.document.csNamespace.Classes[i].constructors.Count; index++) // the same logic is used like in the arguments list.
                                        {
                                            if (model.document.csNamespace.Classes[i].constructors[index].csLines.Count > 0)
                                                constructorLines.Value = model.document.csNamespace.Classes[i].constructors[index].csLines.ToString();
                                            else
                                                constructorLines.Value = null;
                                            constructors.Attributes.Append(constructorLines);
                                        }
                                        finalClass.AppendChild(constructors);
                                    }
                                }
                            }

                            if (model.document.csNamespace.Classes[i].lines.Count > 0)
                            {
                                foreach (var item in model.document.csNamespace.Classes[i].lines) // for each class, we found variables and methods, depending of the value from the current line we are printing a variable or methods.
                                {
                                    string opts = item.GetType().Name;
                                    switch (opts)
                                    {
                                        case "csVar":
                                            foreach (var varItem in model.document.csNamespace.Classes)
                                            {
                                                var lineCasterVar = (csVar)item;
                                                XmlElement variables = newFile.CreateElement("var", string.Empty);

                                                XmlAttribute attrName = newFile.CreateAttribute("name");
                                                attrName.Value = lineCasterVar.name;
                                                variables.Attributes.Append(attrName);

                                                XmlAttribute attrModifier = newFile.CreateAttribute("modifier");
                                                attrModifier.Value = lineCasterVar.modifier;
                                                variables.Attributes.Append(attrModifier);

                                                XmlAttribute attrStatic = newFile.CreateAttribute("isStatic");
                                                attrStatic.Value = lineCasterVar.isStatic.ToString();
                                                variables.Attributes.Append(attrStatic);

                                                XmlAttribute attrValue = newFile.CreateAttribute("value");
                                                attrValue.Value = lineCasterVar.value.ToString();
                                                variables.Attributes.Append(attrValue);

                                                XmlAttribute attrType = newFile.CreateAttribute("type");
                                                attrType.Value = lineCasterVar.type;
                                                variables.Attributes.Append(attrType);

                                                XmlAttribute attrLine = newFile.CreateAttribute("line");
                                                attrLine.Value = lineCasterVar.line.ToString();
                                                variables.Attributes.Append(attrLine);

                                                XmlAttribute attrGetOrSet = newFile.CreateAttribute("getterOrSetter");
                                                attrGetOrSet.Value = lineCasterVar.getterOrSetter.ToString();
                                                variables.Attributes.Append(attrGetOrSet);

                                                finalClass.AppendChild(variables);
                                            }
                                            break;

                                        case "csMethods":
                                            foreach (var methodItem in model.document.csNamespace.Classes)
                                            {
                                                var lineCasterMethod = (csMethods)item;
                                                XmlElement methods = newFile.CreateElement("methods", string.Empty);

                                                XmlAttribute methodName = newFile.CreateAttribute("name");
                                                methodName.Value = lineCasterMethod.name;
                                                methods.Attributes.Append(methodName);

                                                XmlAttribute methodArgs = newFile.CreateAttribute("arguments");
                                                if (lineCasterMethod.arguments != null)
                                                    methodArgs.Value = ResultantArguments(lineCasterMethod.arguments);
                                                else
                                                    methodArgs.Value = null;
                                                methods.Attributes.Append(methodArgs);

                                                XmlAttribute isStatic = newFile.CreateAttribute("static");
                                                isStatic.Value = lineCasterMethod.isStatic.ToString();
                                                methods.Attributes.Append(isStatic);

                                                XmlAttribute isReturn = newFile.CreateAttribute("returned");
                                                isReturn.Value = lineCasterMethod.isReturned.ToString();
                                                methods.Attributes.Append(isReturn);

                                                XmlAttribute dataTypeReturn = newFile.CreateAttribute("dataTypeReturn");
                                                dataTypeReturn.Value = lineCasterMethod.dataTypeReturn;
                                                methods.Attributes.Append(dataTypeReturn);

                                                XmlAttribute methodLines = newFile.CreateAttribute("lines");
                                                if (lineCasterMethod.lines.Count > 0) // lines of code are contained inside the methods, at the same time we have to insert the lineMethod node.
                                                {
                                                    foreach (var lineItem in lineCasterMethod.lines) // inside the method it is the lines of code contained in the model.
                                                    {
                                                        XmlElement lineMethod = newFile.CreateElement("line", string.Empty);
                                                        XmlAttribute line = newFile.CreateAttribute("number");
                                                        line.Value = lineItem.line.ToString();
                                                        lineMethod.Attributes.Append(line);
                                                        XmlAttribute lineChain = newFile.CreateAttribute("chain");
                                                        lineChain.Value = lineItem.lineCode;
                                                        lineMethod.Attributes.Append(lineChain);
                                                        // then we have to complete the attribute 'execute methods' bc don't remember how this part works.
                                                        methods.AppendChild(lineMethod);
                                                    }
                                                    methodLines.Value = lineCasterMethod.lines.ToString();
                                                }
                                                else
                                                {
                                                    methodLines.Value = null;
                                                }
                                                methods.Attributes.Append(methodLines);
                                                finalClass.AppendChild(methods);
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
                //newFile.Save("C:/Users/Carolina Martinez/Desktop/ConstructorSample.xml"); // this line is only enabled when we update the code or to test new specifications.
                return newFile;
            }
            catch (Exception ex)
            {
                XMLStructureValidations(model);
                if (ex != null)
                    throw new TranslateException(ErrorAndExceptionsCatalog._705_Code, ErrorAndExceptionsCatalog._705_NotCreatedNode);
                return newFile;
            }
        }

        private static string SerializeToCSharp(XmlDocument document)
        {
            string classes = "";
            string listOfVariables = "";
            string references = "";
            string namespaces = "";
            string constructorWithoutParameters = "";
            string constructorWithParameters = "";
            string listOfMethods = "";

            try
            {
                FromXMLToCSharpValidations(document);
                if (document != null && document.HasChildNodes) // The first thing to do is to check if our XML document isn't null or empty.
                {
                    var referenceNode = document.SelectSingleNode("//*[local-name()='references']"); // expression used to select a specific node.

                    if (referenceNode.ChildNodes.Count > 0) // At this first part of the translation, if we have one or more references, there are listed in a format.
                        for (var item = 0; item < referenceNode.ChildNodes.Count; item++)
                        {
                            references = TemplatesCollection.ReferenceTemplate;
                            references = String.Format(references, referenceNode.ChildNodes[item].Attributes.GetNamedItem("name").Value);
                            Console.WriteLine(references);
                        }

                    var namespaceNode = document.SelectSingleNode("//*[local-name()='namespace']"); // expression used to select a specific node.
                    if (namespaceNode != null)// after that, if there's a namespace, we have to add it and all it's classes, constructors, methods and variables.
                    {
                        namespaces = TemplatesCollection.NamespaceTemplate;
                        if (namespaceNode.HasChildNodes) // another validation to know if namespace has content.
                        {
                            for (var i = 0; i < namespaceNode.ChildNodes.Count; i++) // this procedure help us to know how many classes are and if each class has lines of code.    POR CADA CLASE VEMOS CUANTOS CONSTRUCTORES Y METODOS HAY ALMACENADOS.
                            {
                                // about structure, we have the constructors listed in here and after that, the methods & lines of code.
                                var constructorsList = namespaceNode.SelectNodes("//*[local-name()='constructor']"); // to verify how many constructors we have.
                                if (constructorsList.Count > 0 && constructorsList != null) // SI NUESTRA LISTA DE CONSTRUCTORES ES MAYOR A 0 Y NO ES NULA
                                {
                                    for (var a = 0; a < constructorsList.Count; a++)
                                    {
                                        if (string.IsNullOrEmpty(constructorsList[a].Attributes.GetNamedItem("arguments").Value)) // this means the constructor doesn't have parameters.
                                        {
                                            if (constructorsList[a].Attributes.GetNamedItem("lines").Value.Length > 0) // this represents when there are lines of code inside.
                                            {
                                                // PENDIENTE lineas de codigo.
                                            }
                                            else // we don't have parameters and lines of code.
                                            {
                                                constructorWithoutParameters = TemplatesCollection.ConstructorTemplate;
                                                constructorWithoutParameters = String.Format(constructorWithoutParameters, namespaceNode.ChildNodes[i].Attributes.GetNamedItem("modifiers").Value, namespaceNode.ChildNodes[i].Attributes.GetNamedItem("name").Value, "", "linea de codigo");
                                            }
                                        }
                                        else if (!string.IsNullOrEmpty(constructorsList[a].Attributes.GetNamedItem("arguments").Value) && constructorsList.Count > 1) // this validation is used when the constructor has parameters and empty constructor.
                                        {
                                            if (constructorsList[a].Attributes.GetNamedItem("lines").Value.Length > 0) // to search for lines of code.
                                            {
                                                // PENDIENTE lineas de codigo.
                                            }
                                            else // we don't have lines
                                            {
                                                constructorWithParameters = TemplatesCollection.ConstructorTemplate;
                                                constructorWithParameters = String.Format(constructorWithParameters, namespaceNode.ChildNodes[i].Attributes.GetNamedItem("modifiers").Value, namespaceNode.ChildNodes[i].Attributes.GetNamedItem("name").Value, constructorsList[a].Attributes.GetNamedItem("arguments").Value, "lineas de codigo");
                                            }
                                        }
                                    }
                                }

                                if (namespaceNode.ChildNodes[i].HasChildNodes)
                                {
                                    for (var list = 0; list < namespaceNode.ChildNodes[i].ChildNodes.Count; list++) // traversing the nodes contained in namespace
                                    {
                                        string name = namespaceNode.ChildNodes[i].ChildNodes[list].Name;
                                        XmlNode varNode = namespaceNode.ChildNodes[i].ChildNodes[list];
                                        switch (name) // used to write all the line codes or methods given
                                        {
                                            case "var":
                                                string variables = TemplatesCollection.VariableTemplate;
                                                if (varNode.Attributes.GetNamedItem("getterOrSetter").Value == "true") // this validation is read when the variable is type class
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
                                                break;

                                            case "methods":
                                                XmlNode methodNode = namespaceNode.ChildNodes[i].ChildNodes[list];
                                                string methods = TemplatesCollection.MethodTemplate;

                                                if (methodNode.Attributes.GetNamedItem("static").Value == "true") // that means the method is static.
                                                {
                                                    if (!string.IsNullOrEmpty(methodNode.Attributes.GetNamedItem("arguments").Value)) // that means a static method w/arguments.
                                                    {
                                                        if (methodNode.Attributes.GetNamedItem("returned").Value == "true") // means the method returns a value.
                                                        {
                                                            if (methodNode.HasChildNodes) // that means a static method w/arguments and lines of code.
                                                            {
                                                                methods = String.Format(methods, namespaceNode.ChildNodes[i].Attributes.GetNamedItem("modifiers").Value, "static", methodNode.Attributes.GetNamedItem("dataTypeReturn").Value, methodNode.Attributes.GetNamedItem("name").Value, methodNode.Attributes.GetNamedItem("arguments").Value, "lineas de codigo", "return");
                                                                listOfMethods += methods + "\n\r";
                                                            }
                                                            else // without lines of code
                                                            {
                                                                methods = String.Format(methods, namespaceNode.ChildNodes[i].Attributes.GetNamedItem("modifiers").Value, "static", methodNode.Attributes.GetNamedItem("dataTypeReturn").Value, methodNode.Attributes.GetNamedItem("name").Value, methodNode.Attributes.GetNamedItem("arguments").Value, "", "return");
                                                                listOfMethods += methods + "\n\r";
                                                            }
                                                        }
                                                        else
                                                        if (methodNode.HasChildNodes) // without returning a value, but w/lines of code
                                                        {
                                                            methods = String.Format(methods, namespaceNode.ChildNodes[i].Attributes.GetNamedItem("modifiers").Value, "static", methodNode.Attributes.GetNamedItem("dataTypeReturn").Value, methodNode.Attributes.GetNamedItem("name").Value, methodNode.Attributes.GetNamedItem("arguments").Value, "lineas de codigo", "");
                                                            listOfMethods += methods + "\n\r";
                                                        }
                                                        else // without returning a value even lines of code
                                                        {
                                                            methods = String.Format(methods, namespaceNode.ChildNodes[i].Attributes.GetNamedItem("modifiers").Value, "static", methodNode.Attributes.GetNamedItem("dataTypeReturn").Value, methodNode.Attributes.GetNamedItem("name").Value, methodNode.Attributes.GetNamedItem("arguments").Value, "", "");
                                                            listOfMethods += methods + "\n\r";
                                                        }
                                                    }
                                                    else // without arguments.
                                                    {
                                                        if (methodNode.Attributes.GetNamedItem("returned").Value == "true") // means the method returns a value.
                                                        {
                                                            if (methodNode.HasChildNodes) // that means a static method and lines of code.
                                                            {
                                                                methods = String.Format(methods, namespaceNode.ChildNodes[i].Attributes.GetNamedItem("modifiers").Value, "static", methodNode.Attributes.GetNamedItem("dataTypeReturn").Value, methodNode.Attributes.GetNamedItem("name").Value, "", "lineas de codigo", "return");
                                                                listOfMethods += methods + "\n\r";
                                                            }
                                                            else // without lines of code
                                                            {
                                                                methods = String.Format(methods, namespaceNode.ChildNodes[i].Attributes.GetNamedItem("modifiers").Value, "static", methodNode.Attributes.GetNamedItem("dataTypeReturn").Value, methodNode.Attributes.GetNamedItem("name").Value, "", "", "return");
                                                                listOfMethods += methods + "\n\r";
                                                            }
                                                        }
                                                        else
                                                        if (methodNode.HasChildNodes) // without returning a value, but w/lines of code
                                                        {
                                                            methods = String.Format(methods, namespaceNode.ChildNodes[i].Attributes.GetNamedItem("modifiers").Value, "static", methodNode.Attributes.GetNamedItem("dataTypeReturn").Value, methodNode.Attributes.GetNamedItem("name").Value, "", "lineas de codigo", "");
                                                            listOfMethods += methods + "\n\r";
                                                        }
                                                        else // without returning a value both lines of code
                                                        {
                                                            methods = String.Format(methods, namespaceNode.ChildNodes[i].Attributes.GetNamedItem("modifiers").Value, "static", methodNode.Attributes.GetNamedItem("dataTypeReturn").Value, methodNode.Attributes.GetNamedItem("name").Value, "", "", "");
                                                            listOfMethods += methods + "\n\r";
                                                        }
                                                    }
                                                }
                                                else // isn't static
                                                {
                                                    if (methodNode.Attributes.GetNamedItem("arguments").Value.Length > 0) // that means a non static method w/arguments.
                                                    {
                                                        if (methodNode.Attributes.GetNamedItem("returned").Value == "true") // means the method returns a value.
                                                        {
                                                            if (methodNode.HasChildNodes) // that means a static method w/arguments and lines of code.
                                                            {
                                                                methods = String.Format(methods, namespaceNode.ChildNodes[i].Attributes.GetNamedItem("modifiers").Value, "", methodNode.Attributes.GetNamedItem("dataTypeReturn").Value, methodNode.Attributes.GetNamedItem("name").Value, methodNode.Attributes.GetNamedItem("arguments").Value, "lineas de codigo", "return");
                                                                listOfMethods += methods + "\n\r";
                                                            }
                                                            else // without lines of code
                                                            {
                                                                methods = String.Format(methods, namespaceNode.ChildNodes[i].Attributes.GetNamedItem("modifiers").Value, "", methodNode.Attributes.GetNamedItem("dataTypeReturn").Value, methodNode.Attributes.GetNamedItem("name").Value, methodNode.Attributes.GetNamedItem("arguments").Value, "", "return");
                                                                listOfMethods += methods + "\n\r";
                                                            }
                                                        }
                                                        else
                                                        if (methodNode.HasChildNodes) // without returning a value, but w/lines of code
                                                        {
                                                            methods = String.Format(methods, namespaceNode.ChildNodes[i].Attributes.GetNamedItem("modifiers").Value, "", methodNode.Attributes.GetNamedItem("dataTypeReturn").Value, methodNode.Attributes.GetNamedItem("name").Value, methodNode.Attributes.GetNamedItem("arguments").Value, "lineas de codigo", "");
                                                            listOfMethods += methods + "\n\r";
                                                        }
                                                        else // without returning a value both lines of code
                                                        {
                                                            methods = String.Format(methods, namespaceNode.ChildNodes[i].Attributes.GetNamedItem("modifiers").Value, "", methodNode.Attributes.GetNamedItem("dataTypeReturn").Value, methodNode.Attributes.GetNamedItem("name").Value, methodNode.Attributes.GetNamedItem("arguments").Value, "", "");
                                                            listOfMethods += methods + "\n\r";
                                                        }
                                                    }
                                                    else // without arguments.
                                                    if (methodNode.Attributes.GetNamedItem("returned").Value == "true") // means the method returns a value.
                                                    {
                                                        if (methodNode.HasChildNodes) // that means a static method and lines of code.
                                                        {
                                                            methods = String.Format(methods, namespaceNode.ChildNodes[i].Attributes.GetNamedItem("modifiers").Value, "", methodNode.Attributes.GetNamedItem("dataTypeReturn").Value, methodNode.Attributes.GetNamedItem("name").Value, "", "lineas de codigo", "return");
                                                            listOfMethods += methods + "\n\r";
                                                        }
                                                        else // without lines of code
                                                        {
                                                            methods = String.Format(methods, namespaceNode.ChildNodes[i].Attributes.GetNamedItem("modifiers").Value, "", methodNode.Attributes.GetNamedItem("dataTypeReturn").Value, methodNode.Attributes.GetNamedItem("name").Value, "", "", "return");
                                                            listOfMethods += methods + "\n\r";
                                                        }
                                                    }
                                                    else
                                                    if (methodNode.HasChildNodes) // without returning a value, but w/lines of code
                                                    {
                                                        methods = String.Format(methods, namespaceNode.ChildNodes[i].Attributes.GetNamedItem("modifiers").Value, "", methodNode.Attributes.GetNamedItem("dataTypeReturn").Value, methodNode.Attributes.GetNamedItem("name").Value, "", "lineas de codigo", "");
                                                        listOfMethods += methods + "\n\r";
                                                    }
                                                    else // without returning a value both lines of code
                                                    {
                                                        methods = String.Format(methods, namespaceNode.ChildNodes[i].Attributes.GetNamedItem("modifiers").Value, "", methodNode.Attributes.GetNamedItem("dataTypeReturn").Value, methodNode.Attributes.GetNamedItem("name").Value, "", "", "");
                                                        listOfMethods += methods + "\n\r";
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                }

                                string varsAndMethods = listOfVariables + listOfMethods;
                                classes = TemplatesCollection.ClassTemplate;
                                classes = String.Format(classes, namespaceNode.ChildNodes[i].Attributes.GetNamedItem("modifiers").Value, namespaceNode.ChildNodes[i].Attributes.GetNamedItem("name").Value, constructorWithoutParameters + constructorWithParameters, varsAndMethods);
                            }
                            for (var k = 0; k < namespaceNode.ChildNodes.Count; k++)
                            {
                                namespaces = string.Format(namespaces, namespaceNode.Attributes.GetNamedItem("name").Value, classes);
                                Console.WriteLine(namespaces);
                            }
                        }
                    }
                }
                string CSharpContainerCode = references + namespaces;
                return CSharpContainerCode;  // the final code.
            }
            catch(Exception ex)
            {
                FromXMLToCSharpValidations(document);
                if (ex != null)
                    throw new TranslateException(ErrorAndExceptionsCatalog._712_Code, ErrorAndExceptionsCatalog._712_DocumentFormatNotCreated);
                else
                    throw new Exception("No se pudo atrapar el error" + ex);
            }
        }

        public static string ResultantArguments(List<csArgument> args)
        {
            var result = "";
            foreach (var item in args)
            {
                result += item.type + " " + item.value + ",";
            }
            result = result.TrimEnd(',');
            return result;
        }

        public static void XMLStructureValidations(csXML model)
        {
            if (string.IsNullOrEmpty(model.version))
                throw new TranslateException(ErrorAndExceptionsCatalog._707_Code, ErrorAndExceptionsCatalog._707_ModelVersionNotFound);
            else if (!model.version.StartsWith("1"))
                throw new TranslateException(ErrorAndExceptionsCatalog._702_Code, ErrorAndExceptionsCatalog._702_InvalidVersionModel);
            if (!model.encoding.StartsWith("U"))
                throw new TranslateException(ErrorAndExceptionsCatalog._703_Code, ErrorAndExceptionsCatalog._703_InvalidEncoding);
            if (string.IsNullOrEmpty(model.document.csNamespace.name))
                throw new TranslateException(ErrorAndExceptionsCatalog._704_Code, ErrorAndExceptionsCatalog._704_NotEnoughInformation);
            if (model.document.csNamespace.Classes.Count == 0 || model.document.references.Count == 0 || model.document.csNamespace.Classes.First().constructors.Count == 0 || model.document.csNamespace.Classes.First().lines.Count == 0)
                throw new TranslateException(ErrorAndExceptionsCatalog._706_Code, ErrorAndExceptionsCatalog._706_ChildNodesNotFound);
        }

        public static void FromXMLToCSharpValidations(XmlDocument document)
        {
            string[] specialDots = {"!", "@", "#", "$", "%", "^","*", "(", ")", "=" ,"+", "/" };
            var mthodLine =  document.SelectSingleNode("//*[local-name()='methods']");
            var constrLine = document.SelectSingleNode("//*[local-name()='constructor']");
            var classLine = document.SelectSingleNode("//*[local-name()='class']");

            if (string.IsNullOrEmpty(mthodLine.Attributes.GetNamedItem("name").Value))
                throw new TranslateException(ErrorAndExceptionsCatalog._711_Code, ErrorAndExceptionsCatalog._711_NameMethodNotFound);
            else if (mthodLine.Attributes.GetNamedItem("dataTypeReturn").Value.ToLower() != "void" && mthodLine.Attributes.GetNamedItem("returned").Value.ToLower() == "false")
                throw new TranslateException(ErrorAndExceptionsCatalog._715_Code, ErrorAndExceptionsCatalog._715__ValueIsNotReturnedInMethod);

            if (string.IsNullOrEmpty(constrLine.Attributes.GetNamedItem("class").Value))
                throw new TranslateException(ErrorAndExceptionsCatalog._713_Code, ErrorAndExceptionsCatalog._713_NameClassNotFound);
            
            if (string.IsNullOrEmpty(classLine.Attributes.GetNamedItem("name").Value))
                throw new TranslateException(ErrorAndExceptionsCatalog._709_Code, ErrorAndExceptionsCatalog._709_MainClassNameNotFound);

            foreach (var item in specialDots)
                if (mthodLine.Attributes.GetNamedItem("name").Value.Contains(item))
                    throw new TranslateException(ErrorAndExceptionsCatalog._708_Code, ErrorAndExceptionsCatalog._708_StrangeCharacterInMethodName);
                else if (constrLine.Attributes.GetNamedItem("class").Value.Contains(item))
                    throw new TranslateException(ErrorAndExceptionsCatalog._714_Code, ErrorAndExceptionsCatalog._714_StrangeCharacterInClassName);
                else if (classLine.Attributes.GetNamedItem("name").Value.Contains(item))
                    throw new TranslateException(ErrorAndExceptionsCatalog._710_Code, ErrorAndExceptionsCatalog._710_MainClassInvalidName);
        }
    }
}
