using System;
using Dicom;
using Dicom.Data;

namespace Dicom.Anonymize
{
	public class anonymizeData
	{		
        Random randomID = new Random();
		
		#region privates
		private DicomUID _newStudyId = DicomUID.Generate();
		private DicomUID _newSessionId; //TODO: Is there a way to default this?
		private String _newStudyDescription = "Study Anonymized at " + DateTime.Now.ToShortTimeString();
		private String _newStudyDate = getDateAsString();
		private String _newPatientId = getPatientId();
		private String _newPatientName = patientId.get + "^Anonymous";
		private String _newPatientDOB = getDateAsString();
		private String _newPhysicianName = "Anonymous^S^Dr.";
		private String _newAccessionNumber = getAccessionNumber();
		
		#endregion
		
		#region properties
        public DicomUID studyId
        {
            get {return _newStudyId;}
		}

		public DicomUID sessionId
		{
			get {return _newSessionId;}
			set {_newSessionId = DicomUID.Generate(_newStudyId, value);}
		}
		
		public String studyDescription
		{
			get {return _newStudyDescription;}
			set {_newStudyDescription = value;}
		}

		public String studyDate
		{
			get {return _newStudyDate;}
			set {_newStudyDate = value;}
		}
		
		public String patientId
		{
			get {return _newPatientId;}
			set {_newPatientId = value;}
		}
		
		public String patientName
		{
			get {return _newPatientName;}
			set {_newPatientName = value;}
		}
		
		public String patientDOB
		{
			get {return _newPatientDOB;}
			set {_newPatientDOB = value;}
		}
		
		public String physicianName
		{
			get {return _newPhysicianName;}
			set {_newPhysicianName = value;}
		}
		
		public String accessionNumber
		{
			get {return _newAccessionNumber;}
			set {_newAccessionNumber = value;}
		}
		
		#endregion


		
		#region private methods
        private string getDateAsString() //returns date string in the yyyymmdd format
        {
            string newMonth;
            string newDay;

            if (DateTime.Now.Month < 10)
            {
                newMonth = "0" + DateTime.Now.Month.ToString();
            }
            else
            {
                newMonth = DateTime.Now.Month.ToString();
            }

            if (DateTime.Now.Day < 10)
            {
                newDay = "0" + DateTime.Now.Day.ToString();
            }
            else
            {
                newDay = DateTime.Now.Day.ToString();
            }

            string newDateString = DateTime.Now.Year.ToString() + newMonth + newDay;
            return newDateString;
        }
		
        private String getPatientID()
        {
            //generate random patient id
            String randPatientId = "AI";
            for (int ctr = 0; ctr <= 5; ctr++)
                randPatientId = randPatientId + randomID.Next(10).ToString();
            String newPatientId = randPatientId;
            return newPatientId;
        }
		
		private String getAccessionNumber()
        {
            //generate accessionNumber
            String randAccessionNum = "AI";
            for (int ctr = 0; ctr <= 5; ctr++)
                randAccessionNum = randAccessionNum + randomID.Next(10).ToString();
            String newAccessionNumber = randAccessionNum;
            return newAccessionNumber;
        }
		#endregion
	}
}


