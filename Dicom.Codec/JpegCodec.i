#define IJGE_BLOCKSIZE 16384

namespace IJGVERS {
	// private error handler struct
	struct ErrorStruct {
	  // the standard IJG error handler object
	  struct jpeg_error_mgr pub;
	};

	// error handler, executes longjmp
	void ErrorExit(j_common_ptr cinfo) {
		ErrorStruct *myerr = (ErrorStruct *)cinfo->err;
	}

	// message handler for warning messages and the like
	void OutputMessage(j_common_ptr cinfo) {
		ErrorStruct *myerr = (ErrorStruct *)cinfo->err;
		char buffer[JMSG_LENGTH_MAX];
		(*cinfo->err->format_message)((jpeg_common_struct *)cinfo, buffer); /* Create the message */
		Dicom::Debug::Log->Info("IJG: {0}", gcnew String(buffer));
	}
}


JPEGCODEC::JPEGCODEC(JpegMode mode, int predictor, int point_transform) {
	Mode = mode;
	Predictor = predictor;
	PointTransform = point_transform;
}

namespace IJGVERS {
	J_COLOR_SPACE getJpegColorSpace(String^ photometricInterpretation) {
		if (photometricInterpretation == "RGB")
			return JCS_RGB;
		else if (photometricInterpretation == "MONOCHROME1" || photometricInterpretation == "MONOCHROME2")
			return JCS_GRAYSCALE;
		else if (photometricInterpretation == "PALETTE COLOR")
			return JCS_UNKNOWN;
		else if (photometricInterpretation == "YBR_FULL" || photometricInterpretation == "YBR_FULL_422" || photometricInterpretation == "YBR_PARTIAL_422")
			return JCS_YCbCr;
		else
			return JCS_UNKNOWN;
	}

	// callbacks for compress-destination-manager
	void initDestination(j_compress_ptr cinfo) {
		JPEGCODEC^ thisPtr = (JPEGCODEC^)JPEGCODEC::This;
		thisPtr->MemoryBuffer = gcnew MemoryStream();
		cinfo->dest->next_output_byte = thisPtr->DataPtr;
		cinfo->dest->free_in_buffer = IJGE_BLOCKSIZE;
	}

	ijg_boolean emptyOutputBuffer(j_compress_ptr cinfo) {
		JPEGCODEC^ thisPtr = (JPEGCODEC^)JPEGCODEC::This;
		thisPtr->MemoryBuffer->Write(thisPtr->DataBuffer, 0, IJGE_BLOCKSIZE);
		cinfo->dest->next_output_byte = thisPtr->DataPtr;
		cinfo->dest->free_in_buffer = IJGE_BLOCKSIZE;
		return TRUE;
	}

	void termDestination(j_compress_ptr cinfo) {
		JPEGCODEC^ thisPtr = (JPEGCODEC^)JPEGCODEC::This;
		int count = IJGE_BLOCKSIZE - cinfo->dest->free_in_buffer;
		thisPtr->MemoryBuffer->Write(thisPtr->DataBuffer, 0, count);
		thisPtr->DataPtr = nullptr;
		thisPtr->DataBuffer = nullptr;
	}

