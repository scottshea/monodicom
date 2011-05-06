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
using System.Text;
using log4net;
using ClearCanvas.Common.Utilities;
using System.Collections.Generic;
using System.IO;

// Configure log4net using the .log4net file
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Logging.config", Watch = true)]
// This will cause log4net to look for a configuration file
// called TestApp.exe.log4net in the application base
// directory (i.e. the directory containing TestApp.exe)
// The config file will be watched for changes.

namespace ClearCanvas.Common
{
	/// <summary>
	/// Defines the logging level for calls to one of the <b>Platform.Log</b> methods.
	/// </summary>
	public enum LogLevel
	{
		/// <summary>
		/// Debug log level.
		/// </summary>
		Debug,
		/// <summary>
		/// Info log level.
		/// </summary>
		Info,
		/// <summary>
		/// Warning log level.
		/// </summary>
		Warn,
		/// <summary>
		/// Error log level.
		/// </summary>
		Error,
		/// <summary>
		/// Fatal log level.
		/// </summary>
		Fatal
	}


    /// <summary>
    /// An extension point for <see cref="IMessageBox"/>es.
    /// </summary>
    [ExtensionPoint()]
    public sealed class MessageBoxExtensionPoint : ExtensionPoint<IMessageBox>
    {
    }

    /// <summary>
    /// Defines the Application Root extension point.
    /// </summary>
    /// <remarks>
	/// When one of the <b>Platform.StartApp</b> methods are called,
	/// the platform creates an application root extension and executes it by calling
	/// <see cref="IApplicationRoot.RunApplication" />.
	/// </remarks>
    [ExtensionPoint()]
    public sealed class ApplicationRootExtensionPoint : ExtensionPoint<IApplicationRoot>
    {
    }

    /// <summary>
    /// An extension point for <see cref="ITimeProvider"/>s.
    /// </summary>
    /// <remarks>
    /// Used internally by the framework to create a <see cref="ITimeProvider"/> for
    /// use by the application (see <see cref="Platform.Time"/>).
    /// </remarks>
	[ExtensionPoint()]
    public sealed class TimeProviderExtensionPoint : ExtensionPoint<ITimeProvider>
    {
    }

    /// <summary>
    /// Defines an extension point for service providers.
    /// </summary>
    /// <remarks>
    /// <para>
	/// A service provider is a class that knows how to provide a specific set of services to the 
	/// application.  A given service should be provided exclusively by one provider 
	/// (i.e. no two providers should provide the same service).  The application obtains
	/// services through the <see cref="Platform.GetService"/> method.
	/// </para>
	/// <para>
    /// A service provider may be accessed by multiple threads.  For reasons of thread-safety, a service provider
    /// should return a new instance of the service class for each call to <see cref="IServiceProvider.GetService"/>,
    /// so that each thread receives its own copy of the service.
    /// If the provider returns the same object (singleton), then the service object itself must be thread-safe.
	/// </para>
    /// </remarks>
    [ExtensionPoint]
    public sealed class ServiceProviderExtensionPoint : ExtensionPoint<IServiceProvider>
    {
    }

	/// <summary>
	/// A collection of useful utility functions.
	/// </summary>
	public static class Platform
	{
		#region Private fields

		private static object _syncRoot = new Object();

    	private static readonly ILog _log = LogManager.GetLogger(typeof(Platform));

		private static string _pluginSubFolder = "plugins";
        private static string _commonSubFolder = "common";
        private static string _logSubFolder = "logs";

		private static volatile string _installDirectory = null;
		private static volatile string _pluginsDirectory = null;
        private static volatile string _commonDirectory = null;
        private static volatile string _logDirectory = null;

		private static volatile PluginManager _pluginManager;
		private static volatile IApplicationRoot _applicationRoot;
		private static volatile IMessageBox _messageBox;
		private static volatile ITimeProvider _timeProvider;
        private static volatile IServiceProvider[] _serviceProviders;

		#endregion

#if UNIT_TESTS

		/// <summary>
        /// Sets the extension factory that is used to instantiate extensions.
        /// </summary>
        /// <remarks>
        /// This purpose of this method is to facilitate unit testing by allowing the creation of extensions
        /// to be controlled by the testing code.
        /// </remarks>
        /// <param name="factory"></param>
        public static void SetExtensionFactory(IExtensionFactory factory)
        {
            ExtensionPoint.SetExtensionFactory(factory);
        }
#endif

