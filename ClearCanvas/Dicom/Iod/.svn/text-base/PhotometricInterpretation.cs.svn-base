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

using System.Collections.Generic;

namespace ClearCanvas.Dicom.Iod
{
	public class PhotometricInterpretation
	{
		public static PhotometricInterpretation Unknown = new PhotometricInterpretation("Unknown", "", false);

		public static PhotometricInterpretation Monochrome1 = new PhotometricInterpretation("Monochrome1", "MONOCHROME1", false);
		public static PhotometricInterpretation Monochrome2 = new PhotometricInterpretation("Monochrome2", "MONOCHROME2", false);
		public static PhotometricInterpretation PaletteColor = new PhotometricInterpretation("Palette Color", "PALETTE COLOR", true);
		public static PhotometricInterpretation Rgb = new PhotometricInterpretation("Rgb", "RGB", true);
		public static PhotometricInterpretation YbrFull = new PhotometricInterpretation("Ybr Full", "YBR_FULL", true);
		public static PhotometricInterpretation YbrFull422 = new PhotometricInterpretation("Ybr (Full 4-2-2)", "YBR_FULL_422", true);
		public static PhotometricInterpretation YbrIct = new PhotometricInterpretation("Ybr (Ict)", "YBR_ICT", true);
		public static PhotometricInterpretation YbrPartial422 = new PhotometricInterpretation("Ybr (Partial 4-2-2)", "YBR_PARTIAL_422", true);
		public static PhotometricInterpretation YbrRct = new PhotometricInterpretation("Ybr (Rct)", "YBR_RCT", true);

		private static readonly Dictionary<string, PhotometricInterpretation> _photometricInterpretations = new Dictionary<string, PhotometricInterpretation>();

		private readonly string _name;
		private readonly string _code;
		private readonly bool _isColor;

		internal PhotometricInterpretation(string name, string code, bool isColor)
		{
			_name = name;
			_code = code;
			_isColor = isColor;
		}

		static PhotometricInterpretation()
		{
			Add(Monochrome1);
			Add(Monochrome2);
			Add(PaletteColor);
			Add(Rgb);
			Add(YbrFull);
			Add(YbrFull422);
			Add(YbrIct);
			Add(YbrPartial422);
			Add(YbrRct);
		}

		private static void Add(PhotometricInterpretation photometricInterpretation)
		{
			_photometricInterpretations.Add(photometricInterpretation.Code, photometricInterpretation);
		}

		public string Name
		{
			get { return _name;	}
		}

		public string Code
		{
			get { return _code; }	
		}

		public bool IsColor
		{
			get { return _isColor; }
		}

		public override int GetHashCode()
		{
			return _code.GetHashCode();
		}

		public override bool Equals(object obj)
		{
 			 if (obj is PhotometricInterpretation)
 			 	return ((PhotometricInterpretation) obj).Code == this.Code;

			return base.Equals(obj);
		}

		public override string ToString()
		{
			return _name;
		}
				
		public static PhotometricInterpretation FromCodeString(string codeString)
		{
			PhotometricInterpretation theInterpretation;
			if (!_photometricInterpretations.TryGetValue(codeString ?? string.Empty, out theInterpretation))
				return Unknown;
			return theInterpretation;
		}
	}
}
