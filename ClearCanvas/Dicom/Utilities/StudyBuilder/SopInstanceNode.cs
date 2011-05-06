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
using System.IO;

namespace ClearCanvas.Dicom.Utilities.StudyBuilder
{
	/// <summary>
	/// A <see cref="StudyBuilderNode"/> representing a SOP instance-level data node in the <see cref="StudyBuilder"/> tree hierarchy.
	/// </summary>
	public sealed class SopInstanceNode : StudyBuilderNode
	{
		private readonly DicomFile _dicomFile;
		private string _instanceUid;

		/// <summary>
		/// Constructs a new <see cref="SopInstanceNode"/> using default values.
		/// </summary>
		public SopInstanceNode()
		{
			_dicomFile = new DicomFile("");
			_instanceUid = StudyBuilder.NewUid();
		}

		/// <summary>
		/// Constructs a new <see cref="SopInstanceNode"/> using the given <see cref="DicomFile"/> as a template.
		/// </summary>
		/// <param name="sourceDicomFile">The <see cref="DicomFile"/> from which to initialize this node.</param>
		public SopInstanceNode(DicomMessageBase sourceDicomFile)
		{
			_dicomFile = new DicomFile("", sourceDicomFile.MetaInfo.Copy(), sourceDicomFile.DataSet.Copy());

			_instanceUid = sourceDicomFile.DataSet[DicomTags.SopInstanceUid].GetString(0, "");
			if (_instanceUid == "")
				_instanceUid = StudyBuilder.NewUid();
		}

		/// <summary>
		/// Copy constructor
		/// </summary>
		/// <param name="source"></param>
		private SopInstanceNode(SopInstanceNode source)
		{
			_instanceUid = StudyBuilder.NewUid();
			_dicomFile = new DicomFile("", source._dicomFile.MetaInfo.Copy(true, true, true), source._dicomFile.DataSet.Copy(true, true, true));
		}

		#region Data Properties

		/// <summary>
		/// Gets or sets the SOP instance UID.
		/// </summary>
		public string InstanceUid
		{
			get { return _instanceUid; }
			internal set
			{
				if (_instanceUid != value)
				{
					if (string.IsNullOrEmpty(value))
						value = StudyBuilder.NewUid();

					_instanceUid = value;
					FirePropertyChanged("InstanceUid");
				}
			}
		}

		#endregion

		#region Update Methods

		/// <summary>
		/// Writes the data in this node into the given <see cref="DicomAttributeCollection"/>.
		/// </summary>
		/// <param name="dataSet">The data set to write data into.</param>
		/// <param name="writeUid"></param>
		internal void Update(DicomAttributeCollection dataSet, bool writeUid)
		{
			int imageNumber = 0;
			if (this.Parent != null)
				imageNumber = this.Parent.Images.IndexOf(this) + 1;

			DicomConverter.SetInt32(dataSet[DicomTags.InstanceNumber], imageNumber);

			if (writeUid)
				dataSet[DicomTags.SopInstanceUid].SetStringValue(_instanceUid);
		}

		#endregion

		#region Copy Methods

		/// <summary>
		/// Creates a new <see cref="SopInstanceNode"/> with the same node data, nulling all references to other nodes.
		/// </summary>
		/// <returns>A copy of the node.</returns>
		public SopInstanceNode Copy()
		{
			return this.Copy(false);
		}

		/// <summary>
		/// Creates a new <see cref="SopInstanceNode"/> with the same node data.
		/// </summary>
		/// <param name="keepExtLinks">Specifies that references to nodes outside of the copy scope should be kept. If False, all references are nulled.</param>
		/// <returns>A copy of the node.</returns>
		public SopInstanceNode Copy(bool keepExtLinks)
		{
			return new SopInstanceNode(this);
		}

		#endregion

		#region Misc

		/// <summary>
		/// Gets the parent of this node, or null if the node is not in a study builder tree.
		/// </summary>
		public new SeriesNode Parent
		{
			get { return base.Parent as SeriesNode; }
			internal set { base.Parent = value; }
		}

		/// <summary>
		/// Gets the underlying data set of this node.
		/// </summary>
		public DicomAttributeCollection DicomData
		{
			get { return _dicomFile.DataSet; }
		}

		internal DicomFile DicomFile
		{
			get { return _dicomFile; }
		}

		/// <summary>
		/// Exports the contents of the data set to a DICOM file in the specified directory.
		/// </summary>
		/// <remarks>
		/// The filename is automatically generated using the SOP instance uid and the &quot;.dcm&quot; extension.
		/// </remarks>
		/// <param name="path">The directory to export the data to.</param>
		internal string ExportToDirectory(string path)
		{
			string filename = Path.Combine(path, _instanceUid + ".dcm");
			_dicomFile.Save(filename);
			return filename;
		}

		#endregion
	}
}