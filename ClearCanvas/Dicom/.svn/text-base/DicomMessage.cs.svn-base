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
using System.Text;
using ClearCanvas.Dicom.Network;

namespace ClearCanvas.Dicom
{
    /// <summary>
    /// Class representing a DICOM Message to be transferred over the network.
    /// </summary>
    /// <seealso cref="DicomMessageBase"/>
    /// <seealso cref="DicomFile"/>
    public class DicomMessage : DicomMessageBase
    {
        #region Private Members
        private TransferSyntax _transferSyntax = TransferSyntax.ExplicitVrLittleEndian;
        #endregion

        #region Command Element Properties
        /// <summary>
        /// The affected SOP Class UID associated with the oepration.
        /// </summary>
        public string AffectedSopClassUid
        {
            get { return _metaInfo[DicomTags.AffectedSopClassUid].GetString(0,String.Empty); }
            set { _metaInfo[DicomTags.AffectedSopClassUid].Values = value; }
        }
        /// <summary>
        /// The requested SOP Class UID associated with the operation.
        /// </summary>
        public string RequestedSopClassUid
        {
            get { return _metaInfo[DicomTags.RequestedSopClassUid].GetString(0,String.Empty); }
            set { _metaInfo[DicomTags.RequestedSopClassUid].Values = value; }
        }
        /// <summary>
        /// This field distinguishes the DIMSE operation conveyed by this Message.
        /// </summary>
        public DicomCommandField CommandField
        {
            get
            {
                ushort command = _metaInfo[DicomTags.CommandField].GetUInt16(0, (ushort)DicomCommandField.CStoreRequest);
                return (DicomCommandField)command;
            }
            set
            {
                _metaInfo[DicomTags.CommandField].Values = (ushort)value;
            }
        }
        /// <summary>
        /// An implementation specific value which distinguishes thsi Message from other Messages.
        /// </summary>
        public ushort MessageId
        {
            get { return _metaInfo[DicomTags.MessageId].GetUInt16(0, 0); }
            set { _metaInfo[DicomTags.MessageId].Values = value; }
        }
        /// <summary>
        /// Shall be set to the value of the Message ID (0000,0110) field used in the associated request Message.
        /// </summary>
        public ushort MessageIdBeingRespondedTo
        {
            get { return _metaInfo[DicomTags.MessageIdBeingRespondedTo].GetUInt16(0, 0); }
            set { _metaInfo[DicomTags.MessageIdBeingRespondedTo].Values = value; }
        }
        /// <summary>
        /// Shall be set to the DICOM AE Ttile of the destination DICOM AE for which the C-STORE sub-operations are being performed.
        /// </summary>
        public string MoveDestination
        {
            get { return _metaInfo[DicomTags.MoveDestination].GetString(0,String.Empty); }
            set { _metaInfo[DicomTags.MoveDestination].Values = value; }
        }
        /// <summary>
        /// The priority shall be set to one of the following values: 
        /// <para>LOW = 0002H</para>
        /// <para>MEDIUM = 0000H</para>
        /// <para>HIGH = 0001H</para>
        /// </summary>
        public DicomPriority Priority
        {
            get
            {
                ushort priority = _metaInfo[DicomTags.Priority].GetUInt16(0, (ushort)DicomPriority.Medium);
                return (DicomPriority)priority;
            }
            set
            {
                _metaInfo[DicomTags.Priority].Values = (ushort)value;
            }
        }
        /// <summary>
        /// This field indicates if a Data Set is present in the Message.  This field shall be set to the value
        /// of 0101H if no Data Set is present, any other value indicates a Data Set is included in the Message.
        /// </summary>
        public ushort DataSetType
        {
            get { return _metaInfo[DicomTags.DataSetType].GetUInt16(0, 0); }
            set { _metaInfo[DicomTags.DataSetType].Values = value; }
        }
        /// <summary>
        /// Confirmation status of the operation.
        /// </summary>
        public DicomStatus Status
        {
            get
            {
                ushort status = _metaInfo[DicomTags.Status].GetUInt16(0, 0);
                return DicomStatuses.Lookup(status);
            }
            set
            {
                _metaInfo[DicomTags.Status].Values = value.Code;
            }
        }
        /// <summary>
        /// If status is Cxxx, then this field contains a list of the elements in which the error was detected.
        /// </summary>
        public uint[] OffendingElement
        {
            get { return (uint[])_metaInfo[DicomTags.OffendingElement].Values; }
            set { _metaInfo[DicomTags.OffendingElement].Values = value; }
        }
        /// <summary>
        /// This field contains an application-specific text description of the error detected.
        /// </summary>
        public string ErrorComment
        {
            get { return _metaInfo[DicomTags.ErrorComment].GetString(0,String.Empty); }
            set { _metaInfo[DicomTags.ErrorComment].Values = value; }
        }
        /// <summary>
        /// This field shall optionally contain an application-specific error code.
        /// </summary>
        public ushort ErrorId
        {
            get { return _metaInfo[DicomTags.ErrorId].GetUInt16(0, 0); }
            set { _metaInfo[DicomTags.ErrorId].Values = value; }
        }
        /// <summary>
        /// Contains the UID of the SOP Instance for which this operation occurred.
        /// </summary>
        public string AffectedSopInstanceUid
        {
            get { return _metaInfo[DicomTags.AffectedSopInstanceUid].GetString(0,String.Empty); }
            set { _metaInfo[DicomTags.AffectedSopInstanceUid].Values = value; }
        }
        /// <summary>
        /// Contains the UID of the SOP Instance for which this operation occurred.
        /// </summary>
        public string RequestedSopInstanceUid
        {
            get { return _metaInfo[DicomTags.RequestedSopInstanceUid].GetString(0,String.Empty); }
            set { _metaInfo[DicomTags.RequestedSopInstanceUid].Values = value; }
        }
        /// <summary>
        /// Values for this field are application-specific.
        /// </summary>
        public ushort EventTypeId
        {
            get { return _metaInfo[DicomTags.EventTypeId].GetUInt16(0, 0); }
            set { _metaInfo[DicomTags.EventTypeId].Values = value; }
        }
        /// <summary>
        /// This field contains an Attribute Tag for each of the n Attributes applicable.
        /// </summary>
        public uint[] AttributeIdentifierList
        {
            get { return (uint[])_metaInfo[DicomTags.AttributeIdentifierList].Values; }
            set { _metaInfo[DicomTags.AttributeIdentifierList].Values = value; }
        }
        /// <summary>
        /// Values for this field are application-specific.
        /// </summary>
        public ushort ActionTypeId
        {
            get { return _metaInfo[DicomTags.ActionTypeId].GetUInt16(0, 0); }
            set { _metaInfo[DicomTags.ActionTypeId].Values = value; }
        }
        /// <summary>
        /// The number of reamining C-STORE sub-operations to be 
        /// invoked for the operation.
        /// </summary>
        public ushort NumberOfRemainingSubOperations
        {
            get { return _metaInfo[DicomTags.NumberOfRemainingSubOperations].GetUInt16(0, 0); }
            set { _metaInfo[DicomTags.NumberOfRemainingSubOperations].Values = value; }
        }
        /// <summary>
        /// The number of C-STORE sub-operations associated with this operation which have 
        /// completed successfully.
        /// </summary>
        public ushort NumberOfCompletedSubOperations
        {
            get { return _metaInfo[DicomTags.NumberOfCompletedSubOperations].GetUInt16(0, 0); }
            set { _metaInfo[DicomTags.NumberOfCompletedSubOperations].Values = value; }
        }
        /// <summary>
        /// The number of C-STORE sub-operations associated with this operation which
        /// have failed.
        /// </summary>
        public ushort NumberOfFailedSubOperations
        {
            get { return _metaInfo[DicomTags.NumberOfFailedSubOperations].GetUInt16(0, 0); }
            set { _metaInfo[DicomTags.NumberOfFailedSubOperations].Values = value; }
        }
        /// <summary>
        /// The number of C-STORE sub-operations associated with this operation which 
        /// generated warning responses.
        /// </summary>
        public ushort NumberOfWarningSubOperations
        {
            get { return _metaInfo[DicomTags.NumberOfWarningSubOperations].GetUInt16(0, 0); }
            set { _metaInfo[DicomTags.NumberOfWarningSubOperations].Values = value; }
        }
        /// <summary>
        /// Contains the DICOM AE Title of the DICOM AE which invoked the C-MOVE operation from which this
        /// C-STORE sub-operation is being performed.
        /// </summary>
        public string MoveOriginatorApplicationEntityTitle
        {
            get { return _metaInfo[DicomTags.MoveOriginatorApplicationEntityTitle].GetString(0,String.Empty); }
            set { _metaInfo[DicomTags.MoveOriginatorApplicationEntityTitle].Values = value; }
        }
        /// <summary>
        /// Contains the Message ID (0000,0110) of the C-MOVE-RQ Message from which this
        /// C-STORE sub-operations is being performed.
        /// </summary>
        public ushort MoveOriginatorMessageId
        {
            get { return _metaInfo[DicomTags.MoveOriginatorMessageId].GetUInt16(0, 0); }
            set { _metaInfo[DicomTags.MoveOriginatorMessageId].Values = value; }
        }