		/// <summary>
		/// Gets the one and only <see cref="PluginManager"/>.
		/// </summary>
		public static PluginManager PluginManager
		{
			get
			{
				if (_pluginManager == null)
				{
					lock (_syncRoot)
					{
						if (_pluginManager == null)
							_pluginManager = new PluginManager(PluginDirectory);
					}
				}

				return _pluginManager;
			}
		}

        /// <summary>
        /// Gets whether the application is executing on a Win32 operating system
        /// </summary>
        /// <remarks>
        /// This method is thread-safe.
        /// </remarks>
		public static bool IsWin32Platform
		{
			get
			{
				PlatformID id = Environment.OSVersion.Platform;
				return (id == PlatformID.Win32NT || id == PlatformID.Win32Windows || id == PlatformID.Win32S || id == PlatformID.WinCE);
			}
		}

        /// <summary>
        /// Gets whether the application is executing on a Unix operating systems
        /// </summary>
        /// <remarks>
        /// This method is thread-safe.
        /// </remarks>
        public static bool IsUnixPlatform
		{
			get
			{
				PlatformID id = Environment.OSVersion.Platform;
				return (id == PlatformID.Unix);
			}
		}

        /// <summary>
        /// Gets the file-system path separator character for the current operating system
        /// </summary>
        /// <remarks>
        /// This method is thread-safe.
        /// </remarks>
        public static char PathSeparator
        {
            get { return IsWin32Platform ? '\\' : '/'; }
        }
		
		/// <summary>
		/// Gets the ClearCanvas installation directory.
		/// </summary>
        /// <remarks>
        /// This method is thread-safe.
        /// </remarks>
        public static string InstallDirectory
		{
			get
			{
                if (_installDirectory == null)
                {
                    lock (_syncRoot)
                    {
						if (_installDirectory == null)
							_installDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    }
                }

				return _installDirectory;
			}
		}

		/// <summary>
		/// Gets the fully qualified plugin directory.
		/// </summary>
        /// <remarks>
        /// This method is thread-safe.
        /// </remarks>
        public static string PluginDirectory
		{
			get
			{
                if (_pluginsDirectory == null)
                {
                    lock (_syncRoot)
                    {
						if (_pluginsDirectory == null)
						{
							string pluginsDirectory =
								Path.Combine(Platform.InstallDirectory, _pluginSubFolder);

							if (Directory.Exists(pluginsDirectory))
								_pluginsDirectory = pluginsDirectory;
							else
							    _pluginsDirectory = InstallDirectory;
						}
                    }
                }

                return _pluginsDirectory;
			}
		}

        /// <summary>
        /// Gets the fully qualified common directory.
        /// </summary>
        /// <remarks>
        /// This method is thread-safe.
        /// </remarks>
        public static string CommonDirectory
        {
            get
            {
                if (_commonDirectory == null)
                {
                    lock (_syncRoot)
                    {
						if (_commonDirectory == null)
						{
							string commonDirectory =
								Path.Combine(Platform.InstallDirectory, _commonSubFolder);

							if (Directory.Exists(commonDirectory))
								_commonDirectory = commonDirectory;
							else
								_commonDirectory = InstallDirectory;
						}
                    }
                }

                return _commonDirectory;
            }
        }
        
        /// <summary>
		/// Gets the fully qualified log directory.
		/// </summary>
        /// <remarks>
        /// This method is thread-safe.
        /// </remarks>
        public static string LogDirectory
		{
			get
			{
                if (_logDirectory == null)
                {
                    lock (_syncRoot)
                    {
						if (_logDirectory == null)
							_logDirectory = System.IO.Path.Combine(Platform.InstallDirectory, _logSubFolder);
                    }
                }

                return _logDirectory;
			}
		}

