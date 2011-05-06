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

#ifdef _DEBUG

#include "DicomJpegCodecTest.h"

using namespace System;
using namespace NUnit::Framework;
using namespace ClearCanvas::Common;
using namespace ClearCanvas::Dicom;
using namespace ClearCanvas::Dicom::Codec;
using namespace ClearCanvas::Dicom::Codec::Tests;

namespace ClearCanvas {
namespace Dicom {
namespace Codec {
namespace Jpeg {


array<Object^>^ StubExtensionFactory::CreateExtensions(ExtensionPoint^ extensionPoint, ExtensionFilter^ filter, bool justOne)
{
	if (extensionPoint->GetType() != DicomCodecFactoryExtensionPoint::typeid)
		return gcnew array<Object^>(0);

	array<Object^>^ theArray = gcnew array<Object^>(4);
	theArray[0] = gcnew DicomJpegProcess1CodecFactory;
	theArray[1] = gcnew DicomJpegProcess24CodecFactory;
	theArray[2] = gcnew DicomJpegLossless14CodecFactory;
	theArray[3] = gcnew DicomJpegLossless14SV1CodecFactory;
	return theArray;
}

array<ExtensionInfo^>^ StubExtensionFactory::ListExtensions(ExtensionPoint^ extensionPoint, ExtensionFilter^ filter)
{
	return gcnew array<ExtensionInfo^>(0);
}

void DicomJpegCodecTest::Init()
{
	Platform::SetExtensionFactory(gcnew StubExtensionFactory());

	//HACK: for now, call the static constructor again, so it will repopulate the dictionary
	System::Reflection::ConstructorInfo^ staticConstructor = DicomCodecRegistry::typeid->TypeInitializer;
	staticConstructor->Invoke(nullptr, nullptr);
}

void DicomJpegCodecTest::DicomJpegProcess1CodecTest()
{
	TransferSyntax^ syntax = TransferSyntax::JpegBaselineProcess1;

	DicomFile^ file = CreateFile(255, 255, "MONOCHROME1", 8, 8, false, 1);
	LossyImageTest(syntax, file);

	file = CreateFile(255, 255, "MONOCHROME2", 8, 8, true, 1);
	LossyImageTest(syntax, file);

	file = CreateFile(255, 255, "MONOCHROME1", 8, 8, true, 5);
	LossyImageTest(syntax, file);

	file = CreateFile(255, 255, "MONOCHROME2", 8, 8, false, 5);
	LossyImageTest(syntax, file);

	file = CreateFile(255, 255, "RGB", 8, 8, false, 1);
	LossyImageTest(syntax, file);

	file = CreateFile(255, 255, "RGB", 8, 8, false, 5);
	LossyImageTest(syntax, file);

	file = CreateFile(512, 512, "RGB", 8, 8, false, 1);
	LossyImageTest(syntax, file);

	file = CreateFile(512, 512, "RGB", 8, 8, false, 5);
	LossyImageTest(syntax, file);

	file = CreateFile(512, 512, "YBR_FULL", 8, 8, false, 1);
	LossyImageTest(syntax, file);

	file = CreateFile(512, 512, "YBR_FULL", 8, 8, false, 2);
	LossyImageTest(syntax, file);

	file = CreateFile(255, 255, "MONOCHROME2", 12, 16, false, 1);
	ExpectedFailureTest(syntax, file);
}

void DicomJpegCodecTest::DicomJpegProcess24CodecTest()
{
	TransferSyntax^ syntax = TransferSyntax::JpegExtendedProcess24;
	DicomFile^ file = CreateFile(512, 512, "MONOCHROME1", 12, 16, false, 1);
	LossyImageTest(syntax, file);

	file = CreateFile(512, 512, "MONOCHROME1", 12, 16, true, 1);
	LossyImageTest(syntax, file);

	file = CreateFile(255, 255, "MONOCHROME1", 8, 8, false, 1);
	LossyImageTest(syntax, file);

	file = CreateFile(255, 255, "MONOCHROME1", 8, 8, true, 1);
	LossyImageTest(syntax, file);

	file = CreateFile(256, 255, "MONOCHROME2", 12, 16, true, 1);
	LossyImageTest(syntax, file);

	file = CreateFile(256, 256, "MONOCHROME1", 12, 16, true, 5);
	LossyImageTest(syntax, file);

	file = CreateFile(255, 255, "MONOCHROME1", 8, 8, true, 5);
	LossyImageTest(syntax, file);

	file = CreateFile(255, 255, "MONOCHROME1", 12, 16, true, 3);
	LossyImageTest(syntax, file);

	file = CreateFile(255, 255, "RGB", 8, 8, false, 1);
	LossyImageTest(syntax, file);

	file = CreateFile(255, 255, "RGB", 8, 8, false, 5);
	LossyImageTest(syntax, file);

	file = CreateFile(512, 512, "RGB", 8, 8, false, 1);
	LossyImageTest(syntax, file);

	file = CreateFile(512, 512, "RGB", 8, 8, false, 5);
	LossyImageTest(syntax, file);

	file = CreateFile(512, 512, "YBR_FULL", 8, 8, false, 1);
	LossyImageTest(syntax, file);

	file = CreateFile(255, 255, "YBR_FULL", 8, 8, false, 5);
	LossyImageTest(syntax, file);

	file = CreateFile(255, 255, "MONOCHROME2", 16, 16, false, 1);
	ExpectedFailureTest(syntax, file);

	file = CreateFile(255, 255, "MONOCHROME2", 15, 16, false, 1);
	ExpectedFailureTest(syntax, file);

	file = CreateFile(255, 255, "MONOCHROME2", 14, 16, false, 1);
	ExpectedFailureTest(syntax, file);

	file = CreateFile(255, 255, "MONOCHROME2", 13, 16, false, 1);
	ExpectedFailureTest(syntax, file);

	file = CreateFile(255, 255, "MONOCHROME2", 11, 16, false, 1);
	ExpectedFailureTest(syntax, file);

	file = CreateFile(255, 255, "MONOCHROME2", 10, 16, false, 1);
	ExpectedFailureTest(syntax, file);

	file = CreateFile(255, 255, "MONOCHROME2", 9, 16, false, 1);
	ExpectedFailureTest(syntax, file);

}

void DicomJpegCodecTest::DicomJpegLossless14CodecTest()
{
	TransferSyntax^ syntax = TransferSyntax::JpegLosslessNonHierarchicalProcess14;
	DicomFile^ file = CreateFile(512, 512, "MONOCHROME1", 12, 16, false, 1);
	LosslessImageTest(syntax, file);

	// Lossy test for signed, becaue it converts
	file = CreateFile(512, 512, "MONOCHROME1", 12, 16, true, 1);
	LosslessImageTestWithConversion(syntax, file);

	file = CreateFile(255, 255, "MONOCHROME1", 8, 8, false, 1);
	LosslessImageTest(syntax, file);

	file = CreateFile(255, 255, "MONOCHROME1", 8, 8, true, 1);
	LosslessImageTestWithConversion(syntax, file);

	file = CreateFile(256, 255, "MONOCHROME2", 16, 16, false, 1);
	LosslessImageTest(syntax, file);

	file = CreateFile(256, 255, "MONOCHROME2", 16, 16, true, 1);
	LosslessImageTestWithConversion(syntax, file);

	file = CreateFile(256, 255, "MONOCHROME2", 15, 16, false, 1);
	LosslessImageTest(syntax, file);

	file = CreateFile(256, 255, "MONOCHROME2", 14, 16, false, 1);
	LosslessImageTest(syntax, file);

	file = CreateFile(256, 255, "MONOCHROME2", 13, 16, false, 1);
	LosslessImageTest(syntax, file);

	file = CreateFile(256, 256, "MONOCHROME1", 12, 16, false, 1);
	LosslessImageTest(syntax, file);

	file = CreateFile(256, 256, "MONOCHROME1", 11, 16, false, 1);
	LosslessImageTest(syntax, file);

	file = CreateFile(256, 256, "MONOCHROME1", 10, 16, false, 1);
	LosslessImageTest(syntax, file);

	file = CreateFile(256, 256, "MONOCHROME1", 9, 16, false, 1);
	LosslessImageTest(syntax, file);

	file = CreateFile(256, 256, "MONOCHROME1", 8, 16, false, 1);
	LosslessImageTestWithConversion(syntax, file);

	file = CreateFile(255, 255, "MONOCHROME1", 8, 8, false, 5);
	LosslessImageTest(syntax, file);

	file = CreateFile(255, 255, "RGB", 8, 8, false, 1);
	LosslessImageTest(syntax, file);

	file = CreateFile(255, 255, "RGB", 8, 8, false, 5);
	LosslessImageTest(syntax, file);

	file = CreateFile(512, 512, "RGB", 8, 8, false, 1);
	LosslessImageTest(syntax, file);

	file = CreateFile(512, 512, "RGB", 8, 8, false, 5);
	LosslessImageTest(syntax, file);

	file = CreateFile(512, 512, "YBR_FULL", 8, 8, false, 1);
	LosslessImageTest(syntax, file);

	file = CreateFile(255, 255, "YBR_FULL", 8, 8, false, 5);
	LosslessImageTest(syntax, file);

}

void DicomJpegCodecTest::DicomJpegLossless14SV1CodecTest()
{
	TransferSyntax^ syntax = TransferSyntax::JpegLosslessNonHierarchicalFirstOrderPredictionProcess14SelectionValue1;
	DicomFile^ file = CreateFile(512, 512, "MONOCHROME1", 12, 16, false, 1);
	LosslessImageTest(syntax, file);

	// Lossy test for signed, becaue it converts
	file = CreateFile(512, 512, "MONOCHROME1", 12, 16, true, 1);
	LosslessImageTestWithConversion(syntax, file);

	file = CreateFile(255, 255, "MONOCHROME1", 8, 8, false, 1);
	LosslessImageTest(syntax, file);

	file = CreateFile(255, 255, "MONOCHROME1", 8, 8, true, 1);
	LosslessImageTestWithConversion(syntax, file);

	file = CreateFile(256, 255, "MONOCHROME2", 16, 16, false, 1);
	LosslessImageTest(syntax, file);

	file = CreateFile(256, 255, "MONOCHROME2", 16, 16, true, 1);
	LosslessImageTestWithConversion(syntax, file);

	file = CreateFile(256, 255, "MONOCHROME2", 15, 16, false, 1);
	LosslessImageTest(syntax, file);

	file = CreateFile(256, 255, "MONOCHROME2", 14, 16, false, 1);
	LosslessImageTest(syntax, file);

	file = CreateFile(256, 255, "MONOCHROME2", 13, 16, false, 1);
	LosslessImageTest(syntax, file);

	file = CreateFile(256, 256, "MONOCHROME1", 12, 16, false, 1);
	LosslessImageTest(syntax, file);

	file = CreateFile(256, 256, "MONOCHROME1", 11, 16, false, 1);
	LosslessImageTest(syntax, file);

	file = CreateFile(256, 256, "MONOCHROME1", 10, 16, false, 1);
	LosslessImageTest(syntax, file);

	file = CreateFile(256, 256, "MONOCHROME1", 9, 16, false, 1);
	LosslessImageTest(syntax, file);

	file = CreateFile(256, 256, "MONOCHROME1", 8, 16, false, 1);
	LosslessImageTestWithConversion(syntax, file);

	file = CreateFile(255, 255, "MONOCHROME1", 8, 8, false, 5);
	LosslessImageTest(syntax, file);

	file = CreateFile(255, 255, "RGB", 8, 8, false, 1);
	LosslessImageTest(syntax, file);

	file = CreateFile(255, 255, "RGB", 8, 8, false, 5);
	LosslessImageTest(syntax, file);

	file = CreateFile(512, 512, "RGB", 8, 8, false, 1);
	LosslessImageTest(syntax, file);

	file = CreateFile(512, 512, "RGB", 8, 8, false, 5);
	LosslessImageTest(syntax, file);

	file = CreateFile(512, 512, "YBR_FULL", 8, 8, false, 1);
	LosslessImageTest(syntax, file);

	file = CreateFile(255, 255, "YBR_FULL", 8, 8, false, 5);
	LosslessImageTest(syntax, file);
}


}
}
}
}

#endif
