using System;
using Mono.Addins;

namespace CocoStudio.Model.ExtensionModel
{
	// Token: 0x02000085 RID: 133
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class FrameExtensionAttribute : CustomExtensionAttribute
	{
		// Token: 0x17000149 RID: 329
		// (get) Token: 0x0600048C RID: 1164 RVA: 0x00013DF8 File Offset: 0x00011FF8
		// (set) Token: 0x0600048D RID: 1165 RVA: 0x00013E0F File Offset: 0x0001200F
		public Type DataType { get; private set; }

		// Token: 0x0600048E RID: 1166 RVA: 0x00013E18 File Offset: 0x00012018
		public FrameExtensionAttribute()
		{
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x00013E23 File Offset: 0x00012023
		public FrameExtensionAttribute(Type dateType)
		{
			this.DataType = dateType;
		}
	}
}
