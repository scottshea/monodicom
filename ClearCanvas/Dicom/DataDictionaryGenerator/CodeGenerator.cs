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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ClearCanvas.Dicom.DataDictionaryGenerator
{
    public class CodeGenerator
    {
    	readonly SortedList<uint, Tag> _tagList = null;
    	readonly SortedList _tSyntaxList = null;
    	readonly SortedList _sopList = null;
    	readonly SortedList _metaSopList = null;
    	readonly XmlDocument _transferSyntaxDoc = null;

        public CodeGenerator(SortedList<uint, Tag> tags, SortedList tSyntax, SortedList sops, SortedList metaSops, XmlDocument transferSyntaxDoc)
        {
            _tagList = tags;
            _tSyntaxList = tSyntax;
            _sopList = sops;
            _metaSopList = metaSops;
            _transferSyntaxDoc = transferSyntaxDoc;
        }

        private void WriterHeader(StreamWriter writer)
        {
            writer.WriteLine("#region License");
            writer.WriteLine("");
            writer.WriteLine("// Copyright (c) 2010, ClearCanvas Inc.");
            writer.WriteLine("// All rights reserved.");
            writer.WriteLine("//");
            writer.WriteLine("// Redistribution and use in source and binary forms, with or without modification, ");
            writer.WriteLine("// are permitted provided that the following conditions are met:");
            writer.WriteLine("//");
            writer.WriteLine("//    * Redistributions of source code must retain the above copyright notice, ");
            writer.WriteLine("//      this list of conditions and the following disclaimer.");
            writer.WriteLine("//    * Redistributions in binary form must reproduce the above copyright notice, ");
            writer.WriteLine("//      this list of conditions and the following disclaimer in the documentation ");
            writer.WriteLine("//      and/or other materials provided with the distribution.");
            writer.WriteLine("//    * Neither the name of ClearCanvas Inc. nor the names of its contributors ");
            writer.WriteLine("//      may be used to endorse or promote products derived from this software without ");
            writer.WriteLine("//      specific prior written permission.");
            writer.WriteLine("//");
            writer.WriteLine("// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS \"AS IS\" ");
            writer.WriteLine("// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, ");
            writer.WriteLine("// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR ");
            writer.WriteLine("// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR ");
            writer.WriteLine("// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, ");
            writer.WriteLine("// OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE ");
            writer.WriteLine("// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) ");
            writer.WriteLine("// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, ");
            writer.WriteLine("// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ");
            writer.WriteLine("// ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY ");
            writer.WriteLine("// OF SUCH DAMAGE.");
            writer.WriteLine("");
            writer.WriteLine("#endregion");
            writer.WriteLine("");
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine("");
            writer.WriteLine("// This file is auto-generated by the ClearCanvas.Dicom.DataDictionaryGenerator project.");
            writer.WriteLine("");
            writer.WriteLine("namespace ClearCanvas.Dicom");
            writer.WriteLine("{");
        }

        private void WriterFooter(StreamWriter writer)
        {
            writer.WriteLine("}");
        }

        /// <summary>
        /// Create the DicomTags.cs file.
        /// </summary>
        /// <param name="tagFile"></param>
        public void WriteTags(String tagFile)
        {
            StreamWriter writer = new StreamWriter(tagFile);

            WriterHeader(writer);

            writer.WriteLine("    /// <summary>");
            writer.WriteLine("    /// This structure contains defines for all DICOM tags.");
            writer.WriteLine("    /// </summary>");
            writer.WriteLine("    public struct DicomTags");
            writer.WriteLine("    {");

            IEnumerator<Tag> iter = _tagList.Values.GetEnumerator();

            while (iter.MoveNext())
            {
                Tag tag = iter.Current;

                writer.WriteLine("        /// <summary>");
                writer.WriteLine("        /// <para>" + tag.tag + " " + tag.name + "</para>");
                writer.WriteLine("        /// <para> VR: " + tag.vr + " VM:" + tag.vm + "</para>");
                if (tag.retired != null && tag.retired.Equals("RET"))
                    writer.WriteLine("        /// <para>This tag has been retired.</para>");
                writer.WriteLine("        /// </summary>");
                writer.WriteLine("        public const uint " + tag.varName + " = " + tag.nTag + ";");
            }

            writer.WriteLine("    }");
            WriterFooter(writer);

            writer.Close();
        }

        /// <summary>
        /// Create the DicomTagDictionary.cs file.
        /// </summary>
        /// <param name="tagFile"></param>
        public void WriteTagDictionary(String tagFile)
        {
            StreamWriter writer = new StreamWriter(tagFile);

            WriterHeader(writer);

            writer.WriteLine("    /// <summary>");
            writer.WriteLine("    /// This class contains a dictionary of all DICOM tags.");
            writer.WriteLine("    /// </summary>");
            writer.WriteLine("    /// <remarks>");
            writer.WriteLine("    /// <para>This class is the Flyweight Factor for the DicomTag Flyweight class as defined in the Flyweight pattern.</para>");
            writer.WriteLine("    /// </remarks>");
            writer.WriteLine("    public class DicomTagDictionary");
            writer.WriteLine("    {");
            writer.WriteLine("        // Internal members");
            writer.WriteLine("        private static Dictionary<uint,DicomTag> _tags = new Dictionary<uint,DicomTag>();");
            writer.WriteLine("        private static Dictionary<string,DicomTag> _tagNames = new Dictionary<string,DicomTag>();");
            writer.WriteLine("");
            writer.WriteLine("        // Static constructor");
            writer.WriteLine("        static DicomTagDictionary()");
            writer.WriteLine("        {");
            writer.WriteLine("            InitStandardTags();");
            writer.WriteLine("        }");
            writer.WriteLine("");
            writer.WriteLine("        /// <summary>");
            writer.WriteLine("        /// Retrieve a strongly typed list containing all DICOM tags.");
            writer.WriteLine("        /// </summary>");
            writer.WriteLine("        /// <returns>A <see cref=\"System.Collections.Generic.List\"/>.</returns>");
            writer.WriteLine("        public static IList<DicomTag> GetDicomTagList()");
            writer.WriteLine("        {");
            writer.WriteLine("            return new List<DicomTag>(_tags.Values);");
            writer.WriteLine("        }");
            writer.WriteLine("");
            writer.WriteLine("        /// <summary>");
            writer.WriteLine("        /// Method used to retrieve DicomTag instances for specific DICOM attributes.");
            writer.WriteLine("        /// </summary>");
            writer.WriteLine("        /// <param name=\"group\"></param>");
            writer.WriteLine("        /// <param name=\"element\"></param>");
            writer.WriteLine("        /// <returns>A DicomTag instance, if the tag exists, or null if it doesn't.</returns>");
            writer.WriteLine("        public static DicomTag GetDicomTag(ushort group, ushort element)");
            writer.WriteLine("        {");
            writer.WriteLine("            return GetDicomTag((uint)group << 16 | (uint)element);");
            writer.WriteLine("        }");
            writer.WriteLine("");
            writer.WriteLine("        /// <summary>");
            writer.WriteLine("        /// Method used to retrieve DicomTag instances for specific DICOM attributes.");
            writer.WriteLine("        /// </summary>");
            writer.WriteLine("        /// <param name=\"tag\">The DICOM tag to retrieve.</param>");
            writer.WriteLine("        /// <returns>A DicomTag instance, if the tag exists, or null if it doesn't.</returns>");
            writer.WriteLine("        public static DicomTag GetDicomTag(uint tag)");
            writer.WriteLine("        {");
			writer.WriteLine("            DicomTag theTag;");
			writer.WriteLine("            if (!_tags.TryGetValue(tag, out theTag))");
			writer.WriteLine("            {");
			writer.WriteLine("            	if (((tag & 0xFFE10000) == 0x60000000) && ((tag & 0xFFFF0000) != 0x60000000))");
			writer.WriteLine("            	{");
			writer.WriteLine("            		theTag = GetDicomTag(tag & 0xFF00FFFF);");
			writer.WriteLine("            		if (theTag == null) return null;");
			writer.WriteLine("");
			writer.WriteLine("            		return new DicomTag(tag, theTag.Name, theTag.VariableName, theTag.VR, theTag.MultiVR, theTag.VMLow, theTag.VMHigh, theTag.Retired);");
			writer.WriteLine("            	}");
			writer.WriteLine("            	return null;");
			writer.WriteLine("            }");
            writer.WriteLine("");
			writer.WriteLine("            return theTag; ");
			writer.WriteLine("        }");
            writer.WriteLine("");
            writer.WriteLine("        /// <summary>");
            writer.WriteLine("        /// Method used to retrieve DicomTag instances for specific DICOM attributes.");
            writer.WriteLine("        /// </summary>");
            writer.WriteLine("        /// <param name=\"name\">The name of the DICOM tag to retrieve.</param>");
            writer.WriteLine("        /// <returns>A DicomTag instance, if the tag exists, or null if it doesn't.</returns>");
            writer.WriteLine("        public static DicomTag GetDicomTag(string name)");
            writer.WriteLine("        {");
			writer.WriteLine("            DicomTag theTag;");
			writer.WriteLine("            if (!_tagNames.TryGetValue(name, out theTag))");
            writer.WriteLine("                return null;");
			writer.WriteLine("            return theTag; ");
            writer.WriteLine("        }");
            writer.WriteLine("");
            writer.WriteLine("        /// <summary>");
            writer.WriteLine("        /// Initialize dictionary with standard tags.");
            writer.WriteLine("        /// </summary>");
            writer.WriteLine("        public static void InitStandardTags()");
            writer.WriteLine("        {");

            IEnumerator<Tag> iter = _tagList.Values.GetEnumerator();

            while (iter.MoveNext())
            {
                Tag tag = iter.Current;
                uint vmLow = 0;
                uint vmHigh = 0;
                char[] charSeparators = new char[] { '�', '-' };
                
                String[] nodes = tag.vm.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
                if (nodes.Length == 1)
                {
                    vmLow = uint.Parse(nodes[0]);
                    vmHigh = vmLow;
                }
                else if (nodes.Length == 2)
                {
                    if (nodes[0].Contains("N") || nodes[0].Contains("n"))
                    {
                        vmLow = 1;
                    }
                    else
                    {
                        vmLow = uint.Parse(nodes[0]);
                    }
                    if (nodes[1].Contains("N") || nodes[1].Contains("n"))
                    {
                        vmHigh = UInt32.MaxValue;
                    }
                    else
                    {
                        vmHigh = uint.Parse(nodes[1]);
                    }


                }

                //public Tag(uint tag, String name, DicomVr vr, uint vmLow, uint vmHigh, bool isRetired)
                writer.WriteLine("            _tags.Add(DicomTags." + tag.varName + ",");
                writer.WriteLine("                      new DicomTag(");
                writer.WriteLine("                          DicomTags." + tag.varName + ",");
                writer.WriteLine("                          \"" + tag.unEscapedName + "\",");
                writer.WriteLine("                          \"" + tag.varName + "\",");
                if (tag.vr.Contains("or"))
                {
                    if (tag.varName.Equals("PixelData"))
                    {
                        writer.WriteLine("                          DicomVr.OWvr,");
                        writer.WriteLine("                          true, //isMultiVrTag");
                    }
                    else
                    {

                        // Just take the first VR listed
                        writer.WriteLine("                          DicomVr." + tag.vr.Substring(0, 2) + "vr,");
                        writer.WriteLine("                          true, //isMultiVrTag");
                    }
                }
                else
                {
                    writer.WriteLine("                          DicomVr." + tag.vr + "vr,");
                    writer.WriteLine("                          false, //isMultiVrTag");
                }
                writer.WriteLine("                          " + vmLow + ", // vmLow");
                writer.WriteLine("                          " + vmHigh + ", // vmHigh");
                if (tag.retired != null && tag.retired.Equals("RET"))
                    writer.WriteLine("                          true // isRetired");
                else
                    writer.WriteLine("                          false // isRetired");
                writer.WriteLine("                          ));");

                writer.WriteLine("            _tagNames.Add(\"" + tag.varName + "\",");
                writer.WriteLine("                      _tags[DicomTags." + tag.varName + "]);");
            }
            writer.WriteLine("        }");

            writer.WriteLine("    }"); // end of class

            WriterFooter(writer);

            writer.Close();
        }

        /// <summary>
        /// Get transfer syntax details from the TransferSyntax.xml file for a specific transfer syntax.
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="littleEndian"></param>
        /// <param name="encapsulated"></param>
        /// <param name="explicitVR"></param>
        /// <param name="deflated"></param>
        /// <param name="lossy"></param>
        /// <param name="lossless"></param>
        public void GetTransferSyntaxDetails(String uid, ref String littleEndian, ref String encapsulated,
            ref String explicitVR, ref String deflated, ref string lossy, ref string lossless)
        {
            XmlNode syntaxNode = _transferSyntaxDoc.FirstChild;

            // I know the format, just do a quick traversal to the first TransferSyntax entry
            syntaxNode = syntaxNode.NextSibling;
            syntaxNode = syntaxNode.FirstChild;

            while (syntaxNode != null)
            {
                if (syntaxNode.Name.Equals("TransferSyntax"))
                {
                    String xmlUid = syntaxNode.Attributes["uid"].Value;

                    if (xmlUid.Equals(uid))
                    {
                        littleEndian = syntaxNode.Attributes["littleEndian"].Value;
                        encapsulated = syntaxNode.Attributes["encapsulated"].Value;
                        explicitVR = syntaxNode.Attributes["explicitVR"].Value;
                        deflated = syntaxNode.Attributes["deflated"].Value;
						lossy = syntaxNode.Attributes["lossy"].Value;
						lossless = syntaxNode.Attributes["lossless"].Value;
						return;
                    }
                }
                syntaxNode = syntaxNode.NextSibling;
            }
        }

        /// <summary>
        /// Create the TransferSyntax.cs file.
        /// </summary>
        /// <param name="syntaxFile"></param>
        public void WriteTransferSyntaxes(String syntaxFile)
        {
            StreamWriter writer = new StreamWriter(syntaxFile);

            WriterHeader(writer);
            writer.WriteLine("    /// <summary>");
            writer.WriteLine("    /// Enumerated value to differentiate between little and big endian.");
            writer.WriteLine("    /// </summary>");
            writer.WriteLine("    public enum Endian");
            writer.WriteLine("    {");
            writer.WriteLine("        Little,");
            writer.WriteLine("        Big");
            writer.WriteLine("    }");
            writer.WriteLine("");

            writer.WriteLine("    /// <summary>");
            writer.WriteLine("    /// This class contains transfer syntax definitions.");
            writer.WriteLine("    /// </summary>");
            writer.WriteLine("    public class TransferSyntax");
            writer.WriteLine("    {");

            IEnumerator iter = _tSyntaxList.GetEnumerator();

            while (iter.MoveNext())
            {
                SopClass tSyntax = (SopClass)((DictionaryEntry)iter.Current).Value;

                writer.WriteLine("        /// <summary>String representing");
                writer.WriteLine("        /// <para>" + tSyntax.name + "</para>");
                writer.WriteLine("        /// <para>UID: " + tSyntax.uid + "</para>");
                writer.WriteLine("        /// </summary>");
                writer.WriteLine("        public static readonly String " + tSyntax.varName + "Uid = \"" + tSyntax.uid + "\";");
                writer.WriteLine("");

                String littleEndian = "";
                String encapsulated = "";
                String explicitVR = "";
                String deflated = "";
            	String lossless = "";
            	string lossy = "";

                GetTransferSyntaxDetails(tSyntax.uid, ref littleEndian, ref encapsulated, ref explicitVR, ref deflated, ref lossy, ref lossless);

                writer.WriteLine("        /// <summary>TransferSyntax object representing");
                writer.WriteLine("        /// <para>" + tSyntax.name + "</para>");
                writer.WriteLine("        /// <para>UID: " + tSyntax.uid + "</para>");
                writer.WriteLine("        /// </summary>");
                writer.WriteLine("        public static readonly TransferSyntax " + tSyntax.varName + " =");
                writer.WriteLine("                    new TransferSyntax(\"" + tSyntax.name + "\",");
                writer.WriteLine("                                 " + tSyntax.varName + "Uid,");
                writer.WriteLine("                                 " + littleEndian + ", // Little Endian?");
                writer.WriteLine("                                 " + encapsulated + ", // Encapsulated?");
                writer.WriteLine("                                 " + explicitVR + ", // Explicit VR?");
                writer.WriteLine("                                 " + deflated + ", // Deflated?");
				writer.WriteLine("                                 " + lossy + ", // lossy?");
				writer.WriteLine("                                 " + lossless + " // lossless?");
				writer.WriteLine("                                 );");
                writer.WriteLine("");
            }

            writer.WriteLine("        // Internal members");
            writer.WriteLine("        private static readonly Dictionary<String,TransferSyntax> _transferSyntaxes = new Dictionary<String,TransferSyntax>();");
            writer.WriteLine("        private readonly bool _littleEndian;");
            writer.WriteLine("        private readonly bool _encapsulated;");
            writer.WriteLine("        private readonly bool _explicitVr;");
            writer.WriteLine("        private readonly bool _deflate;");
			writer.WriteLine("        private readonly bool _lossless;");
			writer.WriteLine("        private readonly bool _lossy;");
			writer.WriteLine("        private readonly String _name;");
            writer.WriteLine("        private readonly String _uid;");
            writer.WriteLine("");
            writer.WriteLine("        ///<summary>");
            writer.WriteLine("        /// Constructor for transfer syntax objects");
            writer.WriteLine("        ///</summary>");
            writer.WriteLine("        public TransferSyntax(String name, String uid, bool bLittleEndian, bool bEncapsulated, bool bExplicitVr, bool bDeflate, bool bLossy, bool bLossless)");
            writer.WriteLine("        {");
            writer.WriteLine("            _uid = uid;");
            writer.WriteLine("            _name = name;");
            writer.WriteLine("            _littleEndian = bLittleEndian;");
            writer.WriteLine("            _encapsulated = bEncapsulated;");
            writer.WriteLine("            _explicitVr = bExplicitVr;");
            writer.WriteLine("            _deflate = bDeflate;");
			writer.WriteLine("            _lossy = bLossy;");
			writer.WriteLine("            _lossless = bLossless;");
			writer.WriteLine("        }");
            writer.WriteLine("");
            writer.WriteLine("        ///<summary>Override to the ToString() method, returns the name of the transfer syntax.</summary>");
            writer.WriteLine("        public override String ToString()");
            writer.WriteLine("        {");
            writer.WriteLine("            return _name;");
            writer.WriteLine("        }");
            writer.WriteLine("");
            writer.WriteLine("        ///<summary>Property representing the UID string of transfer syntax.</summary>");
            writer.WriteLine("        public String UidString");
            writer.WriteLine("        {");
            writer.WriteLine("            get { return _uid; }");
            writer.WriteLine("        }");
            writer.WriteLine("");
            writer.WriteLine("        ///<summary>Property representing the DicomUid of the transfer syntax.</summary>");
            writer.WriteLine("        public DicomUid DicomUid");
            writer.WriteLine("        {");
            writer.WriteLine("            get");
            writer.WriteLine("            {");
            writer.WriteLine("                return new DicomUid(_uid, _name, UidType.TransferSyntax);");
            writer.WriteLine("            }");
            writer.WriteLine("        }");
            writer.WriteLine("");
            writer.WriteLine("        ///<summary>Property representing the name of the transfer syntax.</summary>");
            writer.WriteLine("        public String Name");
            writer.WriteLine("        {");
            writer.WriteLine("            get { return _name; }");
            writer.WriteLine("        }");
            writer.WriteLine("");
            writer.WriteLine("        ///<summary>Property representing if the transfer syntax is encoded as little endian.</summary>");
            writer.WriteLine("        public bool LittleEndian");
            writer.WriteLine("        {");
            writer.WriteLine("            get { return _littleEndian; }");
            writer.WriteLine("        }");
            writer.WriteLine("");
            writer.WriteLine("        ///<summary>Property representing the Endian enumerated value for the transfer syntax.</summary>");
            writer.WriteLine("        public Endian Endian");
            writer.WriteLine("        {");
            writer.WriteLine("            get");
            writer.WriteLine("            {");
            writer.WriteLine("                if (_littleEndian)");
            writer.WriteLine("                    return Endian.Little;");
            writer.WriteLine("");
            writer.WriteLine("                return Endian.Big;");
            writer.WriteLine("            }");
            writer.WriteLine("        }");
            writer.WriteLine("");
            writer.WriteLine("        ///<summary>Property representing if the transfer syntax is encoded as encapsulated.</summary>");
            writer.WriteLine("        public bool Encapsulated");
            writer.WriteLine("        {");
            writer.WriteLine("            get { return _encapsulated; }");
            writer.WriteLine("        }");
            writer.WriteLine("");
			writer.WriteLine("        ///<summary>Property representing if the transfer syntax is a lossy compression syntax.</summary>");
			writer.WriteLine("        public bool LossyCompressed");
			writer.WriteLine("        {");
			writer.WriteLine("            get { return _lossy; }");
			writer.WriteLine("        }");
			writer.WriteLine("");
			writer.WriteLine("        ///<summary>Property representing if the transfer syntax is a lossless compression syntax.</summary>");
			writer.WriteLine("        public bool LosslessCompressed");
			writer.WriteLine("        {");
			writer.WriteLine("            get { return _lossless; }");
			writer.WriteLine("        }");
			writer.WriteLine("");
			writer.WriteLine("        ///<summary>Property representing if the transfer syntax is encoded as explicit Value Representation.</summary>");
            writer.WriteLine("        public bool ExplicitVr");
            writer.WriteLine("        {");
            writer.WriteLine("            get { return _explicitVr; }");
            writer.WriteLine("        }");
            writer.WriteLine("");
            writer.WriteLine("        ///<summary>Property representing if the transfer syntax is encoded in deflate format.</summary>");
            writer.WriteLine("        public bool Deflate");
            writer.WriteLine("        {");
            writer.WriteLine("            get { return _deflate; }");
            writer.WriteLine("        }");
            writer.WriteLine("");
            writer.WriteLine("        /// <summary>");
            writer.WriteLine("        /// Get a TransferSyntax object for a specific transfer syntax UID.");
            writer.WriteLine("        /// </summary>");
            writer.WriteLine("        public static TransferSyntax GetTransferSyntax(String uid)");
            writer.WriteLine("        {");
			writer.WriteLine("            TransferSyntax theSyntax;");
			writer.WriteLine("            if (!_transferSyntaxes.TryGetValue(uid, out theSyntax))");
            writer.WriteLine("                return null;");
            writer.WriteLine("");
			writer.WriteLine("            return theSyntax;");
            writer.WriteLine("        }");
            writer.WriteLine("");
            
            writer.WriteLine("        static TransferSyntax()");
            writer.WriteLine("        {");

            iter = _tSyntaxList.GetEnumerator();

            while (iter.MoveNext())
            {
                SopClass tSyntax = (SopClass)((DictionaryEntry)iter.Current).Value;

                writer.WriteLine("            _transferSyntaxes.Add(" + tSyntax.varName + "Uid,");
                writer.WriteLine("                                  " + tSyntax.varName + ");");
                writer.WriteLine("");
            }

            writer.WriteLine("        }");

            writer.WriteLine("    }");
            WriterFooter(writer);

            writer.Close();
        }

        /// <summary>
        /// Create the SopClass.cs file.
        /// </summary>
        /// <param name="sopsFile"></param>
        public void WriteSqlInsert(String sopsFile)
        {
            StreamWriter writer = new StreamWriter(sopsFile);

            WriterHeader(writer);

            IEnumerator iter = _sopList.GetEnumerator();

            while (iter.MoveNext())
            {
                SopClass sopClass = (SopClass)((DictionaryEntry)iter.Current).Value;
                
                
                writer.WriteLine("INSERT INTO [ImageServer].[dbo].[SopClass] ([GUID],[SopClassUid],[Description],[NonImage])");
                if (sopClass.name.ToLower().Contains("image"))
                    writer.WriteLine("VALUES (newid(), '" + sopClass.uid + "', '" + sopClass.name + "', 0);");
                else
                    writer.WriteLine("VALUES (newid(), '" + sopClass.uid + "', '" + sopClass.name + "', 1);");
                writer.WriteLine("");
                
            }


            writer.Close();
        }

        /// <summary>
        /// Create the SopClass.cs file.
        /// </summary>
        /// <param name="sopsFile"></param>
        public void WriteSopClasses(String sopsFile)
        {
            StreamWriter writer = new StreamWriter(sopsFile);

            WriterHeader(writer);

            writer.WriteLine("    /// <summary>");
            writer.WriteLine("    /// This class contains defines for all DICOM SOP Classes.");
            writer.WriteLine("    /// </summary>");
            writer.WriteLine("    public class SopClass");
            writer.WriteLine("    {");

            IEnumerator iter = _sopList.GetEnumerator();

            while (iter.MoveNext())
            {
                SopClass sopClass = (SopClass)((DictionaryEntry)iter.Current).Value;

                writer.WriteLine("        /// <summary>");
                writer.WriteLine("        /// <para>" + sopClass.name + "</para>");
                writer.WriteLine("        /// <para>UID: " + sopClass.uid + "</para>");
                writer.WriteLine("        /// </summary>");
                writer.WriteLine("        public static readonly String " + sopClass.varName + "Uid = \"" + sopClass.uid + "\";");
                writer.WriteLine("");
                writer.WriteLine("        /// <summary>SopClass for");
                writer.WriteLine("        /// <para>" + sopClass.name + "</para>");
                writer.WriteLine("        /// <para>UID: " + sopClass.uid + "</para>");
                writer.WriteLine("        /// </summary>");
                writer.WriteLine("        public static readonly SopClass " + sopClass.varName + " =");
                writer.WriteLine("                             new SopClass(\"" + sopClass.name + "\", ");
                writer.WriteLine("                                          " + sopClass.varName + "Uid, ");
                writer.WriteLine("                                          false);");
                writer.WriteLine("");

            }

            iter = _metaSopList.GetEnumerator();

            while (iter.MoveNext())
            {
                SopClass sopClass = (SopClass)((DictionaryEntry)iter.Current).Value;

                writer.WriteLine("        /// <summary>String UID for");
                writer.WriteLine("        /// <para>" + sopClass.name + "</para>");
                writer.WriteLine("        /// <para>UID: " + sopClass.uid + "</para>");
                writer.WriteLine("        /// </summary>");
                writer.WriteLine("        public static readonly String " + sopClass.varName + "Uid = \"" + sopClass.uid + "\";");
                writer.WriteLine("");
                writer.WriteLine("        /// <summary>SopClass for");
                writer.WriteLine("        /// <para>" + sopClass.name + "</para>");
                writer.WriteLine("        /// <para>UID: " + sopClass.uid + "</para>");
                writer.WriteLine("        /// </summary>");
                writer.WriteLine("        public static readonly SopClass " + sopClass.varName + " =");
                writer.WriteLine("                             new SopClass(\"" + sopClass.name + "\", ");
                writer.WriteLine("                                          " + sopClass.varName + "Uid, ");
                writer.WriteLine("                                          true);");
            }


            /*
             * Now, write out the constructor and the actual class
             */
            writer.WriteLine("");
            writer.WriteLine("        private readonly String _sopName;");
			writer.WriteLine("        private readonly String _sopUid;");
			writer.WriteLine("        private readonly bool _bIsMeta;");
            writer.WriteLine("");
            writer.WriteLine("        /// <summary> Property that represents the Name of the SOP Class. </summary>");
            writer.WriteLine("        public String Name");
            writer.WriteLine("        {");
            writer.WriteLine("            get { return _sopName; }");
            writer.WriteLine("        }");
            writer.WriteLine("        /// <summary> Property that represents the Uid for the SOP Class. </summary>");
            writer.WriteLine("        public String Uid");
            writer.WriteLine("        {");
            writer.WriteLine("            get { return _sopUid; }");
            writer.WriteLine("        }");
            writer.WriteLine("        /// <summary> Property that returns a DicomUid that represents the SOP Class. </summary>");
            writer.WriteLine("        public DicomUid DicomUid");
            writer.WriteLine("        {");
            writer.WriteLine("            get { return new DicomUid(_sopUid,_sopName,_bIsMeta ? UidType.MetaSOPClass : UidType.SOPClass); }");
            writer.WriteLine("        }");
            writer.WriteLine("        /// <summary> Property that represents the Uid for the SOP Class. </summary>");
            writer.WriteLine("        public bool Meta");
            writer.WriteLine("        {");
            writer.WriteLine("            get { return _bIsMeta; }");
            writer.WriteLine("        }");
            writer.WriteLine("        /// <summary> Constructor to create SopClass object. </summary>");
            writer.WriteLine("        public SopClass(String name,");
            writer.WriteLine("                           String uid,");
            writer.WriteLine("                           bool isMeta)");
            writer.WriteLine("        {");
            writer.WriteLine("            _sopName = name;");
            writer.WriteLine("            _sopUid = uid;");
            writer.WriteLine("            _bIsMeta = isMeta;");
            writer.WriteLine("        }");
            writer.WriteLine("");
            writer.WriteLine("        private static Dictionary<String,SopClass> _sopList = new Dictionary<String,SopClass>();");
            writer.WriteLine("");
            writer.WriteLine("        /// <summary>Override that displays the name of the SOP Class.</summary>");
            writer.WriteLine("        public override string ToString()");
            writer.WriteLine("        {");
            writer.WriteLine("            return this.Name;");
            writer.WriteLine("        }");
            writer.WriteLine("");
            writer.WriteLine("        /// <summary>Retrieve a SopClass object associated with the Uid.</summary>");
            writer.WriteLine("        public static SopClass GetSopClass(String uid)");
            writer.WriteLine("        {");
			writer.WriteLine("            SopClass theSop;");
			writer.WriteLine("            if (!_sopList.TryGetValue(uid, out theSop))");
			writer.WriteLine("                return null;");
            writer.WriteLine("");
            writer.WriteLine("            return theSop;");
            writer.WriteLine("        }");

            writer.WriteLine("        static SopClass()");
            writer.WriteLine("        {");

            // Standard Sops
            iter = _sopList.GetEnumerator();

            while (iter.MoveNext())
            {
                SopClass sopClass = (SopClass)((DictionaryEntry)iter.Current).Value;

                writer.WriteLine("            _sopList.Add(" + sopClass.varName + "Uid, ");
                writer.WriteLine("                         " + sopClass.varName + ");");
                writer.WriteLine("");
            }

            // Now, Meta Sops
            iter = _metaSopList.GetEnumerator();

            while (iter.MoveNext())
            {
                SopClass sopClass = (SopClass)((DictionaryEntry)iter.Current).Value;

                writer.WriteLine("            _sopList.Add(" + sopClass.varName + "Uid, ");
                writer.WriteLine("                         " + sopClass.varName + ");");
                writer.WriteLine("");
            }
            writer.WriteLine("        }");
            writer.WriteLine("");
            
            writer.WriteLine("    }");
            WriterFooter(writer);

            writer.Close();
        }
    }
}