        /// <summary>
        /// Gets the current time from an extension of <see cref="TimeProviderExtensionPoint"/>, if one exists.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The time returned may differ from the current time on the local machine, because the provider may choose
        /// to obtain the time from another source (i.e. a server).
		/// </para>
		/// <para>
        /// This method is thread-safe.
		/// </para>
        /// </remarks>
        public static DateTime Time
        {
            get
            {
                if (_timeProvider == null)
                {
                    lock (_syncRoot)
                    {
                        if (_timeProvider == null)
                        {
                            try
                            {
                                // check for a time provider extension
                                TimeProviderExtensionPoint xp = new TimeProviderExtensionPoint();
                                _timeProvider = (ITimeProvider)xp.CreateExtension();
                            }
                            catch (NotSupportedException)
                            {
                                // can't find time provider, default to local time
                                Log(LogLevel.Warn, SR.LogTimeProviderNotFound);

                                _timeProvider = new LocalTimeProvider();
                            }
                        }
                    }
                }

                // need to lock here, as the time provider itself may not be thread-safe
                // note: lock on _timeProvider rather than _syncRoot, so _syncRoot remains free for other methods
                lock (_timeProvider)
                {
                    return _timeProvider.CurrentTime;
                }
            }
        }

        /// <summary>
        /// Starts the application.
        /// </summary>
        /// <param name="applicationRootFilter">An extension filter that selects the application root extension to execute.</param>
        /// <param name="args">The set of arguments passed from the command line.</param>
        /// <remarks>
        /// A ClearCanvas based application is started by calling this convenience method from
        /// a bootstrap executable of some kind.  Calling this method results in the loading
		/// of all plugins and creation of an <see cref="IApplicationRoot"/> extension.  
		/// This method is not thread-safe as it should only ever be invoked once per execution, by a single thread.
        /// </remarks>
        public static void StartApp(ExtensionFilter applicationRootFilter, string[] args)
        {
#if !DEBUG
            try
            {
#endif
            ApplicationRootExtensionPoint xp = new ApplicationRootExtensionPoint();
            _applicationRoot = (applicationRootFilter == null) ?
                (IApplicationRoot)xp.CreateExtension() :
                (IApplicationRoot)xp.CreateExtension(applicationRootFilter);
            _applicationRoot.RunApplication(args);

#if !DEBUG
            }
            catch (Exception e)
            {
				Platform.Log(LogLevel.Fatal, e);

                // for convenience, if this is console app, also print the message to the console
                Console.WriteLine(e.Message);
            }
#endif
        }

		// Public methods

		/// <summary>
		/// Starts the application.
		/// </summary>
		/// <remarks>
		/// A ClearCanvas based application is started by calling this convenience method from
		/// a bootstrap executable of some kind.  Calling this method results in the loading
		/// of all plugins and creation of an <see cref="IApplicationRoot"/> extension.  
		/// This method is not thread-safe as it should only ever be invoked once per execution, by a single thread.
		/// </remarks>
		public static void StartApp()
		{
            StartApp((ExtensionFilter)null, new string[] { });
		}

        /// <summary>
        /// Starts the application matching the specified fully or partially qualified class name.
        /// </summary>
        /// <param name="appRootClassName">The name of an application root class, which need not be fully qualified.</param>
        /// <param name="args"></param>
        public static void StartApp(string appRootClassName, string[] args)
        {
            ExtensionInfo[] appRoots = new ApplicationRootExtensionPoint().ListExtensions();

            // try an exact match
            List<ExtensionInfo> matchingRoots = CollectionUtils.Select(appRoots,
                delegate(ExtensionInfo info) { return info.ExtensionClass.FullName == appRootClassName; });

            if (matchingRoots.Count == 0)
            {
                // try a partial match
                matchingRoots = CollectionUtils.Select(appRoots,
                    delegate(ExtensionInfo info)
                    {
                         return info.ExtensionClass.FullName.EndsWith(
                             appRootClassName, StringComparison.InvariantCultureIgnoreCase);
                    });
            }

            if(matchingRoots.Count == 0)
                throw new NotSupportedException(
                    string.Format(SR.ExceptionApplicationRootNoMatches, appRootClassName));
            if (matchingRoots.Count > 1)
                throw new NotSupportedException(
                    string.Format(SR.ExceptionApplicationRootMultipleMatches, appRootClassName));

            // start app
            StartApp(new ClassNameExtensionFilter(CollectionUtils.FirstElement(matchingRoots).ExtensionClass.FullName), args);
        }

	    /// <summary>
        /// Obtains an instance of the specified service for use by the application.
        /// </summary>
        /// <remarks>
        /// This method is thread-safe.
        /// </remarks>
        /// <typeparam name="TService">The type of service to obtain.</typeparam>
        /// <returns>An instance of the specified service.</returns>
        /// <exception cref="UnknownServiceException">The requested service cannot be provided.</exception>
        public static TService GetService<TService>()
        {
            return (TService)GetService(typeof(TService));
        }

