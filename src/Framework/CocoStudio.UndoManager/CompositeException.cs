using System;
using System.Collections.Generic;

namespace CocoStudio.UndoManager
{
	// Token: 0x02000003 RID: 3
	[Serializable]
	public class CompositeException : Exception
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002370 File Offset: 0x00000570
		// (set) Token: 0x0600000F RID: 15 RVA: 0x00002387 File Offset: 0x00000587
		public IEnumerable<Exception> Exceptions { get; private set; }

		// Token: 0x06000010 RID: 16 RVA: 0x00002390 File Offset: 0x00000590
		public CompositeException(string message, IEnumerable<Exception> exceptions) : base(message)
		{
			this.Exceptions = exceptions;
		}
	}
}
