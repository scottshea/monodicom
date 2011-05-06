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

using System.IO;
using System.Text;
using System.Xml;

namespace ClearCanvas.Common.Statistics
{
    /// <summary>
    /// Provides statistics logging mechanism.
    /// </summary>
    public class StatisticsLogger
    {
        private static readonly XmlDocument doc = new XmlDocument();
        private static readonly object[] _extensions;
        private static readonly StatisticsLoggerExtensionPoint _xp = new StatisticsLoggerExtensionPoint();

        static StatisticsLogger()
        {
			try
			{
				_extensions = _xp.CreateExtensions();
			}
			catch (PluginException)
			{
				_extensions = new object[0];
			}
        }

		/// <summary>
        /// Logs a statistics.
        /// </summary>
        /// <param name="level">The log level used for logging the statistics</param>
        /// <param name="recursive">Bool telling if the log should be recursive, or just display averages.</param>
        /// <param name="statistics">The statistics to be logged</param>
		public static void Log(LogLevel level, bool recursive, StatisticsSet statistics)
		{
			XmlElement el = statistics.GetXmlElement(doc, recursive);

			using (StringWriter sw = new StringWriter())
			{
				XmlWriterSettings settings = new XmlWriterSettings();
				settings.Indent = true;
				settings.NewLineOnAttributes = false;
				settings.OmitXmlDeclaration = true;
				settings.Encoding = Encoding.UTF8;

				XmlWriter writer = XmlWriter.Create(sw, settings);
				el.WriteTo(writer);
				writer.Flush();

				Platform.Log(level, sw.ToString());

				writer.Close();
			}

			foreach (IStatisticsLoggerListener extension in _extensions)
			{
				extension.OnStatisticsLogged(statistics);
			}
		}

    	/// <summary>
        /// Logs a statistics.
        /// </summary>
        /// <param name="level">The log level used for logging the statistics</param>
        /// <param name="statistics">The statistics to be logged</param>
        public static void Log(LogLevel level, StatisticsSet statistics)
        {
    		Log(level, true, statistics);
        }
    }
}