        /// <summary>
		/// For use with the <see cref="GetService{TService}(WithServiceDelegate{TService})"/> method.
        /// </summary>
		public delegate void WithServiceDelegate<T>(T service);

        /// <summary>
        /// Obtains an instance of the specified service for use by the application.  
        /// </summary>
        /// <remarks>
        /// <para>
		/// Instead of returning the service directly, this overload passes the service to the specified delegate for use.
		/// When the delegate returns, this method automatically takes care of determing whether the service implements <see cref="IDisposable"/>
		/// and calling <see cref="IDisposable.Dispose"/> if it does.  The delegate must not cache the returned service
		/// because it may be disposed as soon as the delegate returns.  For the single-use scenario, this overload is preferred
		/// to the other overloads because it automatically manages the lifecycle of the service object.
		/// </para>
        /// <para>
        /// This method is thread-safe.
		/// </para>
        /// </remarks>
        /// <typeparam name="TService">The service to obtain.</typeparam>
        /// <param name="proc">A delegate that will receive the service for one-time use.</param>
        public static void GetService<TService>(WithServiceDelegate<TService> proc)
        {
            TService service = Platform.GetService<TService>();

            try
            {
                proc(service);
            }
            finally
            {
                if (service is IDisposable)
                {
                    try
                    {
                        (service as IDisposable).Dispose();
                    }
                    catch (Exception e)
                    {
                        // do not allow exceptions thrown from Dispose() because it may have the effect of
                        // hiding an exception that was thrown from the service itself
                        // if the service fails to dispose properly, we don't care, just log it and move on
                        Platform.Log(LogLevel.Error, e);
                    }
                }
            }
        }

        /// <summary>
        /// Obtains an instance of the specified service for use by the application.
        /// </summary>
        /// <remarks>
        /// This method is thread-safe.
        /// </remarks>
        /// <param name="service">The type of service to obtain.</param>
        /// <returns>An instance of the specified service.</returns>
        /// <exception cref="UnknownServiceException">The requested service cannot be provided.</exception>
        public static object GetService(Type service)
        {
            // load all service providers if not yet loaded
            if (_serviceProviders == null)
            {
                lock (_syncRoot)
                {
                    if (_serviceProviders == null)
                    {
                        _serviceProviders = Array.ConvertAll<object, IServiceProvider>(
                                                (new ServiceProviderExtensionPoint()).CreateExtensions(),
                                                    delegate(object sp) { return (IServiceProvider)sp; });
                    }
                }
            }

            // attempt to instantiate the requested service
            foreach (IServiceProvider sp in _serviceProviders)
            {
                // the service provider itself may not be thread-safe, so we need to ensure only one thread will access it
                // at a time
                lock (sp)
                {
                    object impl = sp.GetService(service);
                    if (impl != null)
                        return impl;
                }
            }
            throw new UnknownServiceException(string.Format(SR.ExceptionNoServiceProviderCanProvide, service.FullName));
        }

		/// <summary>
		/// Determines if the specified <see cref="LogLevel"/> is enabled.
		/// </summary>
		/// <param name="category">The logging level to check.</param>
		/// <returns>true if the <see cref="LogLevel"/> is enabled, or else false.</returns>
		public static bool IsLogLevelEnabled(LogLevel category)
		{
			switch (category)
			{
				case LogLevel.Debug:
					return _log.IsDebugEnabled;
				case LogLevel.Info:
					return _log.IsInfoEnabled;
				case LogLevel.Warn:
					return _log.IsWarnEnabled;
				case LogLevel.Error:
					return _log.IsErrorEnabled;
				case LogLevel.Fatal:
					return _log.IsFatalEnabled;
			}
			return false;
		}

