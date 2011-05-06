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

namespace ClearCanvas.Common.Utilities
{
	/// <summary>
	/// Used to report performance statistics via the <see cref="PerformanceReportBroker"/>.
	/// </summary>
	public class PerformanceReport
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public PerformanceReport(string category, string identifier, TimeSpan totalTime)
		{
			Category = category;
			Identifier = identifier;
			TotalTime = totalTime;
		}

		/// <summary>
		/// The Category.
		/// </summary>
		public readonly string Category;

		/// <summary>
		/// The Identifier.
		/// </summary>
		public readonly string Identifier;

		/// <summary>
		/// The total time taken to perform the operation.
		/// </summary>
		public readonly TimeSpan TotalTime;
	}

	/// <summary>
	/// A generic way to publish events.
	/// </summary>
	/// <remarks>
	/// NOTE: Should be used for debugging purposes only.
	/// </remarks>
	public static class PerformanceReportBroker
	{
		private static readonly object _syncLock = new object();
		private static event EventHandler<ItemEventArgs<PerformanceReport>> _report;

		/// <summary>
		/// The <see cref="PerformanceReportBroker.Report"/> event delegate.
		/// </summary>
		public delegate void ReportDelegate(PerformanceReport reportItem);

		/// <summary>
		/// The event that is fired as reports are published.
		/// </summary>
		public static event EventHandler<ItemEventArgs<PerformanceReport>> Report
		{
			add
			{
				lock (_syncLock)
				{
					_report += value;
				}
			}
			remove
			{
				lock (_syncLock)
				{
					_report -= value;
				}
			}
		}

		/// <summary>
		/// Publishes a <see cref="PerformanceReport"/>.
		/// </summary>
		public static void PublishReport(string category, string identifier, double totalTimeSeconds)
		{
			PublishReport(new PerformanceReport(category, identifier, TimeSpan.FromSeconds(totalTimeSeconds)));
		}

		/// <summary>
		/// Publishes a <see cref="PerformanceReport"/>.
		/// </summary>
		public static void PublishReport(string category, string identifier, TimeSpan totalTime)
		{
			PublishReport(new PerformanceReport(category, identifier, totalTime));
		}

		/// <summary>
		/// Publishes a <see cref="PerformanceReport"/>.
		/// </summary>
		public static void PublishReport(PerformanceReport reportItem)
		{
			PublishReport(null, reportItem);	
		}

		/// <summary>
		/// Publishes a <see cref="PerformanceReport"/>.
		/// </summary>
		public static void PublishReport(object sender, PerformanceReport reportItem)
		{
			lock (_syncLock)
			{
				if (_report == null)
					return;

				_report(sender, new ItemEventArgs<PerformanceReport>(reportItem));
			}
		}
	}
}
