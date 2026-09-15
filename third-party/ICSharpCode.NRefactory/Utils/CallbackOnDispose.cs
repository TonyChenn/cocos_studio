using System;
using System.Threading;

namespace ICSharpCode.NRefactory.Utils
{
	/// <summary>
	/// Invokes an action when it is disposed.
	/// </summary>
	/// <remarks>
	/// This class ensures the callback is invoked at most once,
	/// even when Dispose is called on multiple threads.
	/// </remarks>
	public sealed class CallbackOnDispose : IDisposable
	{
		public CallbackOnDispose(Action action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			this.action = action;
		}

		public void Dispose()
		{
			Action action = Interlocked.Exchange<Action>(ref this.action, null);
			if (action != null)
			{
				action();
			}
		}

		private Action action;
	}
}