        /// <summary>
        /// Logs the specified message at the specified <see cref="LogLevel"/>.
        /// </summary>
        /// <remarks>This method is thread-safe.</remarks>
        /// <param name="category">The logging level.</param>
		/// <param name="message">The message to be logged.</param>
		public static void Log(LogLevel category, object message)
        {
			// Just return without formatting if the log level isn't enabled
			if (!IsLogLevelEnabled(category)) return;

            Exception ex = message as Exception;
            if (ex != null)
            {
                switch (category)
                {
                    case LogLevel.Debug:
                        _log.Debug(SR.ExceptionThrown, ex);
                        break;
                    case LogLevel.Info:
                        _log.Info(SR.ExceptionThrown, ex);
                        break;
                    case LogLevel.Warn:
                        _log.Warn(SR.ExceptionThrown, ex);
                        break;
                    case LogLevel.Error:
                        _log.Error(SR.ExceptionThrown, ex);
                        break;
                    case LogLevel.Fatal:
                        _log.Fatal(SR.ExceptionThrown, ex);
                        break;
                }
            }
            else
            {
                switch (category)
                {
                    case LogLevel.Debug:
                        _log.Debug(message);
                        break;
                    case LogLevel.Info:
                        _log.Info(message);
                        break;
                    case LogLevel.Warn:
                        _log.Warn(message);
                        break;
                    case LogLevel.Error:
                        _log.Error(message);
                        break;
                    case LogLevel.Fatal:
                        _log.Fatal(message);
                        break;
                }
            }
        }

        /// <summary>
        /// Logs the specified message at the specified <see cref="LogLevel"/>.
        /// </summary>
        /// <remarks>This method is thread-safe.</remarks>
        /// <param name="category">The log level.</param>
        /// <param name="message">Format message, as used with <see cref="System.Text.StringBuilder"/>.</param>
        /// <param name="args">Optional arguments used with <paramref name="message"/>.</param>
        public static void Log(LogLevel category,String message, params object[] args)
        {
			// Just return without formatting if the log level isn't enabled
			if (!IsLogLevelEnabled(category)) return;

			StringBuilder sb = new StringBuilder();

			if (args == null || args.Length == 0)
				sb.Append(message);
			else
				sb.AppendFormat(message, args);

            switch (category)
            {
                case LogLevel.Debug:
                    _log.Debug(sb.ToString());
                    break;
                case LogLevel.Info:
                    _log.Info(sb.ToString());
                    break;
                case LogLevel.Warn:
                    _log.Warn(sb.ToString());
                    break;
                case LogLevel.Error:
                    _log.Error(sb.ToString());
                    break;
                case LogLevel.Fatal:
                    _log.Fatal(sb.ToString());
                    break;
            }
        }


        /// <summary>
        /// Logs the specified exception at the specified <see cref="LogLevel"/>.
        /// </summary>
        /// <remarks>This method is thread-safe.</remarks>
        /// <param name="ex">The exception to log.</param>
        /// <param name="category">The log level.</param>
        /// <param name="message">Format message, as used with <see cref="System.Text.StringBuilder"/>.</param>
        /// <param name="args">Optional arguments used with <paramref name="message"/>.</param>
        public static void Log(LogLevel category, Exception ex, String message, params object[] args)
        {
			// Just return without formatting if the log level isn't enabled
			if (!IsLogLevelEnabled(category)) return;

			StringBuilder sb = new StringBuilder();
            sb.AppendLine(SR.ExceptionThrown);
            sb.AppendLine();

			if (args == null || args.Length == 0)
				sb.Append(message);
			else
				sb.AppendFormat(message, args);
            
            switch (category)
            {
                case LogLevel.Debug:
                    _log.Debug(sb.ToString(), ex);
                    break;
                case LogLevel.Info:
                    _log.Info(sb.ToString(), ex);
                    break;
                case LogLevel.Warn:
                    _log.Warn(sb.ToString(), ex);
                    break;
                case LogLevel.Error:
                    _log.Error(sb.ToString(), ex);
                    break;
                case LogLevel.Fatal:
                    _log.Fatal(sb.ToString(), ex);
                    break;
            }

        }

        /// <summary>
        /// Displays a message box with the specified message.
        /// </summary>
        /// <remarks>
        /// This method is thread-safe, however displaying message boxes from a thread other than a UI
        /// thread is not a recommended practice.
        /// </remarks>
        [Obsolete("Use DesktopWindow.ShowMessageBox instead", false)]
        public static void ShowMessageBox(string message)
		{
            ShowMessageBox(message, MessageBoxActions.Ok);
		}