	// Borrowed from DCMTK djeijgXX.cxx
	/*
	 * jpeg_simple_spectral_selection() creates a scan script
	 * for progressive JPEG with spectral selection only,
	 * similar to jpeg_simple_progression() for full progression.
	 * The scan sequence for YCbCr is as proposed in the IJG documentation.
	 * The scan sequence for all other color models is somewhat arbitrary.
	 */
	void jpeg_simple_spectral_selection(j_compress_ptr cinfo) {
		int ncomps = cinfo->num_components;
		jpeg_scan_info *scanptr = NULL;
		int nscans = 0;

		/* Safety check to ensure start_compress not called yet. */
		if (cinfo->global_state != CSTATE_START) ERREXIT1(cinfo, JERR_BAD_STATE, cinfo->global_state);

		if (ncomps == 3 && cinfo->jpeg_color_space == JCS_YCbCr) nscans = 7;
		else nscans = 1 + 2 * ncomps;	/* 1 DC scan; 2 AC scans per component */

		/* Allocate space for script.
		* We need to put it in the permanent pool in case the application performs
		* multiple compressions without changing the settings.  To avoid a memory
		* leak if jpeg_simple_spectral_selection is called repeatedly for the same JPEG
		* object, we try to re-use previously allocated space, and we allocate
		* enough space to handle YCbCr even if initially asked for grayscale.
		*/
		if (cinfo->script_space == NULL || cinfo->script_space_size < nscans) {
		cinfo->script_space_size = nscans > 7 ? nscans : 7;
		cinfo->script_space = (jpeg_scan_info *)
		  (*cinfo->mem->alloc_small) ((j_common_ptr) cinfo, 
		  JPOOL_PERMANENT, cinfo->script_space_size * sizeof(jpeg_scan_info));
		}
		scanptr = cinfo->script_space;
		cinfo->scan_info = scanptr;
		cinfo->num_scans = nscans;

		if (ncomps == 3 && cinfo->jpeg_color_space == JCS_YCbCr) {
			/* Custom script for YCbCr color images. */

			// Interleaved DC scan for Y,Cb,Cr:
			scanptr[0].component_index[0] = 0;
			scanptr[0].component_index[1] = 1;
			scanptr[0].component_index[2] = 2;
			scanptr[0].comps_in_scan = 3;
			scanptr[0].Ss = 0;
			scanptr[0].Se = 0;
			scanptr[0].Ah = 0;
			scanptr[0].Al = 0;

			// AC scans
			// First two Y AC coefficients
			scanptr[1].component_index[0] = 0;
			scanptr[1].comps_in_scan = 1;
			scanptr[1].Ss = 1;
			scanptr[1].Se = 2;
			scanptr[1].Ah = 0;
			scanptr[1].Al = 0;

			// Three more
			scanptr[2].component_index[0] = 0;
			scanptr[2].comps_in_scan = 1;
			scanptr[2].Ss = 3;
			scanptr[2].Se = 5;
			scanptr[2].Ah = 0;
			scanptr[2].Al = 0;

			// All AC coefficients for Cb
			scanptr[3].component_index[0] = 1;
			scanptr[3].comps_in_scan = 1;
			scanptr[3].Ss = 1;
			scanptr[3].Se = 63;
			scanptr[3].Ah = 0;
			scanptr[3].Al = 0;

			// All AC coefficients for Cr
			scanptr[4].component_index[0] = 2;
			scanptr[4].comps_in_scan = 1;
			scanptr[4].Ss = 1;
			scanptr[4].Se = 63;
			scanptr[4].Ah = 0;
			scanptr[4].Al = 0;

			// More Y coefficients
			scanptr[5].component_index[0] = 0;
			scanptr[5].comps_in_scan = 1;
			scanptr[5].Ss = 6;
			scanptr[5].Se = 9;
			scanptr[5].Ah = 0;
			scanptr[5].Al = 0;

			// Remaining Y coefficients
			scanptr[6].component_index[0] = 0;
			scanptr[6].comps_in_scan = 1;
			scanptr[6].Ss = 10;
			scanptr[6].Se = 63;
			scanptr[6].Ah = 0;
			scanptr[6].Al = 0;
		}
		else
		{
			/* All-purpose script for other color spaces. */
			int j=0;

			// Interleaved DC scan for all components
			for (j=0; j<ncomps; j++) scanptr[0].component_index[j] = j;
			scanptr[0].comps_in_scan = ncomps;
			scanptr[0].Ss = 0;
			scanptr[0].Se = 0;
			scanptr[0].Ah = 0;
			scanptr[0].Al = 0;

			// first AC scan for each component
			for (j=0; j<ncomps; j++) {
				scanptr[j+1].component_index[0] = j;
				scanptr[j+1].comps_in_scan = 1;
				scanptr[j+1].Ss = 1;
				scanptr[j+1].Se = 5;
				scanptr[j+1].Ah = 0;
				scanptr[j+1].Al = 0;
			}

			// second AC scan for each component
			for (j=0; j<ncomps; j++) {
				scanptr[j+ncomps+1].component_index[0] = j;
				scanptr[j+ncomps+1].comps_in_scan = 1;
				scanptr[j+ncomps+1].Ss = 6;
				scanptr[j+ncomps+1].Se = 63;
				scanptr[j+ncomps+1].Ah = 0;
				scanptr[j+ncomps+1].Al = 0;
			}
		}
	}
}

