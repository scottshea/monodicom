#include "DicomJpeg2000CodecFactory.h"
#include "DicomJpeg2000Codec.h"

using namespace System;
using namespace System::IO;

using namespace ClearCanvas::Dicom::Codec;
using namespace ClearCanvas::Dicom;

namespace ClearCanvas {
namespace Dicom {
namespace Codec {
namespace Jpeg2000 {
	
	//DicomJpeg2000LosslessCodecFactory
	ClearCanvas::Dicom::TransferSyntax^ DicomJpeg2000LosslessCodecFactory::CodecTransferSyntax::get()  {
		return ClearCanvas::Dicom::TransferSyntax::Jpeg2000ImageCompressionLosslessOnly;
	}
	String^ DicomJpeg2000LosslessCodecFactory::Name::get()  {
		return ClearCanvas::Dicom::TransferSyntax::Jpeg2000ImageCompressionLosslessOnly->Name;	
	}
	DicomCodecParameters^ DicomJpeg2000LosslessCodecFactory::GetCodecParameters(DicomAttributeCollection^ dataSet) {
		return gcnew DicomJpeg2000Parameters();
	}
	DicomCodecParameters^ DicomJpeg2000LosslessCodecFactory::GetCodecParameters(XmlDocument^ parms)
    {
		DicomJpeg2000Parameters^ codecParms = gcnew DicomJpeg2000Parameters();
		codecParms->Irreversible = false;
		codecParms->UpdatePhotometricInterpretation = true;
		codecParms->Rate = 1; //1 == Lossless

		XmlElement^ element = parms->DocumentElement;
		if (element->Attributes["convertFromPalette"])
		{
			String^ boolString = element->Attributes["convertFromPalette"]->Value;
			bool convert;
			if (false == bool::TryParse(boolString, convert))
				throw gcnew ApplicationException("Invalid convertFromPalette specified for JPEG 2000 Lossless: " + boolString);
			codecParms->ConvertPaletteToRGB = convert;
		}
		else
			codecParms->ConvertPaletteToRGB = true;

		return codecParms;
	}
	IDicomCodec^ DicomJpeg2000LosslessCodecFactory::GetDicomCodec() {
		return gcnew DicomJpeg2000LosslessCodec();
	}

	//DicomJpeg2000LossyCodecFactory
	TransferSyntax^ DicomJpeg2000LossyCodecFactory::CodecTransferSyntax::get()  {
		return TransferSyntax::Jpeg2000ImageCompression;
	}

	String^ DicomJpeg2000LossyCodecFactory::Name::get()  {
		return ClearCanvas::Dicom::TransferSyntax::Jpeg2000ImageCompression->Name;	
	}

	DicomCodecParameters^ DicomJpeg2000LossyCodecFactory::GetCodecParameters(DicomAttributeCollection^ dataSet) {
		DicomJpeg2000Parameters^ codecParms = gcnew DicomJpeg2000Parameters();

		codecParms->Irreversible = true;
		codecParms->UpdatePhotometricInterpretation = true;
		codecParms->Rate = 5.0;
		return codecParms;
	}

	DicomCodecParameters^ DicomJpeg2000LossyCodecFactory::GetCodecParameters(XmlDocument^ parms)
    {
		DicomJpeg2000Parameters^ codecParms = gcnew DicomJpeg2000Parameters();

		codecParms->Irreversible = true;
		codecParms->UpdatePhotometricInterpretation = true;

		XmlElement^ element = parms->DocumentElement;

		String^ ratioString = element->Attributes["ratio"]->Value;
		float ratio;
		if (false == float::TryParse(ratioString, ratio))
			throw gcnew ApplicationException("Invalid compression ratio specified for JPEG 2000 Lossy: " + ratioString);

		codecParms->Rate = ratio;

		if (element->Attributes["convertFromPalette"])
		{
			String^ boolString = element->Attributes["convertFromPalette"]->Value;
			bool convert;
			if (false == bool::TryParse(boolString, convert))
				throw gcnew ApplicationException("Invalid convertFromPalette specified for JPEG 2000 Lossy: " + boolString);
			codecParms->ConvertPaletteToRGB = convert;
		}
		else
			codecParms->ConvertPaletteToRGB = true;

		return codecParms;
	}
	IDicomCodec^ DicomJpeg2000LossyCodecFactory::GetDicomCodec() {
		return gcnew DicomJpeg2000LossyCodec();
	}

} // Jpeg2000
} // Codec
} // Dicom
} // ClearCanvas