        /// <summary>
        /// Displays a message box with the specified message and buttons, and returns a value indicating the action
        /// taken by the user.
        /// </summary>
        /// <remarks>
        /// This method is thread-safe, however displaying message boxes from a thread other than a UI
        /// thread is not a recommended practice.
        /// </remarks>
        [Obsolete("Use DesktopWindow.ShowMessageBox instead", false)]
        public static DialogBoxAction ShowMessageBox(string message, MessageBoxActions buttons)
        {
            // create message box if does not exist
            if (_messageBox == null)
            {
                lock (_syncRoot)
                {
                    if (_messageBox == null)
                    {
                        MessageBoxExtensionPoint xp = new MessageBoxExtensionPoint();
                        _messageBox = (IMessageBox)xp.CreateExtension();
                    }
                }
            }

            // must lock here, because we have no guarantee that the underlying _messageBox object is thread-safe
            // lock on the _messageBox itself, rather than _syncRoot, so that _syncRoot is free for other threads to lock on
            // (i.e the message box may block this thread for a long time, and all other threads would halt if we locked on _syncRoot)
            lock (_messageBox)
            {
                return _messageBox.Show(message, buttons);
            }
        }


		/// <summary>
		/// Checks if a string is empty.
		/// </summary>
		/// <param name="variable">The string to check.</param>
		/// <param name="variableName">The variable name of the string to checked.</param>
		/// <exception cref="ArgumentNullException"><paramref name="variable"/> or or <paramref name="variableName"/>
		/// is <b>null</b>.</exception>
		/// <exception cref="ArgumentException"><paramref name="variable"/> is zero length.</exception>
		public static void CheckForEmptyString(string variable, string variableName)
		{
			CheckForNullReference(variable, variableName);
			CheckForNullReference(variableName, "variableName");

			if (variable.Length == 0)
				throw new ArgumentException(String.Format(SR.ExceptionEmptyString, variableName));
		}

		/// <summary>
		/// Checks if an object reference is null.
		/// </summary>
		/// <param name="variable">The object reference to check.</param>
		/// <param name="variableName">The variable name of the object reference to check.</param>
		/// <remarks>Use for checking if an input argument is <b>null</b>.  To check if a member variable
		/// is <b>null</b> (i.e., to see if an object is in a valid state), use <b>CheckMemberIsSet</b> instead.</remarks>
		/// <exception cref="ArgumentNullException"><paramref name="variable"/> or <paramref name="variableName"/>
		/// is <b>null</b>.</exception>
		public static void CheckForNullReference(object variable, string variableName)
		{
			if (variableName == null)
				throw new ArgumentNullException("variableName");

			if (null == variable)
				throw new ArgumentNullException(variableName);
		}

		/// <summary>
		/// Checks if an object is of the expected type.
		/// </summary>
		/// <param name="variable">The object to check.</param>
		/// <param name="type">The variable name of the object to check.</param>
		/// <exception cref="ArgumentNullException"><paramref name="variable"/> or <paramref name="type"/>
		/// is <b>null</b>.</exception>
		/// <exception cref="ArgumentException"><paramref name="type"/> is not the expected type.</exception>
		public static void CheckExpectedType(object variable, Type type)
		{
			CheckForNullReference(variable, "variable");
			CheckForNullReference(type, "type");

			if (!type.IsAssignableFrom(variable.GetType()))
				throw new ArgumentException(String.Format(SR.ExceptionExpectedType, type.FullName));
		}

		/// <summary>
		/// Checks if a cast is valid.
		/// </summary>
		/// <param name="castOutput">The object resulting from the cast.</param>
		/// <param name="castInputName">The variable name of the object that was cast.</param>
		/// <param name="castTypeName">The name of the type the object was cast to.</param>
		/// <remarks>
		/// <para>To use this method, casts have to be done using the <b>as</b> operator.  The
		/// method depends on failed casts resulting in <b>null</b>.</para>
		/// <para>This method has been deprecated since it does not actually perform any
		/// cast checking itself and entirely relies on correct usage (which is not apparent
		/// through the Visual Studio Intellisence feature) to function as an exception message
		/// formatter. The recommended practice is to use the <see cref="CheckExpectedType"/>
		/// if the cast output need not be consumed, or use the direct cast operator instead.</para>
		/// </remarks>
		/// <example>
		/// <code>
		/// [C#]
		/// layer = new GraphicLayer();
		/// GraphicLayer graphicLayer = layer as GraphicLayer;
		/// // No exception thrown
		/// Platform.CheckForInvalidCast(graphicLayer, "layer", "GraphicLayer");
		///
		/// ImageLayer image = layer as ImageLayer;
		/// // InvalidCastException thrown
		/// Platform.CheckForInvalidCast(image, "layer", "ImageLayer");
		/// </code>
		/// </example>
		/// <exception cref="ArgumentNullException"><paramref name="castOutput"/>,
		/// <paramref name="castInputName"/>, <paramref name="castTypeName"/> is <b>null</b>.</exception>
		/// <exception cref="InvalidCastException">Cast is invalid.</exception>
		[Obsolete("Use Platform.CheckExpectedType or perform a direct cast instead.")]
		public static void CheckForInvalidCast(object castOutput, string castInputName, string castTypeName)
		{
			Platform.CheckForNullReference(castOutput, "castOutput");
			Platform.CheckForNullReference(castInputName, "castInputName");
			Platform.CheckForNullReference(castTypeName, "castTypeName");

			if (castOutput == null)
				throw new InvalidCastException(String.Format(SR.ExceptionInvalidCast, castInputName, castTypeName));
		}