void JPEGCODEC::Encode(DcmPixelData^ oldPixelData, DcmPixelData^ newPixelData, DcmJpegParameters^ params, int frame) {
	if ((oldPixelData->PhotometricInterpretation == "YBR_ICT") ||
		(oldPixelData->PhotometricInterpretation == "YBR_RCT"))
		throw gcnew DicomCodecException(String::Format("Photometric Interpretation '{0}' not supported by JPEG encoder!",
														oldPixelData->PhotometricInterpretation));

	array<unsigned char>^ frameData = nullptr;
	
	if (oldPixelData->BitsAllocated == 16 && oldPixelData->BitsStored <= 8) {
		array<unsigned short>^ frameData16 = oldPixelData->GetFrameDataU16(frame);
		frameData = gcnew array<unsigned char>(frameData16->Length);
		
		for (int i = 0, l = frameData->Length; i < l; i++) {
			frameData[i] = static_cast<unsigned char>(frameData16[i]);
		}
	}
	else
		frameData = oldPixelData->GetFrameDataU8(frame);

	pin_ptr<unsigned char> framePin = &frameData[0];
	unsigned char* framePtr = framePin;
	unsigned int frameSize = frameData->Length;

	DataBuffer = gcnew array<unsigned char>(IJGE_BLOCKSIZE);
	pin_ptr<unsigned char> DataPin = &DataBuffer[0];
	DataPtr = DataPin;

	try {
		if (oldPixelData->IsPlanar && oldPixelData->SamplesPerPixel > 1) {
			newPixelData->PlanarConfiguration = 0;
			DcmCodecHelper::ChangePlanarConfiguration(frameData, frameData->Length / oldPixelData->BytesAllocated, 
				oldPixelData->BitsAllocated, oldPixelData->SamplesPerPixel, 1);
		}

		struct jpeg_compress_struct cinfo;
		struct IJGVERS::ErrorStruct jerr;
		cinfo.err = jpeg_std_error(&jerr.pub);
		jerr.pub.error_exit = IJGVERS::ErrorExit;
		jerr.pub.output_message = IJGVERS::OutputMessage;

		jpeg_create_compress(&cinfo);
		cinfo.client_data = nullptr;
		
		JPEGCODEC::This = this;

		// Specify destination manager
		jpeg_destination_mgr dest;
		dest.init_destination = IJGVERS::initDestination;
		dest.empty_output_buffer = IJGVERS::emptyOutputBuffer;
		dest.term_destination = IJGVERS::termDestination;
		cinfo.dest = &dest;

		cinfo.image_width = oldPixelData->ImageWidth;
		cinfo.image_height = oldPixelData->ImageHeight;
		cinfo.input_components = oldPixelData->SamplesPerPixel;
		cinfo.in_color_space = IJGVERS::getJpegColorSpace(oldPixelData->PhotometricInterpretation);

		jpeg_set_defaults(&cinfo);

		cinfo.optimize_coding = true;

		if (Mode == JpegMode::Baseline || Mode == JpegMode::Sequential) {
			jpeg_set_quality(&cinfo, params->Quality, 0);
		}
		else if (Mode == JpegMode::SpectralSelection) {
			jpeg_set_quality(&cinfo, params->Quality, 0);
			IJGVERS::jpeg_simple_spectral_selection(&cinfo);
		}
		else if (Mode == JpegMode::Progressive) {
			jpeg_set_quality(&cinfo, params->Quality, 0);
			jpeg_simple_progression(&cinfo);
		}
		else {
			jpeg_simple_lossless(&cinfo, Predictor, PointTransform);
		}
		
		cinfo.smoothing_factor = params->SmoothingFactor;

		if (Mode == JpegMode::Lossless) {
			jpeg_set_colorspace(&cinfo, cinfo.in_color_space);
			cinfo.comp_info[0].h_samp_factor = 1;
			cinfo.comp_info[0].v_samp_factor = 1;
		}
		else {
			// initialize sampling factors
			if (cinfo.jpeg_color_space == JCS_YCbCr && params->SampleFactor != JpegSampleFactor::Unknown) {
				switch(params->SampleFactor) {
				  case JpegSampleFactor::SF444: /* 4:4:4 sampling (no subsampling) */
					cinfo.comp_info[0].h_samp_factor = 1;
					cinfo.comp_info[0].v_samp_factor = 1;
					break;
				  case JpegSampleFactor::SF422: /* 4:2:2 sampling (horizontal subsampling of chroma components) */
					cinfo.comp_info[0].h_samp_factor = 2;
					cinfo.comp_info[0].v_samp_factor = 1;
					break;
				//case JpegSampleFactor::SF411: /* 4:1:1 sampling (horizontal and vertical subsampling of chroma components) */
				//	cinfo.comp_info[0].h_samp_factor = 2;
				//	cinfo.comp_info[0].v_samp_factor = 2;
				//	break;
				}
			}
			else {
				if (params->SampleFactor == JpegSampleFactor::Unknown)
					jpeg_set_colorspace(&cinfo, cinfo.in_color_space);

				// JPEG color space is not YCbCr, disable subsampling.
				cinfo.comp_info[0].h_samp_factor = 1;
				cinfo.comp_info[0].v_samp_factor = 1;
			}
		}

		// all other components are set to 1x1
		for (int sfi = 1; sfi < MAX_COMPONENTS; sfi++) {
			cinfo.comp_info[sfi].h_samp_factor = 1;
			cinfo.comp_info[sfi].v_samp_factor = 1;
		}

		jpeg_start_compress(&cinfo, TRUE);

		JSAMPROW row_pointer[1];
		int row_stride = oldPixelData->ImageWidth * oldPixelData->SamplesPerPixel * (oldPixelData->BitsStored <= 8 ? 1 : oldPixelData->BytesAllocated);

		while (cinfo.next_scanline < cinfo.image_height) {
			row_pointer[0] = (JSAMPLE *)(&framePtr[cinfo.next_scanline * row_stride]);
			jpeg_write_scanlines(&cinfo, row_pointer, 1);
		}

		jpeg_finish_compress(&cinfo);
		jpeg_destroy_compress(&cinfo);

		if (oldPixelData->PhotometricInterpretation == "RGB" && cinfo.jpeg_color_space == JCS_YCbCr) {
			if (params->SampleFactor == JpegSampleFactor::SF422)
				newPixelData->PhotometricInterpretation = "YBR_FULL_422";
			else
				newPixelData->PhotometricInterpretation = "YBR_FULL";
		}

		newPixelData->AddFrame(MemoryBuffer->ToArray());
	} finally {
		MemoryBuffer = nullptr;
	}
}



