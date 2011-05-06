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


#if UNIT_TESTS
using System.Collections.Generic;
using System.IO;
using System.Xml;
using ClearCanvas.Dicom.Tests;
using NUnit.Framework;

namespace ClearCanvas.Dicom.Utilities.Xml.Tests
{
	[TestFixture]
	public class GeneralStreamingTest : AbstractTest
	{
		[Test]
		public void ConstructorTest()
		{
			StudyXml stream = new StudyXml();

			stream = new StudyXml("1.1.1");

		}

		private void WriteStudyStream(string streamFile, StudyXml theStream)
		{
			StudyXmlOutputSettings settings = new StudyXmlOutputSettings();
			settings.IncludeSourceFileName = false;

			XmlDocument doc = theStream.GetMemento(settings);

			if (File.Exists(streamFile))
				File.Delete(streamFile);

			using (Stream fileStream = new FileStream(streamFile, FileMode.CreateNew))
			{
				StudyXmlIo.Write(doc, fileStream);
				fileStream.Close();
			}

			return;
		}
		/// <summary>
		/// Load a <see cref="StudyXml"/> file for a given <see cref="StudyStorageLocation"/>
		/// </summary>
		/// <param name="location">The location a study is stored.</param>
		/// <returns>The <see cref="StudyXml"/> instance for <paramref name="location"/></returns>
		private StudyXml LoadStudyStream(string location)
		{
			StudyXml theXml = new StudyXml();

			if (File.Exists(location))
			{
				using (Stream fileStream = new FileStream(location, FileMode.Open))
				{
					XmlDocument theDoc = new XmlDocument();

					StudyXmlIo.Read(theDoc, fileStream);

					theXml.SetMemento(theDoc);

					fileStream.Close();
				}
			}


			return theXml;
		}

		[Test]
		public void CreationTest()
		{
			IList<DicomAttributeCollection> instanceList;

			string studyInstanceUid = DicomUid.GenerateUid().UID;

			instanceList = SetupMRSeries(4, 10, studyInstanceUid);



			StudyXml studyXml = new StudyXml(studyInstanceUid);

			string studyXmlFilename = Path.GetTempFileName();

			foreach (DicomAttributeCollection instanceCollection in instanceList)
			{
				instanceCollection[DicomTags.PixelData] = null;


				DicomFile theFile = new DicomFile("test", new DicomAttributeCollection(), instanceCollection);
				SetupMetaInfo(theFile);

				studyXml.AddFile(theFile);

				WriteStudyStream(studyXmlFilename, studyXml);
			}

			StudyXml newXml = LoadStudyStream(studyXmlFilename);

			if (!Compare(newXml, instanceList))
				Assert.Fail("Comparison of StudyXML failed against base loaded from disk");

			if (!Compare(studyXml, instanceList))
				Assert.Fail("Comparison of StudyXML failed against base in memory");

		}

		private bool Compare(StudyXml studyXml, IList<DicomAttributeCollection> instanceList)
		{
			foreach (SeriesXml seriesXml in studyXml)
			{
				foreach (InstanceXml instanceXml in seriesXml)
				{
					foreach (DicomAttributeCollection baseCollection in instanceList)
					{
						string sopInstanceUid = baseCollection[DicomTags.SopInstanceUid].ToString();
						if (sopInstanceUid.Equals(instanceXml.SopInstanceUid))
						{
							if (!baseCollection.Equals(instanceXml.Collection))
							{
								return false;
							}
						}
					}
				}
			}
			return true;
		}
	}
}

#endif