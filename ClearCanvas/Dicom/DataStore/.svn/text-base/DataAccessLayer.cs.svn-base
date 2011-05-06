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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using System.IO;
using ClearCanvas.Common;

namespace ClearCanvas.Dicom.DataStore
{
	public sealed partial class DataAccessLayer
    {
	    private static readonly Configuration _hibernateConfiguration;
		private static readonly ISessionFactory _sessionFactory;
		private static volatile IStudyStorageLocator _studyStorageLocator;

		private DataAccessLayer()
		{
		}

		static DataAccessLayer()
		{
			_hibernateConfiguration = new Configuration();
			Assembly thisAssembly = typeof (DataAccessLayer).Assembly;
			string thisAssemblyName = thisAssembly.GetName().Name;
			string exePath = AppDomain.CurrentDomain.BaseDirectory;
			string thisAssemblyPath = Path.GetDirectoryName(thisAssembly.Location);
			
			string configPath = Path.Combine(exePath, thisAssemblyName) + ".cfg.xml";
			if (!File.Exists(configPath))
			{
				Platform.Log(LogLevel.Debug, "NHibernate config file '{0}' does not exist; checking assembly location ...", configPath);
				configPath = Path.Combine(thisAssemblyPath, thisAssemblyName) + ".cfg.xml";
				if (!File.Exists(configPath))
				{
					string message =
						String.Format("NHibernate config file '{0}' does not exist; database cannot be initialized.", configPath);

					Platform.Log(LogLevel.Debug, message);
					throw new FileNotFoundException(message, configPath);
				}
			}

			_hibernateConfiguration.Configure(configPath);
			_hibernateConfiguration.AddAssembly(thisAssemblyName);
			_sessionFactory = _hibernateConfiguration.BuildSessionFactory();
		}

		internal static Configuration HibernateConfiguration
		{
			get { return _hibernateConfiguration; }	
		}

		private static ISessionFactory SessionFactory
		{
			get { return _sessionFactory; }
		}

		public static void SetStudyStorageLocator(IStudyStorageLocator locator)
		{
			Platform.CheckForNullReference(locator, "locator");

			_studyStorageLocator = locator;
		}

		public static IDataStoreReader GetIDataStoreReader()
        {
			return new DataStoreReader(SessionManager.Get());
        }

		public static IDicomPersistentStoreValidator GetIDicomPersistentStoreValidator()
		{
			return new DicomPersistentStoreValidator();
		}

		public static IDicomPersistentStore GetIDicomPersistentStore()
		{
			if (_studyStorageLocator == null)
				throw new InvalidOperationException("The study storage locator must be set before the persistent store can be used.");

			return new DicomPersistentStore();
		}

		public static IDataStoreStudyRemover GetIDataStoreStudyRemover()
		{
			return new DataStoreWriter(SessionManager.Get());
		}

		internal static IDataStoreWriter GetIDataStoreWriter()
        {
			return new DataStoreWriter(SessionManager.Get());
        }

		internal static IStudyStorageLocator GetStudyStorageLocator()
		{
			return _studyStorageLocator;
		}

		#region Helper Methods

		private static readonly string[] WildcardExcludedVRs = 
			{ "DA", "TM", "DT", "SL", "SS", "US", "UL", "FL", "FD", "OB", "OW", "UN", "AT", "DS", "IS", "AS", "UI" };

		private static bool IsWildCardCriteria(string criteria, QueryablePropertyInfo column)
		{
			foreach (string excludeVR in WildcardExcludedVRs)
			{
				if (0 == String.Compare(excludeVR, column.Path.ValueRepresentation.Name, true))
					return false;
			}

			return ContainsWildcardCharacters(criteria);
		}

		private static bool ContainsWildcardCharacters(string criteria)
		{
			return criteria.Contains("*") || criteria.Contains("?");
		}

		#endregion
	}
}