		/// <summary>
		/// Checks if a value is positive.
		/// </summary>
		/// <param name="n">The value to check.</param>
		/// <param name="variableName">The variable name of the value to check.</param>
		/// <exception cref="ArgumentNullException"><paramref name="variableName"/> is <b>null</b>.</exception>
		/// <exception cref="ArgumentException"><paramref name="n"/> &lt;= 0.</exception>
		public static void CheckPositive(int n, string variableName)
		{
			Platform.CheckForNullReference(variableName, "variableName");

			if (n <= 0)
				throw new ArgumentException(SR.ExceptionArgumentNotPositive, variableName);
		}


        /// <summary>
        /// Checks if a value is true.
        /// </summary>
        /// <param name="testTrueCondition">The value to check.</param>
        /// <param name="conditionName">The name of the condition to check.</param>
        /// <exception cref="ArgumentNullException"><paramref name="conditionName"/> is <b>null</b>.</exception>
        /// <exception cref="ArgumentException"><paramref name="testTrueCondition"/> is  <b>false</b>.</exception>
        public static void CheckTrue(bool testTrueCondition, string conditionName)
        {
            Platform.CheckForNullReference(conditionName, "conditionName");

            if (testTrueCondition != true)
                throw new ArgumentException(String.Format(SR.ExceptionConditionIsNotMet, conditionName));
        }


        /// <summary>
        /// Checks if a value is false.
        /// </summary>
        /// <param name="testFalseCondition">The value to check.</param>
        /// <param name="conditionName">The name of the condition to check.</param>
        /// <exception cref="ArgumentNullException"><paramref name="conditionName"/> is <b>null</b>.</exception>
        /// <exception cref="ArgumentException"><paramref name="testFalseCondition"/> is  <b>true</b>.</exception>
        public static void CheckFalse(bool testFalseCondition, string conditionName)
        {
            Platform.CheckForNullReference(conditionName, "conditionName");

            if (testFalseCondition != false)
				throw new ArgumentException(String.Format(SR.ExceptionConditionIsNotMet, conditionName));
        }

		/// <summary>
		/// Checks if a value is positive.
		/// </summary>
		/// <param name="x">The value to check.</param>
		/// <param name="variableName">The variable name of the value to check.</param>
		/// <exception cref="ArgumentNullException"><paramref name="variableName"/> is <b>null</b>.</exception>
		/// <exception cref="ArgumentException"><paramref name="x"/> &lt;= 0.</exception>
		public static void CheckPositive(float x, string variableName)
		{
			Platform.CheckForNullReference(variableName, "variableName");

			if (x <= 0.0f)
				throw new ArgumentException(SR.ExceptionArgumentNotPositive, variableName);
		}

		/// <summary>
		/// Checks if a value is positive.
		/// </summary>
		/// <param name="x">The value to check.</param>
		/// <param name="variableName">The variable name of the value to check.</param>
		/// <exception cref="ArgumentNullException"><paramref name="variableName"/> is <b>null</b>.</exception>
		/// <exception cref="ArgumentException"><paramref name="x"/> &lt;= 0.</exception>
		public static void CheckPositive(double x, string variableName)
		{
			Platform.CheckForNullReference(variableName, "variableName");

			if (x <= 0.0d)
				throw new ArgumentException(SR.ExceptionArgumentNotPositive, variableName);
		}

