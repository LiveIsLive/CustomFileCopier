﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本: 17.0.0.0
//  
//     对此文件的更改可能导致不正确的行为，如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
// ------------------------------------------------------------------------------
namespace ColdShineSoft.CustomFileCopier.Models
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "G:\WindowsApplications\CustomFileCopier\Models\FileFilterTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class FileFilterTemplate : FileFilterTemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("\r\nusing System.Linq;\r\npublic class CustomFileFilter:ColdShineSoft.CustomFileCopie" +
                    "r.Models.FileFilter\r\n{\r\n\t");
            
            #line 10 "G:\WindowsApplications\CustomFileCopier\Models\FileFilterTemplate.tt"
 foreach(IProperty property in this.Job.Conditions.Select(c=>c.Property).Distinct()){ 
            
            #line default
            #line hidden
            this.Write("\t\tprotected readonly ");
            
            #line 11 "G:\WindowsApplications\CustomFileCopier\Models\FileFilterTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ExtentionMethods.GetGenericTypeFullName(property)));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 11 "G:\WindowsApplications\CustomFileCopier\Models\FileFilterTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.PropertyVariableName));
            
            #line default
            #line hidden
            this.Write(";\r\n\t\tprotected ");
            
            #line 12 "G:\WindowsApplications\CustomFileCopier\Models\FileFilterTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Type.FullName));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 12 "G:\WindowsApplications\CustomFileCopier\Models\FileFilterTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.ValueVariableName));
            
            #line default
            #line hidden
            this.Write(";\r\n\t");
            
            #line 13 "G:\WindowsApplications\CustomFileCopier\Models\FileFilterTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\r\n\t");
            
            #line 15 "G:\WindowsApplications\CustomFileCopier\Models\FileFilterTemplate.tt"
 foreach(IOperator o in this.Job.Conditions.Select(c=>c.Operator).Distinct()){ 
            
            #line default
            #line hidden
            this.Write("\t\tprotected readonly ");
            
            #line 16 "G:\WindowsApplications\CustomFileCopier\Models\FileFilterTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ExtentionMethods.GetGenericTypeFullName(o)));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 16 "G:\WindowsApplications\CustomFileCopier\Models\FileFilterTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(o.VariableName));
            
            #line default
            #line hidden
            this.Write(";\r\n\t");
            
            #line 17 "G:\WindowsApplications\CustomFileCopier\Models\FileFilterTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\r\n\t");
            
            #line 19 "G:\WindowsApplications\CustomFileCopier\Models\FileFilterTemplate.tt"
 foreach(Condition condition in this.Job.Conditions){ 
            
            #line default
            #line hidden
            this.Write("\t\tprotected readonly ");
            
            #line 20 "G:\WindowsApplications\CustomFileCopier\Models\FileFilterTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(condition.ValueType.FullName));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 20 "G:\WindowsApplications\CustomFileCopier\Models\FileFilterTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(condition.VariableName));
            
            #line default
            #line hidden
            this.Write(";\r\n\t");
            
            #line 21 "G:\WindowsApplications\CustomFileCopier\Models\FileFilterTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\r\n\tpublic CustomFileFilter(System.Collections.Generic.IEnumerable<ColdShineSoft.C" +
                    "ustomFileCopier.Models.Condition> conditions)\r\n\t{\r\n\t\tColdShineSoft.CustomFileCop" +
                    "ier.Models.Condition condition;\r\n\t\t");
            
            #line 26 "G:\WindowsApplications\CustomFileCopier\Models\FileFilterTemplate.tt"
 for(int i=0;i<this.Job.Conditions.Count;i++)
		{
			Condition condition=this.Job.Conditions[i];
		
            
            #line default
            #line hidden
            this.Write("\t\t\tcondition=conditions.ElementAt(");
            
            #line 30 "G:\WindowsApplications\CustomFileCopier\Models\FileFilterTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(i));
            
            #line default
            #line hidden
            this.Write(");\r\n\t\t\tthis.");
            
            #line 31 "G:\WindowsApplications\CustomFileCopier\Models\FileFilterTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(condition.Property.PropertyVariableName));
            
            #line default
            #line hidden
            this.Write("=(");
            
            #line 31 "G:\WindowsApplications\CustomFileCopier\Models\FileFilterTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ExtentionMethods.GetGenericTypeFullName(condition.Property)));
            
            #line default
            #line hidden
            this.Write(")condition.Property;\r\n\t\t\tthis.");
            
            #line 32 "G:\WindowsApplications\CustomFileCopier\Models\FileFilterTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(condition.Operator.VariableName));
            
            #line default
            #line hidden
            this.Write("=(");
            
            #line 32 "G:\WindowsApplications\CustomFileCopier\Models\FileFilterTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ExtentionMethods.GetGenericTypeFullName(condition.Operator)));
            
            #line default
            #line hidden
            this.Write(")condition.Operator;\r\n\t\t\tthis.");
            
            #line 33 "G:\WindowsApplications\CustomFileCopier\Models\FileFilterTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(condition.VariableName));
            
            #line default
            #line hidden
            this.Write("=(");
            
            #line 33 "G:\WindowsApplications\CustomFileCopier\Models\FileFilterTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(condition.ValueType.FullName));
            
            #line default
            #line hidden
            this.Write(")condition.Value;\r\n\t\t");
            
            #line 34 "G:\WindowsApplications\CustomFileCopier\Models\FileFilterTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write(@"	}

	public override System.Collections.Generic.IEnumerable<System.IO.FileInfo> GetFiles(string sourceDirectoryPath)
	{
		foreach(System.IO.FileInfo file in System.IO.Directory.EnumerateFiles(sourceDirectoryPath, ""*"", System.IO.SearchOption.AllDirectories).Select(f => new System.IO.FileInfo(f)))
		{
			");
            
            #line 41 "G:\WindowsApplications\CustomFileCopier\Models\FileFilterTemplate.tt"
 foreach(IProperty property in this.Job.Conditions.Select(c=>c.Property).Distinct()){ 
            
            #line default
            #line hidden
            this.Write("\t\t\t\tthis.");
            
            #line 42 "G:\WindowsApplications\CustomFileCopier\Models\FileFilterTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.ValueVariableName));
            
            #line default
            #line hidden
            this.Write("=this.");
            
            #line 42 "G:\WindowsApplications\CustomFileCopier\Models\FileFilterTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.PropertyVariableName));
            
            #line default
            #line hidden
            this.Write(".GetValue(file);\r\n\t\t\t");
            
            #line 43 "G:\WindowsApplications\CustomFileCopier\Models\FileFilterTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\t\t\tif(");
            
            #line 44 "G:\WindowsApplications\CustomFileCopier\Models\FileFilterTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.Job.CombinedExpression));
            
            #line default
            #line hidden
            this.Write(")\r\n\t\t\t\tyield return file;\r\n\t\t}\r\n\t}\r\n}");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public class FileFilterTemplateBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
