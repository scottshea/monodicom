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

#if _DEBUG
using namespace System;
using namespace NUnit::Framework;
using namespace ClearCanvas::Common;
using namespace ClearCanvas::Dicom;
using namespace ClearCanvas::Dicom::Codec;
using namespace ClearCanvas::Dicom::Codec::Tests;

#include "DicomJpegCodecFactory.h"

namespace ClearCanvas {
namespace Dicom {
namespace Codec {
namespace Jpeg {

public ref class StubExtensionFactory : IExtensionFactory
{
public:

virtual array<Object^>^ CreateExtensions(ExtensionPoint^ extensionPoint, ExtensionFilter^ filter, bool justOne);

virtual array<ExtensionInfo^>^ ListExtensions(ExtensionPoint^ extensionPoint, ExtensionFilter^ filter);

};

[NUnit::Framework::TestFixture]
public ref class DicomJpegCodecTest : AbstractCodecTest
{
public:

	[NUnit::Framework::TestFixtureSetUp]
	void DicomJpegCodecTest::Init();

	[NUnit::Framework::Test]
	void DicomJpegCodecTest::DicomJpegProcess1CodecTest();

	[NUnit::Framework::Test]
	void DicomJpegCodecTest::DicomJpegProcess24CodecTest();

	[NUnit::Framework::Test]
	void DicomJpegCodecTest::DicomJpegLossless14CodecTest();

	[NUnit::Framework::Test]
	void DicomJpegCodecTest::DicomJpegLossless14SV1CodecTest();

};

}
}
}
}

#endif
