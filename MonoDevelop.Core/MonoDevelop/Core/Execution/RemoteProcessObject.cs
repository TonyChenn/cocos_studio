using System;
using System.Runtime.Remoting;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x0200001A RID: 26
	public class RemoteProcessObject : MarshalByRefObject, IDisposable
	{
		/// <summary>
		/// Disposes the object, and kills the remote process if there are no more remote objects running on it
		/// </summary>
		// Token: 0x060000CF RID: 207 RVA: 0x000053E9 File Offset: 0x000035E9
		public virtual void Dispose()
		{
			RemotingServices.Disconnect(this);
		}

		/// <summary>
		/// Shutdowns the remote process that is running this instance.
		/// </summary>
		/// <remarks>
		/// This method can only be used if the remote process is not shared with other objects.
		/// </remarks>
		// Token: 0x060000D0 RID: 208 RVA: 0x000053F2 File Offset: 0x000035F2
		public void Shutdown()
		{
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000053F4 File Offset: 0x000035F4
		public override object InitializeLifetimeService()
		{
			return null;
		}
	}
}
