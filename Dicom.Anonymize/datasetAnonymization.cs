using System;
using Dicom;
using Dicom.Data;

namespace Dicom.Anonymize
{
	public class datasetAnonymization
	{
		anonymizeData anon = new anonymizeData();
		DicomFileFormat fileEdit = new DicomFileFormat();
		
		public void anonymizeDicomFile(String dicomFile, int fileCount)
		{
			try
			{
				fileEdit.Load(dicomFile, DicomReadOptions.DeferLoadingLargeElements);
				
				fileEdit.Dataset.SetString(new DicomTag(DicomConstTags.PatientsName), anonymizeData.patientName());
                fileEdit.Dataset.SetString(new DicomTag(DicomConstTags.PatientsBirthDate), newData.patientDOB());
                fileEdit.Dataset.SetString(new DicomTag(DicomConstTags.PatientID), newData.patientID());

                fileEdit.Dataset.SetString(new DicomTag(DicomConstTags.StudyDate), newData.studyDate());
                fileEdit.Dataset.SetString(new DicomTag(DicomConstTags.StudyDescription), newData.studyDescription());
                fileEdit.Dataset.SetString(new DicomTag(DicomConstTags.StudyInstanceUID), newStudyInstanceId); // major confusion on this; I may need to do SOPId too and possibly Study ID
                fileEdit.Dataset.SetString(new DicomTag(DicomConstTags.SeriesInstanceUID), newSessionIdString);
                fileEdit.Dataset.SetString(new DicomTag(DicomConstTags.SOPInstanceUID), newInstanceIdString);

                fileEdit.Dataset.SetString(new DicomTag(DicomConstTags.AccessionNumber), newData.accessionNumber());
                fileEdit.FileMetaInfo.SetString(new DicomTag(DicomConstTags.MediaStorageSOPInstanceUID), newInstanceIdString);
			}

			
			
			              
		}			
	}
}

