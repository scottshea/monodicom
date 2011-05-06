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
using ClearCanvas.Dicom.Utilities;

namespace ClearCanvas.Dicom.ServiceModel.Query
{
	#region Study Comparers

	/// <summary>
	/// Sorts <see cref="StudyIdentifier"/>s and <see cref="StudyRootStudyIdentifier"/>s by Study Date/time,
	/// in reverse (most recent first).
	/// </summary>
	public class StudyDateTimeComparer : IComparer<StudyIdentifier>, IComparer<StudyRootStudyIdentifier>
	{
		public StudyDateTimeComparer()
		{
		}

		#region IComparer<StudyIdentifier> Members

		/// <summary>
		/// Compares two <see cref="StudyIdentifier"/>s.
		/// </summary>
		public int Compare(StudyIdentifier x, StudyIdentifier y)
		{
			DateTime? studyDateX = DateParser.Parse(x.StudyDate);
			DateTime? studyTimeX = TimeParser.Parse(x.StudyTime);

			DateTime? studyDateY = DateParser.Parse(y.StudyDate);
			DateTime? studyTimeY = TimeParser.Parse(y.StudyTime);

			DateTime? studyDateTimeX = studyDateX;
			if (studyDateTimeX != null && studyTimeX != null)
				studyDateTimeX = studyDateTimeX.Value.Add(studyTimeX.Value.TimeOfDay);

			DateTime? studyDateTimeY = studyDateY;
			if (studyDateTimeY != null && studyTimeY != null)
				studyDateTimeY = studyDateTimeY.Value.Add(studyTimeY.Value.TimeOfDay);

			if (studyDateTimeX == null)
			{
				if (studyDateTimeY == null)
					return Math.Sign(x.StudyInstanceUid.CompareTo(y.StudyInstanceUid));
				else
					return 1; // > because we want x at the end.
			}
			else if (studyDateY == null)
				return -1; // < because we want x at the beginning.

			//Return negative of x compared to y because we want most recent first.
			return -Math.Sign(studyDateTimeX.Value.CompareTo(studyDateTimeY));
		}

		#endregion

		#region IComparer<StudyRootStudyIdentifier> Members

		/// <summary>
		/// Compares 2 <see cref="StudyRootStudyIdentifier"/>s.
		/// </summary>
		public int Compare(StudyRootStudyIdentifier x, StudyRootStudyIdentifier y)
		{
			return Compare((StudyIdentifier) x, (StudyIdentifier) y);
		}

		#endregion
	}

	#endregion
}