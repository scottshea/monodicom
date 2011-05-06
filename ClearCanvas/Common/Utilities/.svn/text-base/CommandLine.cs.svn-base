#region License

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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ClearCanvas.Common.Utilities
{
    /// <summary>
    /// Parses command line arguments.  May be used directly, or may be subclassed and used in conjunction with
    /// <see cref="CommandLineParameterAttribute"/> to have strongly-typed resolution.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Parses an array of command line arguments. Each argument is treated as either a named parameter,
    /// a positional parameter, or a switch, depending on its format.  Arguments may be quoted to allow spaces -
    /// the .NET runtime removes the quotes prior to providing the arguments to application code.
    /// </para>
    /// <para>
    /// A named parameter has the form 
    ///     /param:value
    ///     -param:value
    ///     /param=value
    ///     -param=value 
    /// 
    /// There must not be a space between the : or = and the value.  Named parameters are stored in the
    /// <see cref="Named"/> property.
    /// </para>
    /// <para>
    /// A switch has the form
    ///     /switch[+-]
    ///     -switch[+-]
    /// 
    /// The trailing + or - is optional, and if omitted, the boolean state of the switch is simply toggled.
    /// Switch states are stored in the <see cref="Switches"/> property.
    /// </para>
    /// <para>
    /// A parameter that does not match either the named or switch form is interpreted as a positional parameter.
    /// Positional parameters are stored in the <see cref="Positional"/> property, in the order in which they occur.
    /// </para>
    /// </remarks>
    public class CommandLine
    {
        private readonly Dictionary<string, string> _namedArgs = new Dictionary<string, string>();
        private readonly List<string> _positionalArgs = new List<string>();
        private readonly Dictionary<string, bool> _switches = new Dictionary<string, bool>();

        private readonly Regex _namedArgRegex = new Regex(@"^[\/\-](\w+)[:=](.*)$", RegexOptions.Compiled);
        private readonly Regex _boolSwitchRegex = new Regex(@"^[\/\-](\w+)([\+\-]?)$", RegexOptions.Compiled);

        /// <summary>
        /// Constructor.
        /// </summary>
        public CommandLine()
        {
        }

        /// <summary>
        /// Constructs an instance of this class, parsing the specified argument list.
        /// </summary>
        /// <param name="args"></param>
        public CommandLine(string[] args)
        {
            Parse(args);
        }

        /// <summary>
        /// Gets the set of named arguments.
        /// </summary>
        public IDictionary<string, string> Named
        {
            get { return _namedArgs; }
        }

        /// <summary>
        /// Gets the set of positional arguments.
        /// </summary>
        public IList<string> Positional
        {
            get { return _positionalArgs; }
        }

        /// <summary>
        /// Gets the set of switches.
        /// </summary>
        public IDictionary<string, bool> Switches
        {
            get { return _switches; }
        }

        /// <summary>
        /// Parses the specified argument list, using the results to populate the contents of this instance.
        /// </summary>
        /// <param name="args"></param>
        /// <exception cref="CommandLineException">
        /// Thrown if any error occurs attempting to parse the arguments, or if required arguments 
        /// are missing.
        /// </exception>
        public void Parse(string[] args)
        {
            ProcessArgs(args);
            ProcessAttributes();
        }

        /// <summary>
        /// Generates a usage message, based on meta-data supplied by <see cref="CommandLineParameterAttribute"/>s declared
        /// on members of the subclass.
        /// </summary>
        /// <param name="writer"></param>
        public void PrintUsage(TextWriter writer)
        {
            List<string> positionals = new List<string>();
            List<string> options = new List<string>();

            foreach (IObjectMemberContext context in WalkDataMembers())
            {
                CommandLineParameterAttribute a = AttributeUtils.GetAttribute<CommandLineParameterAttribute>(context.Member);
                if (a.Position > -1)
                {
                    positionals.Add(string.Format(a.Required ? "{0}" : "[{0}]", a.DisplayName));
                }
                else
                {
                    string format = context.MemberType == typeof(bool) ? "/{0}[+|-]\t{1}" : "/{0}:(value)\t{1}";
                    string s = string.Format(format, a.Key, a.Usage);
                    if (!string.IsNullOrEmpty(a.KeyShortForm))
                        s += string.Format(" (Short form: /{0})", a.KeyShortForm);
                    options.Add(s);
                }
            }

            writer.Write("Usage: ");
            if (options.Count > 0)
                writer.Write("[options] ");

            positionals.ForEach(delegate(string s) { writer.Write("{0} ", s); });
            writer.WriteLine();

            if (options.Count > 0)
            {
                writer.WriteLine("Options:");
                options.ForEach(delegate(string s) { writer.WriteLine(s); });
            }
        }

        #region Private Helpers

        private void ProcessArgs(string[] args)
        {
            foreach (string arg in args)
            {
                if (TryParseNamedArg(arg))
                {
                }
                else if (TryParseBoolSwitch(arg))
                {
                }
                else
                {
                    _positionalArgs.Add(arg);
                }
            }
        }

        private void ProcessAttributes()
        {
            foreach (IObjectMemberContext context in WalkDataMembers())
            {
                CommandLineParameterAttribute a = AttributeUtils.GetAttribute<CommandLineParameterAttribute>(context.Member);
                if (a != null)
                {
                    if (a.Position > -1)
                    {
                        // treat as positional
                        ValidateMemberType(context.Member.Name, new Type[] { typeof(string) }, context.MemberType);
                        if (_positionalArgs.Count > a.Position)
                            context.MemberValue = _positionalArgs[a.Position];
                        else
                        {
                            if (a.Required)
                                throw new CommandLineException(string.Format("Missing required command line argument <{0}>", a.DisplayName));
                        }
                    }
                    else
                    {
                        // treat as named/switch
                        if (context.MemberType == typeof(bool))
                        {
                            ValidateMemberType(context.Member.Name, new Type[] { typeof(bool) }, context.MemberType);
                            SetMemberValue(a, context, _switches);
                        }
                        else
                        {
                            ValidateMemberType(context.Member.Name, new Type[] { typeof(string), typeof(int), typeof(Enum) }, context.MemberType);
                            SetMemberValue(a, context, _namedArgs);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets the value of a member property/field from the specified source dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="context"></param>
        /// <param name="source"></param>
        private void SetMemberValue<T>(CommandLineParameterAttribute a, IObjectMemberContext context, IDictionary<string, T> source)
        {
            string[] keys = new string[] { a.Key, a.KeyShortForm ?? "" };
            // T will be either bool or string, depending on whether source is "Switches" or "Named"
            foreach (string key in keys)
            {
                if (source.ContainsKey(key))
                {
                    T value = source[key];
                    if (context.MemberType == typeof(T))
                        context.MemberValue = value;
                    else if (context.MemberType.IsEnum)
                    {
                        try
                        {
                            context.MemberValue = Enum.Parse(context.MemberType, value.ToString(), true);
                        }
                        catch (Exception)
                        {
                            throw new CommandLineException(
                                string.Format("Invalid option for named command line argument: {0} ({1})",
                                value,
                                StringUtilities.Combine(keys, ", ")));
                        }
                    }
                    else if (context.MemberType == typeof(int))
                    {
                        context.MemberValue = int.Parse(value.ToString());
                    }
                    return;
                }
            }
            if (a.Required)
                throw new CommandLineException(
                    string.Format("Missing required command line argument <{0}> ({1})",
                    a.Usage,
                    StringUtilities.Combine(keys, ", ")));
        }

        private void ValidateMemberType(string memberName, Type[] allowable, Type actual)
        {
            if (!CollectionUtils.Contains(allowable, delegate(Type t) { return t.IsAssignableFrom(actual); }))
                throw new InvalidOperationException(
                    string.Format("Property/field {0} cannot be of type {1} - must be of type {2}",
                        memberName,
                        actual,
                        StringUtilities.Combine(allowable, ", ")));
        }

        private bool TryParseNamedArg(string arg)
        {
            Match match = _namedArgRegex.Match(arg);
            if (match.Success)
            {
                string key = match.Groups[1].Value;
                string value = match.Groups.Count > 2 ? match.Groups[2].Value : "";
                _namedArgs[key] = value;
                return true;
            }
            return false;
        }

        private bool TryParseBoolSwitch(string arg)
        {
            Match match = _boolSwitchRegex.Match(arg);
            if (match.Success)
            {
                string key = match.Groups[1].Value;
                if (!_switches.ContainsKey(key))
                    _switches.Add(key, false);

                // check for a + or -, indicating true or false
                string value = match.Groups.Count > 2 ? match.Groups[2].Value : "";
                if (value == "+")
                    _switches[key] = true;
                else if (value == "-")
                    _switches[key] = false;
                else
                    // if no value passed explicitly, then toggle the state of the switch
                    _switches[key] = !_switches[key];

                return true;
            }
            return false;
        }

        private IEnumerable<IObjectMemberContext> WalkDataMembers()
        {
            ObjectWalker walker = new ObjectWalker(
                delegate(MemberInfo member)
                {
                    return AttributeUtils.HasAttribute<CommandLineParameterAttribute>(member);
                });
            walker.IncludeNonPublicFields = true;
            walker.IncludeNonPublicProperties = true;
            return walker.Walk(this);
        }

        #endregion
    }
}
