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
using System.Windows.Forms;

namespace ClearCanvas.Dicom.Samples
{
	public partial class DicomdirDisplay : Form
	{
		public DicomdirDisplay()
		{
			InitializeComponent();
		}

		private void AddTagNodes(TreeNode parent, DicomSequenceItem record)
		{
			foreach (DicomAttribute attrib in record)
			{
				string name;
				if (attrib is DicomAttributeSQ || attrib is DicomAttributeOB || attrib is DicomAttributeOW || attrib is DicomAttributeOF)
					name = attrib.ToString();
				else
				{
					name = String.Format("{0}: {1}", attrib.Tag.ToString(), attrib.ToString());
				}
				TreeNode tagNode = new TreeNode(name);
				parent.Nodes.Add(tagNode);

				DicomAttributeSQ sqAttrib = attrib as DicomAttributeSQ;
				if (sqAttrib != null)
				{
					for (int i=0; i< sqAttrib.Count; i++)
					{
						TreeNode sqNode = new TreeNode("Sequence Item");
						tagNode.Nodes.Add(sqNode);
						AddTagNodes(sqNode, sqAttrib[i]);
					}
				}
			}
		}

		public void Add(DicomDirectory dir)
		{
			_treeViewDicomdir.BeginUpdate();
			_treeViewDicomdir.TopNode = new TreeNode();
			
			TreeNode topNode = new TreeNode("DICOMDIR: " + dir.FileSetId);

			_treeViewDicomdir.Nodes.Add( topNode);

			foreach (DirectoryRecordSequenceItem patientRecord in dir.RootDirectoryRecordCollection)
			{
				TreeNode patientNode = new TreeNode(patientRecord.ToString());
				topNode.Nodes.Add(patientNode);

				AddTagNodes(patientNode, patientRecord);

				foreach (DirectoryRecordSequenceItem studyRecord in patientRecord.LowerLevelDirectoryRecordCollection)
				{
					TreeNode studyNode = new TreeNode(studyRecord.ToString());
					patientNode.Nodes.Add(studyNode);

					AddTagNodes(studyNode, studyRecord);

					foreach (DirectoryRecordSequenceItem seriesRecord in studyRecord.LowerLevelDirectoryRecordCollection)
					{
						TreeNode seriesNode = new TreeNode(seriesRecord.ToString());
						studyNode.Nodes.Add(seriesNode);

						AddTagNodes(seriesNode, seriesRecord);

						foreach (DirectoryRecordSequenceItem instanceRecord in seriesRecord.LowerLevelDirectoryRecordCollection)
						{
							TreeNode instanceNode = new TreeNode(instanceRecord.ToString());
							seriesNode.Nodes.Add(instanceNode);

							AddTagNodes(instanceNode, instanceRecord);
						}
					}
				}
			}
			_treeViewDicomdir.EndUpdate();
		}
	}
}
