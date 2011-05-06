using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using Dicom;
using Dicom.Data;

namespace Monodicom.Utilities
{
	public class DicomUpdate
	{
        public void UpdateDicomFile(UpdateData updateData)
        {
            DicomFileFormat dicomFile = new DicomFileFormat();

            dicomFile.Load(updateData.DicomFileName, DicomReadOptions.DeferLoadingLargeElements);

            foreach (KeyValuePair<DicomTag, string> kvp in updateData.UpdateDataset)
            {
                dicomFile.Dataset.SetString(kvp.Key, kvp.Value);
            }

            foreach (KeyValuePair<DicomTag, string> kvp in updateData.UpdateMetadata)
            {
                dicomFile.FileMetaInfo.SetString(kvp.Key, kvp.Value);
            }

            dicomFile.Dataset.PreloadDeferredBuffers();
            dicomFile.Save(updateData.DicomFileName, DicomWriteOptions.Default);

            //if (updateData.DicomFileName != null)
            //{
            //    string path = Path.GetDirectoryName(updateData.DicomFilePath);
            //    File.Move(updateData.DicomFilePath, path + @"\" + updateData.DicomFileName);
            //}
        }

		public void UpdateDicomFile(string updateFile, string dicomTag, string newValue)
		{
            try
            {
                DicomFileFormat dicomFile = new DicomFileFormat();
                DicomTag updateDicomTag = DicomTag.Parse(dicomTag);

                dicomFile.Load(updateFile, DicomReadOptions.DeferLoadingLargeElements);
                string what = updateDicomTag.Entry.ToString();

                if (updateDicomTag.Entry.ToString().StartsWith("0002"))
                {
                    dicomFile.FileMetaInfo.SetString(updateDicomTag, newValue);
                }

                else
                {
                    dicomFile.Dataset.SetString(updateDicomTag, newValue);
                }

                dicomFile.Dataset.PreloadDeferredBuffers();
                dicomFile.Save(updateFile, DicomWriteOptions.Default);
            }

            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
	}		
}

