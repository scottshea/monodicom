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

using System.Configuration;
using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace ClearCanvas.Common
{
	/// <summary>
	/// Provides some basic information about the product, such as the name and version.
	/// </summary>
	public static class ProductInformation
	{
		private static string _name;
		private static Version _version;
		private static string _versionSuffix;
		private static string _copyright;
		private static string _license;

		static ProductInformation()
		{
			ProductSettings.Default.PropertyChanged += OnSettingPropertyChanged;
		}

		static void OnSettingPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			_name = null;
			_version = null;
			_versionSuffix = null;
			_copyright = null;
			_license = null;
		}

		/// <summary>
		/// Gets the product name.
		/// </summary>
		public static string Name
		{
			get
			{
				if (_name == null)
					_name = Decrypt(ProductSettings.Default.Name);

				return _name;
			}
		}

		/// <summary>
		/// Gets the product version.
		/// </summary>
		public static Version Version
		{
			get
			{
				if (_version == null)
				{
					string version = Decrypt(ProductSettings.Default.Version);
					if (String.IsNullOrEmpty(version))
						_version = Assembly.GetExecutingAssembly().GetName().Version;
                    else
						_version = new Version(version);
				}

				return _version;
			}
		}

		/// <summary>
		/// Gets the product version suffix (e.g. "SP1").
		/// </summary>
		public static string VersionSuffix
		{
			get
			{
				if (_versionSuffix == null)
				{
					string versionSuffix = Decrypt(ProductSettings.Default.VersionSuffix);
					if (String.IsNullOrEmpty(versionSuffix) || versionSuffix[0] != '*')
						_versionSuffix = "Unverified Build";
					else
						_versionSuffix = versionSuffix.Substring(1);
				}

				return _versionSuffix;
			}
		}

		/// <summary>
		/// Gets the product copyright (e.g. "Copyright 2009 ClearCanvas Inc.").
		/// </summary>
		public static string Copyright
		{
			get
			{
				if (_copyright == null)
					_copyright = Decrypt(ProductSettings.Default.Copyright);

				return _copyright;
			}
		}

		/// <summary>
		/// Gets the product license.
		/// </summary>
		public static string License
		{
			get
			{
				if (_license == null)
					_license = Decrypt(ProductSettings.Default.License);

				return _license;
			}
		}

		private static string Decrypt(string @string)
		{
			if (String.IsNullOrEmpty(@string))
				return @string;

			string result;
			try
			{
				byte[] bytes = Convert.FromBase64String(@string);
				using (MemoryStream dataStream = new MemoryStream(bytes))
				{
					RC2CryptoServiceProvider cryptoService = new RC2CryptoServiceProvider();
					cryptoService.Key = Encoding.UTF8.GetBytes("ClearCanvas");
					cryptoService.IV = Encoding.UTF8.GetBytes("IsSoCool");
					cryptoService.UseSalt = false;
					using (CryptoStream cryptoStream = new CryptoStream(dataStream, cryptoService.CreateDecryptor(), CryptoStreamMode.Read))
					{
						using (StreamReader reader = new StreamReader(cryptoStream, Encoding.UTF8))
						{
							result = reader.ReadToEnd();
							reader.Close();
						}
						cryptoStream.Close();
					}
					dataStream.Close();
				}
			}
			catch (Exception)
			{
				result = string.Empty;
			}
			return result;
		}

		/// <summary>
		/// Gets a string containing both the product name and version.
		/// </summary>
		/// <param name="includeBuildAndRevision">Specifies whether to include the build and revision numbers in the version; false means only the major and minor numbers are included.</param>
		/// <param name="includeVersionSuffix">Specifies whether to include the version suffix.</param>
		public static string GetNameAndVersion(bool includeBuildAndRevision, bool includeVersionSuffix)
		{
			return String.Format("{0} {1}", Name, GetVersion(includeBuildAndRevision, includeVersionSuffix));
		}

		/// <summary>
		/// Gets the version as a string, formatted based on the input options.
		/// </summary>
		/// <param name="includeBuildAndRevision">Specifies whether to include the build and revision numbers in the version; false means only the major and minor numbers are included.</param>
		/// <param name="includeVersionSuffix">Specifies whether to include the version suffix.</param>
		public static string GetVersion(bool includeBuildAndRevision, bool includeVersionSuffix)
		{
			string versionString;
			Version version = Version;

			if (includeBuildAndRevision)
				versionString = String.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
			else
				versionString = String.Format("{0}.{1}", version.Major, version.Minor);

			if (includeVersionSuffix && !String.IsNullOrEmpty(VersionSuffix))
				return String.Format("{0} {1}", versionString, VersionSuffix);

			return versionString;
		}
	}

	[SettingsGroupDescription("Settings that describe the product, such as the product name and version.")]
	[SettingsProvider(typeof(LocalFileSettingsProvider))]
	internal sealed partial class ProductSettings
	{
		private ProductSettings()
		{
		}
	}
}
