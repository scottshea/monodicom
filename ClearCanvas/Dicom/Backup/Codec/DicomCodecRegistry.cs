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
using System.Collections.Generic;
using ClearCanvas.Common;
using ClearCanvas.Dicom;

namespace ClearCanvas.Dicom.Codec
{
    /// <summary>
    /// Registry of <see cref="IDicomCodecFactory"/> implementations that extend <see cref="DicomCodecFactoryExtensionPoint"/>.
    /// </summary>
    public static class DicomCodecRegistry
    {
        #region Private Members

    	private static readonly Dictionary<TransferSyntax, IDicomCodecFactory> _dictionary;

		#endregion

        #region Static Constructor
        
		static DicomCodecRegistry()
        {
			_dictionary = new Dictionary<TransferSyntax, IDicomCodecFactory>();

			try
			{
				DicomCodecFactoryExtensionPoint ep = new DicomCodecFactoryExtensionPoint();
				object[] codecFactories = ep.CreateExtensions();

				foreach (IDicomCodecFactory codecFactory in codecFactories)
					_dictionary[codecFactory.CodecTransferSyntax] = codecFactory;
			}
			catch(NotSupportedException)
			{
				Platform.Log(LogLevel.Info, "No dicom codec extension(s) exist.");
			}
			catch(Exception e)
			{
				Platform.Log(LogLevel.Error, e, "An error occurred while attempting to register the dicom codec extensions.");
			}
        }

		#endregion

		#region Public Static Methods

		/// <summary>
		/// Gets the <see cref="TransferSyntax"/>es of the available <see cref="IDicomCodecFactory"/> implementations.
		/// </summary>
		public static TransferSyntax[] GetCodecTransferSyntaxes()
		{
			TransferSyntax[] syntaxes = new TransferSyntax[_dictionary.Count];
			_dictionary.Keys.CopyTo(syntaxes, 0);
			return syntaxes;
		}

    	/// <summary>
    	/// Gets an array of <see cref="IDicomCodec"/>s (one from each available <see cref="IDicomCodecFactory"/>).
    	/// </summary>
		public static IDicomCodec[] GetCodecs()
		{
			IDicomCodec[] codecs = new IDicomCodec[_dictionary.Count];
			int i = 0;
			foreach (IDicomCodecFactory factory in _dictionary.Values)
				codecs[i++] = factory.GetDicomCodec();

			return codecs;
		}

		/// <summary>
		/// Gets an array <see cref="IDicomCodecFactory"/> instances.
		/// </summary>
		/// <returns></returns>
		public static IDicomCodecFactory[] GetCodecFactories()
		{
			DicomCodecFactoryExtensionPoint ep = new DicomCodecFactoryExtensionPoint();
			object[] extensions = ep.CreateExtensions();
			IDicomCodecFactory[] codecFactories = new IDicomCodecFactory[extensions.Length];
			extensions.CopyTo(codecFactories, 0);
			return codecFactories;
		}
		
		/// <summary>
        /// Get a codec instance from the registry.
        /// </summary>
        /// <param name="syntax">The transfer syntax to get a codec for.</param>
        /// <returns>null if a codec has not been registered, an <see cref="IDicomCodec"/> instance otherwise.</returns>
        public static IDicomCodec GetCodec(TransferSyntax syntax)
        {
			IDicomCodecFactory factory;
            if (!_dictionary.TryGetValue(syntax, out factory))
                return null;

            return factory.GetDicomCodec();
        }

        /// <summary>
        /// Get default parameters for the codec.
        /// </summary>
        /// <param name="syntax">The transfer syntax to get the parameters for.</param>
        /// <param name="collection">The <see cref="DicomAttributeCollection"/> that the codec will work on.</param>
        /// <returns>null if no codec is registered, the parameters otherwise.</returns>
        public static DicomCodecParameters GetCodecParameters(TransferSyntax syntax, DicomAttributeCollection collection)
        {
			IDicomCodecFactory factory;
			if (!_dictionary.TryGetValue(syntax, out factory))
				return null;

            return factory.GetCodecParameters(collection);
        }
        #endregion
    }
}
