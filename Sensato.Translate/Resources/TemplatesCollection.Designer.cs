﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sensato.Translate.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class TemplatesCollection {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal TemplatesCollection() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Sensato.Translate.Resources.TemplatesCollection", typeof(TemplatesCollection).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to    {0} class {1}
        ///   {{
        ///   {2}
        ///   {3}
        ///   }}.
        /// </summary>
        public static string ClassTemplate {
            get {
                return ResourceManager.GetString("ClassTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} {1}({2})
        ///{{
        ///	{3}
        ///}}
        ///.
        /// </summary>
        public static string ConstructorTemplate {
            get {
                return ResourceManager.GetString("ConstructorTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .
        /// </summary>
        public static string MethodTemplate {
            get {
                return ResourceManager.GetString("MethodTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///namespace {0}
        ///{{
        ///{1}
        ///}}.
        /// </summary>
        public static string NamespaceTemplate {
            get {
                return ResourceManager.GetString("NamespaceTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to using {0}.
        /// </summary>
        public static string ReferenceTemplate {
            get {
                return ResourceManager.GetString("ReferenceTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 	{0} {1} {2} {3} {4}.
        /// </summary>
        public static string VariableTemplate {
            get {
                return ResourceManager.GetString("VariableTemplate", resourceCulture);
            }
        }
    }
}
