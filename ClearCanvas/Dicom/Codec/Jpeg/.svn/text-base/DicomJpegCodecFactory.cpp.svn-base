#include "DicomJpegCodecFactory.h"
#include "DicomJpegCodec.h"

using namespace System;
using namespace System::IO;
using namespace System::Xml;

using namespace ClearCanvas::Dicom::Codec;
using namespace ClearCanvas::Dicom;

#include "DicomJpegParameters.h"

namespace ClearCanvas {
namespace Dicom {
namespace Codec {
namespace Jpeg {
	
	//DicomJpegProcess1CodecFactory
	ClearCanvas::Dicom::TransferSyntax^ DicomJpegProcess1CodecFactory::CodecTransferSyntax::get()  {
		return ClearCanvas::Dicom::TransferSyntax::JpegBaselineProcess1;
	}
	String^ DicomJpegProcess1CodecFactory::Name::get()  {
		return ClearCanvas::Dicom::TransferSyntax::JpegBaselineProcess1->Name;	
	}
	DicomCodecParameters^ DicomJpegProcess1CodecFactory::GetCodecParameters(DicomAttributeCollection^ dataSet) 
	{
		DicomJpegParameters^ codecParms = gcnew DicomJpegParameters();
		codecParms->ConvertPaletteToRGB = true;
		return codecParms;
	}
	DicomCodecParameters^ DicomJpegProcess1CodecFactory::GetCodecParameters(XmlDocument^ parms)
    {
		DicomJpegParameters^ codecParms = gcnew DicomJpegParameters();

		XmlElement^ element = parms->DocumentElement;

		String^ qualityString = element->Attributes["quality"]->Value;
		int quality;
		if (false == int::TryParse(qualityString, quality))
			throw gcnew ApplicationException("Invalid quality specified for JPEG Process 1: " + qualityString);

		codecParms->Quality = quality;
		codecParms->ConvertYBRtoRGB = true;

		if (element->Attributes["convertFromPalette"])
		{
			String^ boolString = element->Attributes["convertFromPalette"]->Value;
			bool convert;
			if (false == bool::TryParse(boolString, convert))
				throw gcnew ApplicationException("Invalid compressor thread count specified for JPEG Process 1: " + boolString);
			codecParms->ConvertPaletteToRGB = convert;
		}
		else
			codecParms->ConvertPaletteToRGB = true;

		return codecParms;
	}

	IDicomCodec^ DicomJpegProcess1CodecFactory::GetDicomCodec() {
		return gcnew DicomJpegProcess1Codec();
	}

	//DicomJpegProcess24CodecFactory
    TransferSyntax^ DicomJpegProcess24CodecFactory::CodecTransferSyntax::get()  {
		return TransferSyntax::JpegExtendedProcess24;
	}
	String^ DicomJpegProcess24CodecFactory::Name::get()  {
		return ClearCanvas::Dicom::TransferSyntax::JpegExtendedProcess24->Name;	
	}
	DicomCodecParameters^ DicomJpegProcess24CodecFactory::GetCodecParameters(DicomAttributeCollection^ dataSet) {
		DicomJpegParameters^ codecParms = gcnew DicomJpegParameters();
		codecParms->ConvertPaletteToRGB = true;
		return codecParms;
	}
	DicomCodecParameters^ DicomJpegProcess24CodecFactory::GetCodecParameters(XmlDocument^ parms)
    {
		DicomJpegParameters^ codecParms = gcnew DicomJpegParameters();

		XmlElement^ element = parms->DocumentElement;

		String^ qualityString = element->Attributes["quality"]->Value;
		int quality;
		if (false == int::TryParse(qualityString, quality))
			throw gcnew ApplicationException("Invalid quality specified for JPEG Process 24: " + qualityString);

		codecParms->Quality = quality;

		if (element->Attributes["convertFromPalette"])
		{
			String^ boolString = element->Attributes["convertFromPalette"]->Value;
			bool convert;
			if (false == bool::TryParse(boolString, convert))
				throw gcnew ApplicationException("Invalid convertFromPalette specified for JPEG Process 24: " + boolString);
			codecParms->ConvertPaletteToRGB = convert;
		}
		else
			codecParms->ConvertPaletteToRGB = true;

		return codecParms;
	}
	IDicomCodec^ DicomJpegProcess24CodecFactory::GetDicomCodec() {
		return gcnew DicomJpegProcess24Codec();
	}

