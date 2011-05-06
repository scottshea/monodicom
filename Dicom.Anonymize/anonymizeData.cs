using System;
using Dicom;
using Dicom.Data;

namespace Dicom.Anonymize
{
	public class anonymizeData
	{		
		#region privates
		private static DicomUID _newStudyId = DicomUID.Generate();
		private static DicomUID _newSessionId; //TODO: Is there a way to default this?
		private static String _newStudyDescription = "Study Anonymized at " + DateTime.Now.ToShortTimeString();
		private static String _newStudyDate = getDateAsString();
		private static String _newPatientId = getPatientId();
		private static String _newPatientName = _newPatientId + "^Anonymous";
		private static String _newPatientDOB = getDateAsString();
		private static String _newPhysicianName = "Anonymous^S^Dr.";
		private static String _newAccessionNumber = getAccessionNumber();
		
		#endregion
		
		#region properties
        public DicomUID studyId
        {
            get {return _newStudyId;}
		}

		public DicomUID sessionId
		{
			get {return _newSessionId;}
			//set {_newSessionId = DicomUID.Generate(_newStudyId, value);}
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
		
		#region public methods
		public void setSesionId(int studyNumber) //doing this in place of a set on the property; cannot take an int on the set since it is expecting a DicomUID
		{
			_newSessionId = DicomUID.Generate(_newStudyId, studyNumber);
		}
		#endregion	


		
		#region private methods
        private static string getDateAsString() //returns date string in the yyyymmdd format
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
		
        private static String getPatientId()
        {
            //generate random patient id
            Random randomID = new Random();
			String randPatientId = "AI";
            for (int ctr = 0; ctr <= 5; ctr++)
                randPatientId = randPatientId + randomID.Next(10).ToString();
            String newPatientId = randPatientId;
            return newPatientId;
        }
		
		private static String getAccessionNumber()
        {
            //generate accessionNumber
			Random randomID = new Random();
            String randAccessionNum = "AI";
            for (int ctr = 0; ctr <= 5; ctr++)
                randAccessionNum = randAccessionNum + randomID.Next(10).ToString();
            String newAccessionNumber = randAccessionNum;
            return newAccessionNumber;
        }
		#endregion
	}
}


