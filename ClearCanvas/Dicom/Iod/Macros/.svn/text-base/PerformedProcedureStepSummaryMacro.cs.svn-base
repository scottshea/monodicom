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
using ClearCanvas.Dicom.Iod.Macros.PerformedProcedureStepSummary;
using ClearCanvas.Dicom.Iod.Macros.PerformedProcedureStepSummary.PerformedProtocolCodeSequence;
using ClearCanvas.Dicom.Utilities;

namespace ClearCanvas.Dicom.Iod.Macros
{
	/// <summary>
	/// PerformedProcedureStepSummary Macro
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section 10.13 (Table 10-16)</remarks>
	public interface IPerformedProcedureStepSummaryMacro : IIodMacro
	{
		/// <summary>
		/// Gets or sets the value of PerformedProcedureStepId in the underlying collection. Type 3.
		/// </summary>
		string PerformedProcedureStepId { get; set; }

		/// <summary>
		/// Gets or sets the value of PerformedProcedureStepStartDate and PerformedProcedureStepStartTime in the underlying collection.  Type 3.
		/// </summary>
		DateTime? PerformedProcedureStepStartDateTime { get; set; }

		/// <summary>
		/// Gets or sets the value of PerformedProcedureStepDescription in the underlying collection. Type 3.
		/// </summary>
		string PerformedProcedureStepDescription { get; set; }

		/// <summary>
		/// Gets or sets the value of PerformedProtocolCodeSequence in the underlying collection. Type 3.
		/// </summary>
		IPerformedProtocolCodeSequence[] PerformedProtocolCodeSequence { get; set; }

		/// <summary>
		/// Gets or sets the value of CommentsOnThePerformedProcedureStep in the underlying collection. Type 3.
		/// </summary>
		string CommentsOnThePerformedProcedureStep { get; set; }

		/// <summary>
		/// Creates a single instance of a PerformedProtocolCodeSequence item. Does not modify the PerformedProtocolCodeSequence in the underlying collection.
		/// </summary>
		IPerformedProtocolCodeSequence CreatePerformedProtocolCodeSequence();
	}

	/// <summary>
	/// PerformedProcedureStepSummary Macro
	/// </summary>
	/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section 10.13 (Table 10-16)</remarks>
	internal class PerformedProcedureStepSummaryMacro : SequenceIodBase, IPerformedProcedureStepSummaryMacro
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PerformedProcedureStepSummaryMacro"/> class.
		/// </summary>
		public PerformedProcedureStepSummaryMacro() : base() {}

		/// <summary>
		/// Initializes a new instance of the <see cref="PerformedProcedureStepSummaryMacro"/> class.
		/// </summary>
		/// <param name="dicomSequenceItem">The dicom sequence item.</param>
		public PerformedProcedureStepSummaryMacro(DicomSequenceItem dicomSequenceItem) : base(dicomSequenceItem) {}

		/// <summary>
		/// Initializes the underlying collection to implement the module or sequence using default values.
		/// </summary>
		public void InitializeAttributes() {}

		/// <summary>
		/// Gets or sets the value of PerformedProcedureStepId in the underlying collection. Type 3.
		/// </summary>
		public string PerformedProcedureStepId
		{
			get { return base.DicomAttributeProvider[DicomTags.PerformedProcedureStepId].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					base.DicomAttributeProvider[DicomTags.PerformedProcedureStepId] = null;
					return;
				}
				base.DicomAttributeProvider[DicomTags.PerformedProcedureStepId].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of PerformedProcedureStepStartDate and PerformedProcedureStepStartTime in the underlying collection.  Type 3.
		/// </summary>
		public DateTime? PerformedProcedureStepStartDateTime
		{
			get
			{
				string date = base.DicomAttributeProvider[DicomTags.PerformedProcedureStepStartDate].GetString(0, string.Empty);
				string time = base.DicomAttributeProvider[DicomTags.PerformedProcedureStepStartTime].GetString(0, string.Empty);
				return DateTimeParser.ParseDateAndTime(string.Empty, date, time);
			}
			set
			{
				if (!value.HasValue)
				{
					base.DicomAttributeProvider[DicomTags.PerformedProcedureStepStartDate] = null;
					base.DicomAttributeProvider[DicomTags.PerformedProcedureStepStartTime] = null;
					return;
				}
				DicomAttribute date = base.DicomAttributeProvider[DicomTags.PerformedProcedureStepStartDate];
				DicomAttribute time = base.DicomAttributeProvider[DicomTags.PerformedProcedureStepStartTime];
				DateTimeParser.SetDateTimeAttributeValues(value, date, time);
			}
		}

		/// <summary>
		/// Gets or sets the value of PerformedProcedureStepDescription in the underlying collection. Type 3.
		/// </summary>
		public string PerformedProcedureStepDescription
		{
			get { return base.DicomAttributeProvider[DicomTags.PerformedProcedureStepDescription].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					base.DicomAttributeProvider[DicomTags.PerformedProcedureStepDescription] = null;
					return;
				}
				base.DicomAttributeProvider[DicomTags.PerformedProcedureStepDescription].SetString(0, value);
			}
		}

