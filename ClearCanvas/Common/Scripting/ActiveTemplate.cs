﻿#region License

// Copyright (c) 2010, ClearCanvas Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
//
//    * Redistributions of source code must retain the above copyright notice, 
//      this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, 
//      this list of conditions and the following disclaimer in the documentation 
//      and/or other materials provided with the distribution.
//    * Neither the name of ClearCanvas Inc. nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without 
//      specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
// OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
// ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
// OF SUCH DAMAGE.

#endregion

using System;
using System.Collections;
using System.IO;
using System.Text;
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.Common.Scripting
{
    /// <summary>
    /// Represents an instance of an active template.
    /// </summary>
    /// <remarks>
	/// <para>
	/// An active template is equivalent to a classic ASP page: that is,
	/// it is a template that contains snippets of script code that can call back into the context in which the script
	/// is being evaluated.  Currently only the Jscript language is supported.
	/// </para>
	/// <para>
	/// Initialize the template context via the constructor.  The template
	/// can then be evaluated within a given context by calling one of the <b>Evaluate</b> methods.
	/// </para>
	/// </remarks>
    public class ActiveTemplate
    {
        private string _inversion;
        private IExecutableScript _script;

        /// <summary>
        /// Constructs a template from the specified content.
        /// </summary>
        public ActiveTemplate(TextReader content)
        {
            _inversion = ComputeInversion(content);
        }

        /// <summary>
        /// Overload that allows the output of the template evaluation to be written directly to a <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="context">A dictionary of objects to pass into the script.</param>
        /// <param name="output">A text writer to which the output should be written.</param>
        public void Evaluate(IDictionary context, TextWriter output)
        {
            try
            {
                // add a variable for the output stream to the context
                context["__out__"] = output;

                // create executable script if not created
                if (_script == null)
                {
                    string[] variables = CollectionUtils.Map<string, string>(context.Keys, delegate(string s) { return s; }).ToArray();
                    _script = ScriptEngineFactory.CreateEngine("jscript").CreateScript(_inversion, variables);
                }

                _script.Run(context);

            }
            catch (Exception e)
            {
				throw new ActiveTemplateException(SR.ExceptionTemplateEvaluation, e);
            }
        }

        /// <summary>
        /// Evaluates this template in the specified context.
        /// </summary>
        /// <remarks>
		/// The context parameter allows a set of named objects to be passed into 
		/// the scripting environment.  Within the scripting environment
		/// these objects can be referenced directly as properties of "this".  For example,
		/// <code>
		///     Hashtable scriptingContext = new Hashtable();
		///     scriptingContext["Patient"] = patient;  // add a reference to an existing instance of a patient object
		/// 
		///     Template template = new Template(...);
		///     template.Evaluate(scriptingContext);
		/// 
		///     // now, in the template, the script would access the object as shown
		///     &lt;%= this.Patient.Name %&gt;
		/// </code>
		/// </remarks>
        /// <param name="context">A dictionary of objects to pass into the script.</param>
        /// <returns>The result of the template evaluation as a string.</returns>
        public string Evaluate(IDictionary context)
        {
            StringWriter output = new StringWriter();
            Evaluate(context, output);
            return output.ToString();
        }

        /// <summary>
        /// Inverts the template content, returning a Jscript script that, when evaluated, will return
        /// the full result of the template.
        /// </summary>
        private string ComputeInversion(TextReader template)
        {
            StringBuilder inversion = new StringBuilder();
            string line = null;
            bool inCode = false;    // keep track of whether we are inside a <% %> or not

            // process each line of the template
            while ((line = template.ReadLine()) != null)
            {
                inCode = ProcessLine(line, inCode, inversion);
                
                // preserve the formatting of the original template by writing new lines appropriately
                if(!inCode)
                    inversion.AppendLine("this.__out__.WriteLine();");
            }

            return inversion.ToString();
        }

        private static bool ProcessLine(string line, bool inCode, StringBuilder inversion)
        {
            inCode = !inCode;   // just make the loop work correctly

            // break the line up into code/non-code parts
            string[] parts = line.Split(new string[] { "<%", "%>" }, StringSplitOptions.None);
            foreach (string part in parts)
            {
                inCode = !inCode;
                if (inCode)
                {
                    if (part.StartsWith("="))
                    {
                        inversion.AppendLine(string.Format("this.__out__.Write({0});", part.Substring(1)));
                    }
                    else
                    {
                        inversion.Append(part);
                        inversion.AppendLine();
                    }
                }
                else
                {
                    string escaped = part.Replace("\"", "\\\"");  // escape any " characters
                    inversion.AppendLine(string.Format("this.__out__.Write(\"{0}\");", escaped));
                }
            }
            return inCode;
        }
    }
}
