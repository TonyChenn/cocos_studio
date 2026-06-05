using System;

namespace CocoStudio.Model
{
	// Token: 0x02000009 RID: 9
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public class RequestOperationModeAttribute : Attribute
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002B74 File Offset: 0x00000D74
		// (set) Token: 0x06000049 RID: 73 RVA: 0x00002B8B File Offset: 0x00000D8B
		public OperationMask RequestMode { get; private set; }

		// Token: 0x0600004A RID: 74 RVA: 0x00002B94 File Offset: 0x00000D94
		public RequestOperationModeAttribute(OperationMask requestMode)
		{
			this.RequestMode = requestMode;
		}
	}
}