        #endregion

        #region Public Properties
        /// <summary>
        /// A <see cref="DicomAttributeCollection"/> instance representing the group 0x000 elements within the message.
        /// </summary>
        public DicomAttributeCollection CommandSet
        {
            get { return MetaInfo; }
        }

        /// <summary>
        /// The <see cref="SopClass"/> associated with the message.
        /// </summary>
        /// <remarks>If the SOP Clas is unknown, an new SopClass instance is
        /// returned with the SOP Class UID set appropriately.</remarks>
        public SopClass SopClass
        {
            get
            {
                String sopClassUid = base.DataSet[DicomTags.SopClassUid].GetString(0, String.Empty);

                SopClass sop = SopClass.GetSopClass(sopClassUid);

                if (sop == null)
                    sop = new SopClass("Unknown Sop Class", sopClassUid, false);

                return sop;
            }
        }

        public override TransferSyntax TransferSyntax
        {
            get { return _transferSyntax; }
            set { _transferSyntax = value; }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Constructor for creating a new DicomMessage instance from existing command and data sets.
        /// </summary>
        /// <param name="command">The command set.</param>
        /// <param name="data">The data set.</param>
        public DicomMessage(DicomAttributeCollection command, DicomAttributeCollection data)
        {
            if (command == null)
                _metaInfo = new DicomAttributeCollection(0x00000000,0x0000FFFF);
            else
                _metaInfo = command;

            if (data == null)
                _dataSet = new DicomAttributeCollection(0x00040000,0xFFFFFFFF);
            else
                _dataSet = data;
        }

        /// <summary>
        /// Creates a new DicomMessage instance from an existing <see cref="DicomFile"/>.
        /// </summary>
        /// <remarks>
        /// This method creates a new command set for the DicomMessage, but shares the DataSet with <paramref name="file"/>.
        /// </remarks>
        /// <param name="file">The <see cref="DicomFile"/> to change into a DicomMessage.</param>
        public DicomMessage(DicomFile file)
        {
            _transferSyntax = file.TransferSyntax;
            _metaInfo = new DicomAttributeCollection(0x00000000,0x0000FFFF);
            _dataSet = file.DataSet;
        }

        /// <summary>
        /// Default constructor that creates an empty message.
        /// </summary>
        public DicomMessage()
        {
            _metaInfo = new DicomAttributeCollection(0x00000000, 0x0000FFFF);
            _dataSet = new DicomAttributeCollection(0x00040000, 0xFFFFFFFF);
        }
        #endregion

        #region Dump
        /// <summary>
        /// Dump the contents of the message to a StringBuilder.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="prefix"></param>
        /// <param name="options"></param>
        public override void Dump(StringBuilder sb, string prefix, DicomDumpOptions options)
        {
            if (sb == null) throw new NullReferenceException("sb");
            sb.Append(prefix).Append("Command Elements:").AppendLine();
            _metaInfo.Dump(sb, prefix, options);
            sb.AppendLine().Append(prefix).Append("Data Set:").AppendLine();
            _dataSet.Dump(sb, prefix, options);
            sb.AppendLine();
        }
        #endregion
    }
}
