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
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ClearCanvas.Common;

namespace ClearCanvas.Dicom.Network
{
    internal struct ListenerInfo
    {
        public StartAssociation StartDelegate;
        public ServerAssociationParameters Parameters;
    }

    /// <summary>
    /// Class used to create background listen threads for incoming DICOM associations.
    /// </summary>
    internal class Listener : IDisposable
    {
        #region Members

        static private readonly Dictionary<IPEndPoint, Listener> _listeners = new Dictionary<IPEndPoint, Listener>();
        private readonly IPEndPoint _ipEndPoint = null;
        private readonly Dictionary<String, ListenerInfo> _applications = new Dictionary<String, ListenerInfo>();
        private TcpListener _tcpListener = null;
        private Thread _theThread = null;
        private volatile bool _stop = false;
		private static readonly object _syncLock = new object();
        #endregion

        #region Public Static Methods

        public static bool Listen(ServerAssociationParameters parameters, StartAssociation acceptor)
        {
			lock (_syncLock)
			{
				Listener theListener;
				if (_listeners.TryGetValue(parameters.LocalEndPoint, out theListener))
				{

					ListenerInfo info = new ListenerInfo();

					info.StartDelegate = acceptor;
					info.Parameters = parameters;

					if (theListener._applications.ContainsKey(parameters.CalledAE))
					{
						Platform.Log(LogLevel.Error, "Already listening with AE {0} on {1}", parameters.CalledAE,
						             parameters.LocalEndPoint.ToString());
						return false;
					}

					theListener._applications.Add(parameters.CalledAE, info);
					Platform.Log(LogLevel.Info, "Starting to listen with AE {0} on existing port {1}", parameters.CalledAE,
					             parameters.LocalEndPoint.ToString());
				}
				else
				{
					theListener = new Listener(parameters, acceptor);
					if (!theListener.StartListening())
					{
						Platform.Log(LogLevel.Error, "Unexpected error starting to listen on {0}", parameters.LocalEndPoint.ToString());
						return false;
					}

					_listeners[parameters.LocalEndPoint] = theListener;
					theListener.StartThread();

					Platform.Log(LogLevel.Info, "Starting to listen with AE {0} on port {1}", parameters.CalledAE,
					             parameters.LocalEndPoint.ToString());
				}

				return true;
			}
        }

		public bool StartListening()
		{
			_tcpListener = new TcpListener(_ipEndPoint);
			try
			{
				_tcpListener.Start(50);
			}
			catch (SocketException e)
			{
				Platform.Log(LogLevel.Error, e, "Unexpected exception when starting TCP listener");
				Platform.Log(LogLevel.Error, "Shutting down listener on {0}", _ipEndPoint.ToString());
				_tcpListener = null;
				return false;
			}
			return true;
		}

        public static bool StopListening(ServerAssociationParameters parameters)
        {
			lock (_syncLock)
			{
				Listener theListener;

				if (_listeners.TryGetValue(parameters.LocalEndPoint, out theListener))
				{
					if (theListener._applications.ContainsKey(parameters.CalledAE))
					{
						theListener._applications.Remove(parameters.CalledAE);

						if (theListener._applications.Count == 0)
						{
							// Cleanup the listener
							_listeners.Remove(parameters.LocalEndPoint);
							theListener.StopThread();
							theListener.Dispose();
						}
						Platform.Log(LogLevel.Info, "Stopping listening with AE {0} on {1}", parameters.CalledAE,
						             parameters.LocalEndPoint.ToString());
					}
					else
					{
						Platform.Log(LogLevel.Error, "Unable to stop listening on AE {0}, assembly was not listening with this AE.",
						             parameters.CalledAE);
						return false;
					}
				}
				else
				{
					Platform.Log(LogLevel.Error, "Unable to stop listening, assembly was not listening on end point {0}.",
					             parameters.LocalEndPoint.ToString());
					return false;
				}

				return true;
			}
        }
        #endregion

        #region Constructors
        internal Listener(ServerAssociationParameters parameters, StartAssociation acceptor)
        {
            ListenerInfo info = new ListenerInfo();

            info.Parameters = parameters;
            info.StartDelegate = acceptor;

            _applications.Add(parameters.CalledAE, info);

            _ipEndPoint = parameters.LocalEndPoint;
        }
        #endregion

        private void StartThread()
        {
            
            _theThread = new Thread(Listen);
            _theThread.Name = "Association Listener on port " + _ipEndPoint.Port;

            _theThread.Start();
        }

        private void StopThread()
        {
            _stop = true;

            _theThread.Join();
        }

        public void Listen()
        {
            while (_stop == false)
            {
                // Tried Async i/o here, but had some weird problems with connections not getting
                // through.
                if (_tcpListener.Pending())
                {
                    Socket theSocket = _tcpListener.AcceptSocket();

					// The DicomServer will automatically start working in the background
                    new DicomServer(theSocket, _applications);
                    continue;
                }
                Thread.Sleep(10);               
            }
            try
            {
                _tcpListener.Stop();
            }
            catch (SocketException e)
            {
				Platform.Log(LogLevel.Error, e, "Unexpected exception when stoppinging TCP listener on {0}", _ipEndPoint, ToString());
            }
        }

        #region IDisposable Implementation
        public void Dispose()
        {
            StopThread();
            if (_tcpListener != null)
            {
                _tcpListener.Stop();
                _tcpListener = null;
            }
        }
        #endregion
    }
}
