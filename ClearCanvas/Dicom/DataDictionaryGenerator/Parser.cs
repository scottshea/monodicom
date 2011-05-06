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
using System.Security;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections;
using System.Globalization;


namespace ClearCanvas.Dicom.DataDictionaryGenerator
{
    public struct Tag
    {
        public uint nTag;
        public String tag;
        public String name;
		public String unEscapedName;
		public String vr;
        public String vm;
        public String retired;
        public String varName;
    }

    public struct SopClass
    {
        public String name;
        public String uid;
        public String type;
        public String varName;
    }

    public class Parser
    {
        public SortedList<uint, Tag> _tags = new SortedList<uint, Tag>();
        public SortedList _sopClasses = new SortedList();
        public SortedList _metaSopClasses = new SortedList();
        public SortedList _tranferSyntaxes = new SortedList();

        public Parser()
        {
        }

        /// <summary>
        /// Formats to pascal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string FormatToPascal(string value)
        {
            if (value == null)
                return null;

            StringBuilder sb = new StringBuilder();
            bool lastCharWasSpace = false;
            foreach (char c in value)
            {
                if (c.ToString() == " ")
                    lastCharWasSpace = true;
                else if (lastCharWasSpace || sb.Length == 0)
                    sb.Append(c.ToString().ToUpperInvariant());
                else
                    sb.Append(c.ToString().ToLowerInvariant());
            }
            return sb.ToString();
        }

        public static string CreateVariableName(string input)
        {
            // Now create the variable name
            char[] charSeparators = new char[] {'(', ')', ',', ' ', '\'', '�', '�', '-', '/', '&', '[', ']', '@'};

            // just remove apostrophes so casing is correct
            string tempString = input.Replace("’", ""); 
            tempString = tempString.Replace("'", "");
            tempString = tempString.Replace("(", "");
            tempString = tempString.Replace(")", "");
            tempString = tempString.Replace("–", "");

            String[] nodes = tempString.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);

            string output = "";
            foreach (String node in nodes)
                output += FormatToPascal(node);

            return output;
        }

        public static void CreateNames(ref Tag thisTag)
        {
            thisTag.varName = CreateVariableName(thisTag.name);

            // Handling leading digits in names
            if (thisTag.varName.Length > 0 && char.IsDigit(thisTag.varName[0]))
                thisTag.varName = "Tag" + thisTag.varName;

            if (thisTag.retired != null 
             && thisTag.retired.Equals("RET") 
             && !thisTag.varName.EndsWith("Retired"))
                thisTag.varName += "Retired";
        	thisTag.name = thisTag.name.Replace("’", "'");
        	thisTag.unEscapedName = thisTag.name;
            thisTag.name = SecurityElement.Escape(thisTag.name);
        }

        public void ParseFile(String filename)
        {
            TextReader tReader = new StreamReader(filename);
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.CheckCharacters = false;
            settings.ValidationType = ValidationType.None;
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.IgnoreProcessingInstructions = true;
            XmlReader reader = XmlReader.Create(tReader, settings);
            String[] columnArray = new String[10];
            int colCount = -1;
            bool isFirst = true;
            bool isTag = true;
            bool isUid = true;
            try
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        isFirst = true;
                        if (reader.Name == "w:tbl")
                        {
                            while (reader.Read())
                            {
                                if (reader.IsStartElement())
                                {
                                    if (reader.Name == "w:tc")
                                    {
                                        colCount++;
                                    }
                                    else if (reader.Name == "w:t")
                                    {
                                        String val = reader.ReadString();
                                        //if (val != "(")
                                        if (columnArray[colCount] == null)
                                            columnArray[colCount] = val;
                                        else
                                            columnArray[colCount] += val;
                                    }
                                }
                                if ((reader.NodeType == XmlNodeType.EndElement)
                                    && (reader.Name == "w:tr"))
                                {
                                    if (isFirst)
                                    {
                                        if (columnArray[0] == "Tag")
                                        {
                                            isTag = true;
                                            isUid = false;
                                        }
                                        else
                                        {
                                            isTag = false;
                                            isUid = true;
                                        }

                                        isFirst = false;
                                    }
                                    else
                                    {
                                        if (isTag)
                                        {
                                            Tag thisTag = new Tag();
                                            if (columnArray[0] != null && columnArray[0] != "Tag")
                                            {
                                                thisTag.tag = columnArray[0];
                                                thisTag.name = columnArray[1];
                                                if (columnArray[2] != null)
                                                    thisTag.vr = columnArray[2].Trim();
                                                if (columnArray[3] != null)
                                                    thisTag.vm = columnArray[3].Trim();
                                                thisTag.retired = columnArray[4];

                                                // Handle repeating groups
                                                if (thisTag.tag[3] == 'x')
                                                    thisTag.tag = thisTag.tag.Replace("xx", "00");

                                                char[] charSeparators = new char[] { '(', ')', ',', ' ' };

                                                String[] nodes = thisTag.tag.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
                                                UInt32 group, element; 
                                                if (UInt32.TryParse(nodes[0],NumberStyles.HexNumber,null, out group)
                                                 && UInt32.TryParse(nodes[1], NumberStyles.HexNumber,null, out element)
                                                    && thisTag.name != null)
                                                {
                                                    thisTag.nTag = element | group << 16;

                                                    CreateNames(ref thisTag);

                                                    if (!thisTag.varName.Equals("Item")
                                                     && !thisTag.varName.Equals("ItemDelimitationItem")
                                                     && !thisTag.varName.Equals("SequenceDelimitationItem")
                                                     && !thisTag.varName.Equals("GroupLength"))

                                                        _tags.Add(thisTag.nTag, thisTag);
                                                }
                                            }
                                        }
                                        else if (isUid)
                                        {

                                            if (columnArray[0] != null)
                                            {
                                                SopClass thisUid = new SopClass();

                                                thisUid.uid = columnArray[0];
                                                thisUid.name = columnArray[1];
                                                thisUid.type = columnArray[2];

                                                thisUid.varName = CreateVariableName(thisUid.name);

                                                // Take out the invalid chars in the name, and replace with escape characters.
                                                thisUid.name = SecurityElement.Escape(thisUid.name);

                                                if (thisUid.type == "SOP Class")
                                                {
                                                    // Handling leading digits in names
                                                    if (thisUid.varName.Length > 0 && char.IsDigit(thisUid.varName[0]))
                                                        thisUid.varName = "Sop" + thisUid.varName;
                                                    _sopClasses.Add(thisUid.name, thisUid);
                                                }
                                                else if (thisUid.type == "Transfer Syntax")
                                                {
                                                    int index = thisUid.varName.IndexOf(':');
                                                    if (index != -1)
                                                        thisUid.varName = thisUid.varName.Remove(index);

                                                    _tranferSyntaxes.Add(thisUid.name, thisUid);
                                                }
                                                else if (thisUid.type == "Meta SOP Class")
                                                {
                                                    // Handling leading digits in names
                                                    if (thisUid.varName.Length > 0 && char.IsDigit(thisUid.varName[0]))
                                                        thisUid.varName = "Sop" + thisUid.varName;
                                                    _metaSopClasses.Add(thisUid.name, thisUid);
                                                }
                                            }
                                        }
                                    }

                                    colCount = -1;
                                    for (int i = 0; i < columnArray.Length; i++)
                                        columnArray[i] = null;
                                }

                                if ((reader.NodeType == XmlNodeType.EndElement)
                                 && (reader.Name == "w:tbl"))
                                    break; // end of table
                            }
                        }
                    }
                }
            }
            catch (XmlException)
            {

            }
        }
    }
}
