using System;
using System.Threading;
using MonoDevelop.Core;
using MonoDevelop.Core.Execution;

namespace MonoDevelop.Debugger
{
	internal class DebugExecutionHandler : IProcessAsyncOperation, IAsyncOperation, IDisposable
	{
		private bool done;

		private ManualResetEvent stopEvent;

		private DebuggerEngine factory;

		public int ExitCode => 0;

		public bool IsCompleted => done;

		public bool Success => true;

		public bool SuccessWithWarnings => true;

		public int ProcessId => -1;

		event OperationHandler IAsyncOperation.Completed
		{
			add
			{
				bool flag = false;
				lock (this)
				{
					if (done)
					{
						flag = true;
					}
					else
					{
						completedEvent += value;
					}
				}
				if (flag)
				{
					value(this);
				}
			}
			remove
			{
				lock (this)
				{
					completedEvent -= value;
				}
			}
		}

		private event OperationHandler completedEvent;

		public DebugExecutionHandler(DebuggerEngine factory)
		{
			this.factory = factory;
			DebuggingService.StoppedEvent += OnStopDebug;
		}

		public IProcessAsyncOperation Execute(ExecutionCommand command, IConsole console)
		{
			DebuggingService.InternalRun(command, factory, console);
			return this;
		}

		public void Cancel()
		{
			DebuggingService.Stop();
		}

		public void WaitForCompleted()
		{
			lock (this)
			{
				if (done)
				{
					return;
				}
				if (stopEvent == null)
				{
					stopEvent = new ManualResetEvent(initialState: false);
				}
			}
			stopEvent.WaitOne();
		}

		private void OnStopDebug(object sender, EventArgs args)
		{
			lock (this)
			{
				done = true;
				if (stopEvent != null)
				{
					stopEvent.Set();
				}
				if (completedEvent != null)
				{
					completedEvent(this);
				}
			}
			DebuggingService.StoppedEvent -= OnStopDebug;
		}

		void IDisposable.Dispose()
		{
		}
	}
}
