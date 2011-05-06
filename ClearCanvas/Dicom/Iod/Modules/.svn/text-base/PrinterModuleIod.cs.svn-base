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
using ClearCanvas.Dicom.Utilities;

namespace ClearCanvas.Dicom.Iod.Modules
{
    /// <summary>
    /// Printer Module as per Part 3 Table C.13-9, page 872
    /// </summary>
    public class PrinterModuleIod : IodBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PrinterModuleIod"/> class.
        /// </summary>
        public PrinterModuleIod()
            :base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrinterModuleIod"/> class.
        /// </summary>
		public PrinterModuleIod(IDicomAttributeProvider dicomAttributeProvider) : base(dicomAttributeProvider)
        {
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the printer status.
        /// </summary>
        /// <value>The printer status.</value>
        public PrinterStatus PrinterStatus
        {
            get { return IodBase.ParseEnum<PrinterStatus>(base.DicomAttributeProvider[DicomTags.PrinterStatus].GetString(0, String.Empty), PrinterStatus.None); }
            set { IodBase.SetAttributeFromEnum(base.DicomAttributeProvider[DicomTags.PrinterStatus], value, false); }
        }

        /// <summary>
        /// Gets or sets the printer status info.
        /// </summary>
        /// <value>The printer status info.</value>
        public string PrinterStatusInfo
        {
            get { return base.DicomAttributeProvider[DicomTags.PrinterStatusInfo].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.PrinterStatusInfo].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the name of the printer.
        /// </summary>
        /// <value>The name of the printer.</value>
        public string PrinterName
        {
            get { return base.DicomAttributeProvider[DicomTags.PrinterName].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.PrinterName].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the manufacturer.
        /// </summary>
        /// <value>The manufacturer.</value>
        public string Manufacturer
        {
            get { return base.DicomAttributeProvider[DicomTags.Manufacturer].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.Manufacturer].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the name of the manufacturers model.
        /// </summary>
        /// <value>The name of the manufacturers model.</value>
        public string ManufacturersModelName
        {
            get { return base.DicomAttributeProvider[DicomTags.ManufacturersModelName].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.ManufacturersModelName].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the device serial number.
        /// </summary>
        /// <value>The device serial number.</value>
        public string DeviceSerialNumber
        {
            get { return base.DicomAttributeProvider[DicomTags.DeviceSerialNumber].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.DeviceSerialNumber].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the software versions.
        /// </summary>
        /// <value>The software versions.</value>
        public string SoftwareVersions
        {
            get { return base.DicomAttributeProvider[DicomTags.SoftwareVersions].GetString(0, String.Empty); }
            set { base.DicomAttributeProvider[DicomTags.SoftwareVersions].SetString(0, value); }
        }

        /// <summary>
        /// Gets or sets the date of last calibration.
        /// </summary>
        /// <value>The date of last calibration.</value>
        public DateTime? DateOfLastCalibration
        {
        	get { return DateTimeParser.ParseDateAndTime(String.Empty, 
        					base.DicomAttributeProvider[DicomTags.DateOfLastCalibration].GetString(0, String.Empty), 
                  base.DicomAttributeProvider[DicomTags.TimeOfLastCalibration].GetString(0, String.Empty)); }

                  set { DateTimeParser.SetDateTimeAttributeValues(value, base.DicomAttributeProvider[DicomTags.DateOfLastCalibration], base.DicomAttributeProvider[DicomTags.TimeOfLastCalibration]); }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Sets the commonly used tags in the base dicom attribute collection.
        /// </summary>
        public void SetCommonTags()
        {
            SetCommonTags(base.DicomAttributeProvider);
        }
        #endregion

        #region Public Static Methods
        /// <summary>
        /// Sets the commonly used tags in the specified dicom attribute collection.
        /// </summary>
        public static void SetCommonTags(IDicomAttributeProvider dicomAttributeProvider)
        {
            if (dicomAttributeProvider == null)
				throw new ArgumentNullException("dicomAttributeProvider");

            dicomAttributeProvider[DicomTags.PrinterStatus].SetNullValue();
            dicomAttributeProvider[DicomTags.PrinterStatusInfo].SetNullValue();
            dicomAttributeProvider[DicomTags.PrinterName].SetNullValue();
            dicomAttributeProvider[DicomTags.Manufacturer].SetNullValue();
            dicomAttributeProvider[DicomTags.ManufacturersModelName].SetNullValue();
            dicomAttributeProvider[DicomTags.DeviceSerialNumber].SetNullValue();
            dicomAttributeProvider[DicomTags.SoftwareVersions].SetNullValue();
            dicomAttributeProvider[DicomTags.DateOfLastCalibration].SetNullValue();
            dicomAttributeProvider[DicomTags.TimeOfLastCalibration].SetNullValue();

        }
        #endregion
    }

    #region PrinterStatus Enum
    /// <summary>
    /// Enumeration for Printer Status
    /// </summary>
    public enum PrinterStatus
    {
        /// <summary>
        /// 
        /// </summary>
        None,
        /// <summary>
        /// 
        /// </summary>
        Normal,
        /// <summary>
        /// 
        /// </summary>
        Warning,
        /// <summary>
        /// 
        /// </summary>
        Failure
    }
    #endregion
}
