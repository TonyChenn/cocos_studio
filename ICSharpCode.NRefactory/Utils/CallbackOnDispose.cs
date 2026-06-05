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
	// Token: 0x0200010A RID: 266
	public sealed class CallbackOnDispose : IDisposable
	{
		// Token: 0x06000992 RID: 2450 RVA: 0x0001999E File Offset: 0x0001899E
		public CallbackOnDispose(Action action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			this.action = action;
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x000199BC File Offset: 0x000189BC
		public void Dispose()
		{
			Action action = Interlocked.Exchange<Action>(ref this.action, null);
			if (action != null)
			{
				action();
			}
		}

		// Token: 0x04000317 RID: 791
		private Action action;
	}
}