namespace IJGVERS {
	// private source manager struct
	struct SourceManagerStruct {
		// the standard IJG source manager object
		struct jpeg_source_mgr pub;

		// number of bytes to skip at start of buffer
		long skip_bytes;

		// buffer from which reading will continue as soon as the current buffer is empty
		unsigned char *next_buffer;

		// buffer size
		unsigned int *next_buffer_size;
	};

	void initSource(j_decompress_ptr /* cinfo */) {
	}

	ijg_boolean fillInputBuffer(j_decompress_ptr cinfo) {
		SourceManagerStruct *src = (SourceManagerStruct *)(cinfo->src);

		// if we already have the next buffer, switch buffers
		if (src->next_buffer) {
			src->pub.next_input_byte    = src->next_buffer;
			src->pub.bytes_in_buffer    = (unsigned int) src->next_buffer_size;
			src->next_buffer            = NULL;
			src->next_buffer_size       = 0;

			// The suspension was caused by IJG16skipInputData iff src->skip_bytes > 0.
			// In this case we must skip the remaining number of bytes here.
			if (src->skip_bytes > 0) {
				if (src->pub.bytes_in_buffer < (unsigned long) src->skip_bytes) {
					src->skip_bytes            -= src->pub.bytes_in_buffer;
					src->pub.next_input_byte   += src->pub.bytes_in_buffer;
					src->pub.bytes_in_buffer    = 0;
					// cause a suspension return
					return FALSE;
				}
				else {
					src->pub.bytes_in_buffer   -= (unsigned int) src->skip_bytes;
					src->pub.next_input_byte   += src->skip_bytes;
					src->skip_bytes             = 0;
				}
			}
			return TRUE;
		}

		// otherwise cause a suspension return
		return FALSE;
	}

