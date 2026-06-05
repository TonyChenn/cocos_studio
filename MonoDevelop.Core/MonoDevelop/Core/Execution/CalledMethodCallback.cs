using System;
using System.Runtime.Remoting.Messaging;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x020000C8 RID: 200
	// (Invoke) Token: 0x060006C6 RID: 1734
	public delegate IMethodReturnMessage CalledMethodCallback(object obj, IMethodCallMessage msg, IMethodReturnMessage ret, bool timedOut);
}
