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
using System.Data.SqlServerCe;
using ClearCanvas.Common;
using System.IO;

namespace ClearCanvas.Dicom.DataStore.SetupApplication
{
	[ExtensionOf(typeof(ApplicationRootExtensionPoint))]
	public class Application : IApplicationRoot
	{
		#region IApplicationRoot Members

		public void RunApplication(string[] args)
		{
			// example command line args: 
			// ClearCanvas.Dicom.DataStore.SetupApplication.Application "<TrunkPath>\Dicom\DataStore\AuxiliaryFiles\empty_viewer.sdf" "<TrunkPath>\Dicom\DataStore\AuxiliaryFiles\CreateTables.clearcanvas.dicom.datastore.ddl"

			string databaseFile = args[0];
			string scriptFile = args[1];

			File.Delete(databaseFile);

			string connectionString = String.Format("Data Source=\"{0}\"", databaseFile);

			SqlCeEngine engine = new SqlCeEngine(connectionString);
			engine.CreateDatabase();
			engine.Dispose();

			StreamReader reader = new StreamReader(scriptFile);
			string scriptText = reader.ReadToEnd();
			reader.Close();

			SqlCeConnection connection = new SqlCeConnection(connectionString);
			connection.Open();

			SqlCeTransaction transaction = connection.BeginTransaction();
			SqlCeCommand command = new SqlCeCommand();
			command.Connection = connection;
			command.CommandText = scriptText;
			command.ExecuteNonQuery();

			transaction.Commit();
			connection.Close();

		}

		#endregion
	}
}