	void skipInputData(j_decompress_ptr cinfo, long num_bytes) {
		SourceManagerStruct *src = (SourceManagerStruct *)(cinfo->src);

		if (src->pub.bytes_in_buffer < (size_t)num_bytes) {
			src->skip_bytes             = num_bytes - src->pub.bytes_in_buffer;
			src->pub.next_input_byte   += src->pub.bytes_in_buffer;
			src->pub.bytes_in_buffer    = 0; // causes a suspension return
		}
		else {
			src->pub.bytes_in_buffer   -= (unsigned int) num_bytes;
			src->pub.next_input_byte   += num_bytes;
			src->skip_bytes             = 0;
		}
	}

	void termSource(j_decompress_ptr /* cinfo */) {
	}
}

void JPEGCODEC::Decode(DcmPixelData^ oldPixelData, DcmPixelData^ newPixelData, DcmJpegParameters^ params, int frame) {
	array<unsigned char>^ jpegData = oldPixelData->GetFrameDataU8(frame);
	pin_ptr<unsigned char> jpegPin = &jpegData[0];
	unsigned char* jpegPtr = jpegPin;
	unsigned int jpegSize = jpegData->Length;
	
	jpeg_decompress_struct dinfo;
	memset(&dinfo, 0, sizeof(dinfo));

	IJGVERS::SourceManagerStruct src;
	memset(&src, 0, sizeof(IJGVERS::SourceManagerStruct));
	src.pub.init_source       = IJGVERS::initSource;
	src.pub.fill_input_buffer = IJGVERS::fillInputBuffer;
	src.pub.skip_input_data   = IJGVERS::skipInputData;
	src.pub.resync_to_restart = jpeg_resync_to_restart;
	src.pub.term_source       = IJGVERS::termSource;
	src.pub.bytes_in_buffer   = 0;
	src.pub.next_input_byte   = NULL;
	src.skip_bytes            = 0;
	src.next_buffer           = jpegPin;
	src.next_buffer_size      = (unsigned int*)jpegSize;

    IJGVERS::ErrorStruct jerr;
	memset(&jerr, 0, sizeof(IJGVERS::ErrorStruct));
	dinfo.err = jpeg_std_error(&jerr.pub);
	jerr.pub.error_exit = IJGVERS::ErrorExit;
	jerr.pub.output_message = IJGVERS::OutputMessage;

	jpeg_create_decompress(&dinfo);
	dinfo.src = (jpeg_source_mgr*)&src.pub;

	if (jpeg_read_header(&dinfo, TRUE) == JPEG_SUSPENDED)
		throw gcnew DicomCodecException("Unable to decompress JPEG: Suspended");
		
	if (newPixelData->PhotometricInterpretation == "YBR_FULL_422" || newPixelData->PhotometricInterpretation == "YBR_PARTIAL_422")
		newPixelData->PhotometricInterpretation = "YBR_FULL";
	else
		newPixelData->PhotometricInterpretation = oldPixelData->PhotometricInterpretation;

	if (params->ConvertColorspaceToRGB && (dinfo.out_color_space == JCS_YCbCr || dinfo.out_color_space == JCS_RGB)) { 
		if (oldPixelData->IsSigned) 
			throw gcnew DicomCodecException("JPEG codec unable to perform colorspace conversion on signed pixel data");
		dinfo.jpeg_color_space = IJGVERS::getJpegColorSpace(oldPixelData->PhotometricInterpretation);
		dinfo.out_color_space = JCS_RGB;
		newPixelData->PhotometricInterpretation = "RGB";
		newPixelData->PlanarConfiguration = 0;
	}
	else {
		dinfo.jpeg_color_space = JCS_UNKNOWN; 
		dinfo.out_color_space = JCS_UNKNOWN;
	}
	
	if (newPixelData->PhotometricInterpretation == "YBR_FULL")
		newPixelData->PlanarConfiguration = 1;

	jpeg_calc_output_dimensions(&dinfo);
	jpeg_start_decompress(&dinfo);

	int rowSize = dinfo.output_width * dinfo.output_components * sizeof(JSAMPLE);
	int frameSize = rowSize * dinfo.output_height;
	if ((frameSize % 2) != 0)
		frameSize++;
	array<unsigned char>^ frameBuffer = gcnew array<unsigned char>(frameSize);
	pin_ptr<unsigned char> framePin = &frameBuffer[0];
	unsigned char* framePtr = framePin;

	while (dinfo.output_scanline < dinfo.output_height) {
		int rows = jpeg_read_scanlines(&dinfo, (JSAMPARRAY)&framePtr, dinfo.output_height - dinfo.output_scanline);
		framePtr += rows * rowSize;
	}

	oldPixelData->Unload();
	
	if (newPixelData->IsPlanar)
		DcmCodecHelper::ChangePlanarConfiguration(frameBuffer, frameSize / newPixelData->BytesAllocated, newPixelData->BitsAllocated, newPixelData->SamplesPerPixel, 0);

	newPixelData->AddFrame(frameBuffer);

	jpeg_destroy_decompress(&dinfo);
}