		/// <summary>
		/// Checks if a value is non-negative.
		/// </summary>
		/// <param name="n">The value to check.</param>
		/// <param name="variableName">The variable name of the value to check.</param>
		/// <exception cref="ArgumentNullException"><paramref name="variableName"/> is <b>null</b>.</exception>
		/// <exception cref="ArgumentException"><paramref name="n"/> &lt; 0.</exception>
		public static void CheckNonNegative(int n, string variableName)
		{
			Platform.CheckForNullReference(variableName, "variableName");

			if (n < 0)
				throw new ArgumentException(SR.ExceptionArgumentNegative, variableName);
		}

		/// <summary>
		/// Checks if a value is within a specified range.
		/// </summary>
		/// <param name="argumentValue">Value to be checked.</param>
		/// <param name="min">Minimum value.</param>
		/// <param name="max">Maximum value.</param>
		/// <param name="variableName">Variable name of value to be checked.</param>
		/// <remarks>Checks if <paramref name="min"/> &lt;= <paramref name="argumentValue"/> &lt;= <paramref name="max"/></remarks>
		/// <exception cref="ArgumentNullException"><paramref name="variableName"/> is <b>null</b>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="argumentValue"/> is not within the
		/// specified range.</exception>
		public static void CheckArgumentRange(int argumentValue, int min, int max, string variableName)
		{
			Platform.CheckForNullReference(variableName, "variableName");

			if (argumentValue < min || argumentValue > max)
				throw new ArgumentOutOfRangeException(String.Format(SR.ExceptionArgumentOutOfRange, argumentValue, min, max, variableName));
		}

		/// <summary>
		/// Checks if an index is within a specified range.
		/// </summary>
		/// <param name="index">Index to be checked</param>
		/// <param name="min">Minimum value.</param>
		/// <param name="max">Maximum value.</param>
		/// <param name="obj">Object being indexed.</param>
		/// <remarks>Checks if <paramref name="min"/> &lt;= <paramref name="index"/> &lt;= <paramref name="max"/>.</remarks>
		/// <exception cref="ArgumentNullException"><paramref name="obj"/> is <b>null</b>.</exception>
		/// <exception cref="IndexOutOfRangeException"><paramref name="index"/> is not within the
		/// specified range.</exception>
		public static void CheckIndexRange(int index, int min, int max, object obj)
		{
			Platform.CheckForNullReference(obj, "obj");

			if (index < min || index > max)
				throw new IndexOutOfRangeException(String.Format(SR.ExceptionIndexOutOfRange, index, min, max, obj.GetType().Name));
		}

		/// <summary>
		/// Checks if a field or property is null.
		/// </summary>
		/// <param name="variable">Field or property to be checked.</param>
		/// <param name="variableName">Name of field or property to be checked.</param>
		/// <remarks>Use this method in your classes to verify that the object
		/// is not in an invalid state by checking that various fields and/or properties
		/// have been set, i.e., are not null.</remarks>
		/// <exception cref="ArgumentNullException"><paramref name="variableName"/> is <b>null</b>.</exception>
		/// <exception cref="System.InvalidOperationException"><paramref name="variable"/> is <b>null</b>.</exception>
		public static void CheckMemberIsSet(object variable, string variableName)
		{
			Platform.CheckForNullReference(variableName, "variableName");

			if (variable == null)
				throw new InvalidOperationException(String.Format(SR.ExceptionMemberNotSet, variableName));
		}

		/// <summary>
		/// Checks if a field or property is null.
		/// </summary>
		/// <param name="variable">Field or property to be checked.</param>
		/// <param name="variableName">Name of field or property to be checked.</param>
		/// <param name="detailedMessage">A more detailed and informative message describing
		/// why the object is in an invalid state.</param>
		/// <remarks>Use this method in your classes to verify that the object
		/// is not in an invalid state by checking that various fields and/or properties
		/// have been set, i.e., are not null.</remarks>
		/// <exception cref="ArgumentNullException"><paramref name="variableName"/> is <b>null</b>.</exception>
		/// <exception cref="System.InvalidOperationException"><paramref name="variable"/> is <b>null</b>.</exception>
		public static void CheckMemberIsSet(object variable, string variableName, string detailedMessage)
		{
			Platform.CheckForNullReference(variableName, "variableName");
			Platform.CheckForNullReference(detailedMessage, "detailedMessage");

			if (variable == null)
				throw new InvalidOperationException(String.Format(SR.ExceptionMemberNotSetVerbose, variableName, detailedMessage));
		}


        
    }
}
