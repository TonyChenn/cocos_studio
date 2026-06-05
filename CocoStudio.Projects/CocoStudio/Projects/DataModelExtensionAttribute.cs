using System;
using Mono.Addins;

namespace CocoStudio.Projects
{
	// Token: 0x02000023 RID: 35
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class DataModelExtensionAttribute : CustomExtensionAttribute
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x000040E0 File Offset: 0x000022E0
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x000040E8 File Offset: 0x000022E8
		public Type ModelType { get; private set; }

		// Token: 0x060000C3 RID: 195 RVA: 0x000040F1 File Offset: 0x000022F1
		public DataModelExtensionAttribute()
		{
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000040F9 File Offset: 0x000022F9
		public DataModelExtensionAttribute(Type modelType)
		{
			this.ModelType = modelType;
		}
	}
}