int JPEGCODEC::ScanHeaderForPrecision(DcmPixelData^ pixelData) {
	array<unsigned char>^ jpegData = pixelData->GetFrameDataU8(0);
	pin_ptr<unsigned char> jpegPin = &jpegData[0];
	unsigned char* jpegPtr = jpegPin;
	unsigned int jpegSize = jpegData->Length;
	
	jpeg_decompress_struct dinfo;
	memset(&dinfo, 0, sizeof(dinfo));

	IJGVERS::SourceManagerStruct src;
	memset(&src, 0, sizeof(IJGVERS::SourceManagerStruct));
	src.pub.init_source       = IJGVERS::initSource;
	src.pub.fill_input_buffer = IJGVERS::fillInputBuffer;
	src.pub.skip_input_data   = IJGVERS::skipInputData;
	src.pub.resync_to_restart = jpeg_resync_to_restart;
	src.pub.term_source       = IJGVERS::termSource;
	src.pub.bytes_in_buffer   = 0;
	src.pub.next_input_byte   = NULL;
	src.skip_bytes            = 0;
	src.next_buffer           = jpegPin;
	src.next_buffer_size      = (unsigned int*)jpegSize;

    IJGVERS::ErrorStruct jerr;
	memset(&jerr, 0, sizeof(IJGVERS::ErrorStruct));
	dinfo.err = jpeg_std_error(&jerr.pub);
	jerr.pub.error_exit = IJGVERS::ErrorExit;
	jerr.pub.output_message = IJGVERS::OutputMessage;

	jpeg_create_decompress(&dinfo);
	dinfo.src = (jpeg_source_mgr*)&src.pub;

	if (jpeg_read_header(&dinfo, TRUE) == JPEG_SUSPENDED) {
		jpeg_destroy_decompress(&dinfo);
		throw gcnew DicomCodecException("Unable to read JPEG header: Suspended");
	}

	//pixelData->Unload();

	jpeg_destroy_decompress(&dinfo);

	return dinfo.data_precision;
}