	//DicomJpegLossless14CodecFactory
	TransferSyntax^ DicomJpegLossless14CodecFactory::CodecTransferSyntax::get()  {
		return TransferSyntax::JpegLosslessNonHierarchicalProcess14;
	}
	String^ DicomJpegLossless14CodecFactory::Name::get()  {
		return ClearCanvas::Dicom::TransferSyntax::JpegLosslessNonHierarchicalProcess14->Name;	
	}
	DicomCodecParameters^ DicomJpegLossless14CodecFactory::GetCodecParameters(DicomAttributeCollection^ dataSet)
	{
		DicomJpegParameters^ codecParms = gcnew DicomJpegParameters();

		codecParms->Quality = 1;
		codecParms->ConvertYBRtoRGB = false;
		codecParms->ConvertPaletteToRGB = true;

		return codecParms;
	}
	DicomCodecParameters^ DicomJpegLossless14CodecFactory::GetCodecParameters(XmlDocument^ parms)
    {
		DicomJpegParameters^ codecParms = gcnew DicomJpegParameters();

		codecParms->Quality = 1;
		codecParms->ConvertYBRtoRGB = false;

		XmlElement^ element = parms->DocumentElement;
		if (element->Attributes["convertFromPalette"])
		{
			String^ boolString = element->Attributes["convertFromPalette"]->Value;
			bool convert;
			if (false == bool::TryParse(boolString, convert))
				throw gcnew ApplicationException("Invalid convertFromPalette specified for JPEG Lossless 14: " + boolString);
			codecParms->ConvertPaletteToRGB = convert;
		}
		else
			codecParms->ConvertPaletteToRGB = true;

		return codecParms;
	}
	IDicomCodec^ DicomJpegLossless14CodecFactory::GetDicomCodec() {
		return gcnew DicomJpegLossless14Codec();
	}

	//DicomJpegLossless14SV1CodecFactory
    ClearCanvas::Dicom::TransferSyntax^ DicomJpegLossless14SV1CodecFactory::CodecTransferSyntax::get()  {
		return ClearCanvas::Dicom::TransferSyntax::JpegLosslessNonHierarchicalFirstOrderPredictionProcess14SelectionValue1;
	}
	String^ DicomJpegLossless14SV1CodecFactory::Name::get()  {
		return ClearCanvas::Dicom::TransferSyntax::JpegLosslessNonHierarchicalFirstOrderPredictionProcess14SelectionValue1->Name;	
	}
	DicomCodecParameters^ DicomJpegLossless14SV1CodecFactory::GetCodecParameters(DicomAttributeCollection^ dataSet)
	{
		DicomJpegParameters^ codecParms = gcnew DicomJpegParameters();

		codecParms->Quality = 1;
		codecParms->ConvertYBRtoRGB = false;
		codecParms->ConvertPaletteToRGB = true;
		
		return codecParms;
	}
	DicomCodecParameters^ DicomJpegLossless14SV1CodecFactory::GetCodecParameters(XmlDocument^ parms)
    {
		DicomJpegParameters^ codecParms = gcnew DicomJpegParameters();

		codecParms->Quality = 1;
		codecParms->ConvertYBRtoRGB = false;

		XmlElement^ element = parms->DocumentElement;
		if (element->Attributes["convertFromPalette"])
		{
			String^ boolString = element->Attributes["convertFromPalette"]->Value;
			bool convert;
			if (false == bool::TryParse(boolString, convert))
				throw gcnew ApplicationException("Invalid convertFromPalette specified for JPEG Lossless SV1: " + boolString);
			codecParms->ConvertPaletteToRGB = convert;
		}
		else
			codecParms->ConvertPaletteToRGB = true;

		return codecParms;
	}
	IDicomCodec^ DicomJpegLossless14SV1CodecFactory::GetDicomCodec() {
		return gcnew DicomJpegLossless14SV1Codec();
	}


} // Jpeg
} // Codec
} // Dicom
} // ClearCanvas