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
using System.Runtime.Remoting.Messaging;
using ClearCanvas.Dicom.Iod.Iods;

namespace ClearCanvas.Dicom.Network.Scu
{
    /// <summary>
    /// Modality Worklist Scu.
    /// <para>
    /// <example>
    /// ModalityWorklistScu modalityWorklistScu = new ModalityWorklistScu();
    /// ModalityWorklistIod modalityWorklistIod = new ModalityWorklistIod();
    /// modalityWorklistIod.SetCommonTags();
    /// modalityWorklistScu.GetWorkList("myClientAeTitle", "myServerAeTitle", "127.0.0.1", 5678, modalityWorklistIod);
    /// System.Diagnostics.Debug.WriteLine(ModalityWorklistResults.Count.ToString()); 
    /// </example>
    /// </para>
    /// </summary>
    public class ModalityWorklistScu : FindScuBase
    {
        #region Public Events/Delegates
        public delegate IList<ModalityWorklistIod> ModalityWorklistDelegate(string clientAETitle, string remoteAE, string remoteHost, int remotePort, ModalityWorklistIod modalityWorklistIod);
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="VerificationScu"/> class.
        /// </summary>
        public ModalityWorklistScu()
            :base()
        {
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Specifies the find sop class (ModalityWorklistInformationModelFind)
        /// </summary>
        /// <value>The find sop class.</value>
        /// <remarks>Abstract so subclass can specify.</remarks>
        public override SopClass FindSopClass
        {
            get { return SopClass.ModalityWorklistInformationModelFind; }
        }

        /// <summary>
        /// Gets the results as a bindinglist of ModalityWorklistIods.
        /// </summary>
        /// <value>The modality worklist results.</value>
        public IList<ModalityWorklistIod> ModalityWorklistResults
        {
            get
            {
                IList<ModalityWorklistIod> results = new System.ComponentModel.BindingList<ModalityWorklistIod>();
                if (base.Results != null)
                {
                    foreach (DicomAttributeCollection dicomAttributeCollection in base.Results)
                        results.Add(new ModalityWorklistIod(dicomAttributeCollection));
                }
                return results;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Finds the specified modality worklist Iod.
        /// </summary>
        /// <param name="clientAETitle">The client AE title.</param>
        /// <param name="remoteAE">The remote AE.</param>
        /// <param name="remoteHost">The remote host.</param>
        /// <param name="remotePort">The remote port.</param>
        /// <param name="modalityWorklistIod">The modality worklist iod.</param>
        /// <returns></returns>
        public IList<ModalityWorklistIod> GetWorkList(string clientAETitle, string remoteAE, string remoteHost, int remotePort, ModalityWorklistIod modalityWorklistIod)
        {
			base.Find(clientAETitle, remoteAE, remoteHost, remotePort, modalityWorklistIod.DicomAttributeProvider as DicomAttributeCollection);
            return this.ModalityWorklistResults;
        }

        /// <summary>
        /// Begins the find request in asynchronous mode.
        /// </summary>
        /// <param name="clientAETitle">The client AE title.</param>
        /// <param name="remoteAE">The remote AE.</param>
        /// <param name="remoteHost">The remote host.</param>
        /// <param name="remotePort">The remote port.</param>
        /// <param name="requestAttributeCollection">The request attribute collection.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="asyncState">State of the async.</param>
        /// <returns></returns>
        public IAsyncResult BeginGetWorkList(string clientAETitle, string remoteAE, string remoteHost, int remotePort, ModalityWorklistIod modalityWorklistIod, AsyncCallback callback, object asyncState)
        {
            ModalityWorklistDelegate modalityWorklistDelegate = new ModalityWorklistDelegate(this.GetWorkList);

            return modalityWorklistDelegate.BeginInvoke(clientAETitle, remoteAE, remoteHost, remotePort, modalityWorklistIod, callback, asyncState);
        }

        /// <summary>
        /// Ends the asyncronous findrequest.
        /// </summary>
        /// <param name="ar">The ar.</param>
        /// <returns></returns>
        public IList<ModalityWorklistIod> EndGetWorkList(IAsyncResult ar)
        {
            ModalityWorklistDelegate modalityWorklistDelegate = ((AsyncResult)ar).AsyncDelegate as ModalityWorklistDelegate;
            if (modalityWorklistDelegate != null)
            {
                return modalityWorklistDelegate.EndInvoke(ar) as List<ModalityWorklistIod>;
            }
            else
                throw new InvalidOperationException("cannot get results, asynchresult is null");
        }
        #endregion

        #region Protected Overridden Methods
        /// <summary>
        /// Determines whether [is query level valid].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [is query level valid]; otherwise, <c>false</c>.
        /// </returns>
        protected override bool ValidateQuery()
        {
            // Modality Worklist shouldn't have a query retrieve level... 
            return true;

        }
        #endregion

    }
}
