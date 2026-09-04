using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Remoting;
using System.Threading;
using Microsoft.Build.BuildEngine;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	public class BuildEngine : MarshalByRefObject, IBuildEngine, IDisposable
	{
		private static readonly AutoResetEvent workDoneEvent = new AutoResetEvent(initialState: false);

		private static ThreadStart workDelegate;

		private static readonly object workLock = new object();

		private static Thread workThread;

		private static CultureInfo uiCulture;

		private static Exception workError;

		private readonly ManualResetEvent doneEvent = new ManualResetEvent(initialState: false);

		private readonly Dictionary<string, string> unsavedProjects = new Dictionary<string, string>();

		internal readonly Engine Engine = new Engine
		{
			DefaultToolsVersion = "4.0"
		};

		private static readonly object threadLock = new object();

		internal WaitHandle WaitHandle => doneEvent;

		public void Dispose()
		{
			doneEvent.Set();
		}

		public void Ping()
		{
		}

		public void SetCulture(CultureInfo uiCulture)
		{
			BuildEngine.uiCulture = uiCulture;
		}

		public void SetGlobalProperties(IDictionary<string, string> properties)
		{
			BuildPropertyGroup globalProperties = Engine.GlobalProperties;
			foreach (KeyValuePair<string, string> property in properties)
			{
				globalProperties.SetProperty(property.Key, property.Value);
			}
		}

		public IProjectBuilder LoadProject(string file)
		{
			return new ProjectBuilder(this, file);
		}

		public void UnloadProject(IProjectBuilder pb)
		{
			((ProjectBuilder)pb).Dispose();
			RemotingServices.Disconnect((MarshalByRefObject)pb);
		}

		public override object InitializeLifetimeService()
		{
			return null;
		}

		internal void UnloadProject(string file)
		{
			lock (unsavedProjects)
			{
				unsavedProjects.Remove(file);
			}
			RunSTA(delegate
			{
				Project loadedProject = Engine.GetLoadedProject(file);
				if (loadedProject != null)
				{
					Engine.UnloadProject(loadedProject);
				}
			});
		}

		internal void SetUnsavedProjectContent(string file, string content)
		{
			lock (unsavedProjects)
			{
				unsavedProjects[file] = content;
			}
		}

		internal string GetUnsavedProjectContent(string file)
		{
			lock (unsavedProjects)
			{
				unsavedProjects.TryGetValue(file, out var value);
				return value;
			}
		}

		internal static void RunSTA(ThreadStart ts)
		{
			lock (workLock)
			{
				lock (threadLock)
				{
					workDelegate = ts;
					workError = null;
					if (workThread == null)
					{
						workThread = new Thread(STARunner);
						workThread.SetApartmentState(ApartmentState.STA);
						workThread.IsBackground = true;
						workThread.CurrentUICulture = uiCulture;
						workThread.Start();
					}
					else
					{
						Monitor.Pulse(threadLock);
					}
				}
				workDoneEvent.WaitOne();
			}
			if (workError != null)
			{
				throw new Exception("MSBuild operation failed", workError);
			}
		}

		private static void STARunner()
		{
			lock (threadLock)
			{
				do
				{
					try
					{
						workDelegate();
					}
					catch (Exception ex)
					{
						workError = ex;
					}
					workDoneEvent.Set();
				}
				while (Monitor.Wait(threadLock, 60000));
				workThread = null;
			}
		}
	}
}
