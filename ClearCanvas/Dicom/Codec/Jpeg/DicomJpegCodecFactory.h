// Copyright (c) 2009, ClearCanvas Inc.
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

#pragma once

using namespace System;
using namespace System::IO;
using namespace System::Xml;

using namespace ClearCanvas::Dicom;
using namespace ClearCanvas::Dicom::Codec;

#include "DicomJpegCodec.h"
#include "DicomJpegParameters.h"

namespace ClearCanvas {
namespace Dicom {
namespace Codec {
namespace Jpeg {

[ClearCanvas::Common::ExtensionOf(DicomCodecFactoryExtensionPoint::typeid)]
public ref class DicomJpegProcess1CodecFactory : public IDicomCodecFactory {
public:
    virtual property String^ Name { String^ get() ;}
    virtual property ClearCanvas::Dicom::TransferSyntax^ CodecTransferSyntax {  ClearCanvas::Dicom::TransferSyntax^ get(); };

    virtual DicomCodecParameters^ GetCodecParameters(DicomAttributeCollection^ dataSet);
	virtual DicomCodecParameters^ GetCodecParameters(XmlDocument^ parms);
    virtual IDicomCodec^ GetDicomCodec();
};

[ClearCanvas::Common::ExtensionOf(DicomCodecFactoryExtensionPoint::typeid)]
public ref class DicomJpegProcess24CodecFactory : public IDicomCodecFactory {
public:
    virtual property String^ Name { String^ get();}
    virtual property ClearCanvas::Dicom::TransferSyntax^ CodecTransferSyntax { ClearCanvas::Dicom::TransferSyntax^ get(); };

    virtual DicomCodecParameters^ GetCodecParameters(DicomAttributeCollection^ dataSet);
	virtual DicomCodecParameters^ GetCodecParameters(XmlDocument^ parms);
	virtual IDicomCodec^ GetDicomCodec();
};

[ClearCanvas::Common::ExtensionOf(DicomCodecFactoryExtensionPoint::typeid)]
public ref class DicomJpegLossless14CodecFactory : public IDicomCodecFactory {
public:
    virtual property String^ Name { String^ get();}
    virtual property ClearCanvas::Dicom::TransferSyntax^ CodecTransferSyntax { ClearCanvas::Dicom::TransferSyntax^ get(); };

    virtual DicomCodecParameters^ GetCodecParameters(DicomAttributeCollection^ dataSet);
    virtual DicomCodecParameters^ GetCodecParameters(XmlDocument^ parms);
	virtual IDicomCodec^ GetDicomCodec();
};

[ClearCanvas::Common::ExtensionOf(DicomCodecFactoryExtensionPoint::typeid)]
public ref class DicomJpegLossless14SV1CodecFactory : public IDicomCodecFactory {
public:
    virtual property String^ Name { String^ get();}
    virtual property ClearCanvas::Dicom::TransferSyntax^ CodecTransferSyntax { ClearCanvas::Dicom::TransferSyntax^ get(); };

    virtual DicomCodecParameters^ GetCodecParameters(DicomAttributeCollection^ dataSet);
    virtual DicomCodecParameters^ GetCodecParameters(XmlDocument^ parms);
	virtual IDicomCodec^ GetDicomCodec();
};

} // Jpeg
} // Codec
} // Dicom
} // ClearCanvas
