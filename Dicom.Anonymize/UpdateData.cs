using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

using Dicom;
using Dicom.Data;

namespace Monodicom.Utilities
{
    public class UpdateData
    {
        private Dictionary<DicomTag, string> _dataset = new Dictionary<DicomTag, string>();
        private Dictionary<DicomTag, string> _metadata = new Dictionary<DicomTag, string>();
        private string _filepath;
        private string _filename;

        public Dictionary<DicomTag, string> UpdateDataset
        {
            get { return _dataset; }
            set { _dataset = value; }
        }

        public Dictionary<DicomTag, string> UpdateMetadata
        {
            get { return _metadata; }
            set { _metadata = value; }
        }

        public string DicomFilePath
        {
            get { return _filepath; }
            set { _filepath = value; }
        }

        public string DicomFileName
        {
            get { return _filename; }
            set
            {
                _filename = value;
            }
        }
    }
}