		/// <summary>
		/// Gets or sets the value of PerformedProtocolCodeSequence in the underlying collection. Type 3.
		/// </summary>
		public IPerformedProtocolCodeSequence[] PerformedProtocolCodeSequence
		{
			get
			{
				DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.PerformedProtocolCodeSequence];
				if (dicomAttribute.IsNull || dicomAttribute.Count == 0)
				{
					return null;
				}

				IPerformedProtocolCodeSequence[] result = new IPerformedProtocolCodeSequence[dicomAttribute.Count];
				DicomSequenceItem[] items = (DicomSequenceItem[]) dicomAttribute.Values;
				for (int n = 0; n < items.Length; n++)
					result[n] = new PerformedProtocolCodeSequenceClass(items[n]);

				return result;
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					base.DicomAttributeProvider[DicomTags.PerformedProtocolCodeSequence] = null;
					return;
				}

				DicomSequenceItem[] result = new DicomSequenceItem[value.Length];
				for (int n = 0; n < value.Length; n++)
					result[n] = value[n].DicomSequenceItem;

				base.DicomAttributeProvider[DicomTags.PerformedProtocolCodeSequence].Values = result;
			}
		}

		/// <summary>
		/// Creates a single instance of a PerformedProtocolCodeSequence item. Does not modify the PerformedProtocolCodeSequence in the underlying collection.
		/// </summary>
		public IPerformedProtocolCodeSequence CreatePerformedProtocolCodeSequence()
		{
			IPerformedProtocolCodeSequence iodBase = new PerformedProtocolCodeSequenceClass(new DicomSequenceItem());
			iodBase.InitializeAttributes();
			return iodBase;
		}

		/// <summary>
		/// Gets or sets the value of CommentsOnThePerformedProcedureStep in the underlying collection. Type 3.
		/// </summary>
		public string CommentsOnThePerformedProcedureStep
		{
			get { return base.DicomAttributeProvider[DicomTags.CommentsOnThePerformedProcedureStep].GetString(0, string.Empty); }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					base.DicomAttributeProvider[DicomTags.CommentsOnThePerformedProcedureStep] = null;
					return;
				}
				base.DicomAttributeProvider[DicomTags.CommentsOnThePerformedProcedureStep].SetString(0, value);
			}
		}

		/// <summary>
		/// PerformedProtocol Code Sequence
		/// </summary>
		/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section 10.13 (Table 10-16)</remarks>
		internal class PerformedProtocolCodeSequenceClass : CodeSequenceMacro, IPerformedProtocolCodeSequence
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="PerformedProtocolCodeSequenceClass"/> class.
			/// </summary>
			public PerformedProtocolCodeSequenceClass() : base() {}

			/// <summary>
			/// Initializes a new instance of the <see cref="PerformedProtocolCodeSequenceClass"/> class.
			/// </summary>
			/// <param name="dicomSequenceItem">The dicom sequence item.</param>
			public PerformedProtocolCodeSequenceClass(DicomSequenceItem dicomSequenceItem) : base(dicomSequenceItem) {}

			/// <summary>
			/// Initializes the underlying collection to implement the module or sequence using default values.
			/// </summary>
			public virtual void InitializeAttributes() {}

			/// <summary>
			/// Gets or sets the value of ProtocolContextSequence in the underlying collection. Type 3.
			/// </summary>
			public IProtocolContextSequence[] ProtocolContextSequence
			{
				get
				{
					DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.ProtocolContextSequence];
					if (dicomAttribute.IsNull || dicomAttribute.Count == 0)
					{
						return null;
					}

					ProtocolContextSequenceClass[] result = new ProtocolContextSequenceClass[dicomAttribute.Count];
					DicomSequenceItem[] items = (DicomSequenceItem[]) dicomAttribute.Values;
					for (int n = 0; n < items.Length; n++)
						result[n] = new ProtocolContextSequenceClass(items[n]);

					return result;
				}
				set
				{
					if (value == null || value.Length == 0)
					{
						base.DicomAttributeProvider[DicomTags.ProtocolContextSequence] = null;
						return;
					}

					DicomSequenceItem[] result = new DicomSequenceItem[value.Length];
					for (int n = 0; n < value.Length; n++)
						result[n] = value[n].DicomSequenceItem;

					base.DicomAttributeProvider[DicomTags.ProtocolContextSequence].Values = result;
				}
			}

			/// <summary>
			/// Creates a single instance of a ProtocolContextSequence item. Does not modify the tag in the underlying collection.
			/// </summary>
			public IProtocolContextSequence CreateProtocolContextSequence()
			{
				IProtocolContextSequence iodBase = new ProtocolContextSequenceClass(new DicomSequenceItem());
				iodBase.InitializeAttributes();
				return iodBase;
			}

			/// <summary>
			/// ProtocolContext Sequence
			/// </summary>
			/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section 10.13 (Table 10-16)</remarks>
			internal class ProtocolContextSequenceClass : ContentItemMacro, IProtocolContextSequence
			{
				/// <summary>
				/// Initializes a new instance of the <see cref="ProtocolContextSequenceClass"/> class.
				/// </summary>
				public ProtocolContextSequenceClass() : base() {}

				/// <summary>
				/// Initializes a new instance of the <see cref="ProtocolContextSequenceClass"/> class.
				/// </summary>
				/// <param name="dicomSequenceItem">The dicom sequence item.</param>
				public ProtocolContextSequenceClass(DicomSequenceItem dicomSequenceItem) : base(dicomSequenceItem) {}

				/// <summary>
				/// Initializes the underlying collection to implement the module or sequence using default values.
				/// </summary>
				public virtual void InitializeAttributes() {}

				/// <summary>
				/// Gets or sets the value of ContentItemModifierSequence in the underlying collection. Type 3.
				/// </summary>
				public ContentItemMacro[] ContentItemModifierSequence
				{
					get
					{
						DicomAttribute dicomAttribute = base.DicomAttributeProvider[DicomTags.ContentItemModifierSequence];
						if (dicomAttribute.IsNull || dicomAttribute.Count == 0)
						{
							return null;
						}

						ContentItemMacro[] result = new ContentItemMacro[dicomAttribute.Count];
						DicomSequenceItem[] items = (DicomSequenceItem[]) dicomAttribute.Values;
						for (int n = 0; n < items.Length; n++)
							result[n] = new ContentItemMacro(items[n]);

						return result;
					}
					set
					{
						if (value == null || value.Length == 0)
						{
							base.DicomAttributeProvider[DicomTags.ContentItemModifierSequence] = null;
							return;
						}

						DicomSequenceItem[] result = new DicomSequenceItem[value.Length];
						for (int n = 0; n < value.Length; n++)
							result[n] = value[n].DicomSequenceItem;

						base.DicomAttributeProvider[DicomTags.ContentItemModifierSequence].Values = result;
					}
				}
			}
		}
	}

	namespace PerformedProcedureStepSummary
	{
		/// <summary>
		/// PerformedProtocol Code Sequence
		/// </summary>
		/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section 10.13 (Table 10-16)</remarks>
		public interface IPerformedProtocolCodeSequence : IIodMacro
		{
			/// <summary>
			/// Gets or sets the value of ProtocolContextSequence in the underlying collection. Type 3.
			/// </summary>
			IProtocolContextSequence[] ProtocolContextSequence { get; set; }

			/// <summary>
			/// Creates a single instance of a ProtocolContextSequence item. Does not modify the tag in the underlying collection.
			/// </summary>
			IProtocolContextSequence CreateProtocolContextSequence();
		}

		namespace PerformedProtocolCodeSequence
		{
			/// <summary>
			/// ProtocolContext Sequence
			/// </summary>
			/// <remarks>As defined in the DICOM Standard 2008, Part 3, Section 10.13 (Table 10-16)</remarks>
			public interface IProtocolContextSequence : IIodMacro
			{
				/// <summary>
				/// Gets or sets the value of ContentItemModifierSequence in the underlying collection. Type 3.
				/// </summary>
				ContentItemMacro[] ContentItemModifierSequence { get; set; }
			}
		}
	}
}