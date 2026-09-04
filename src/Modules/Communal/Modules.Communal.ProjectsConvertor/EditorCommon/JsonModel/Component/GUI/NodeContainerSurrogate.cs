using System;
using System.Runtime.Serialization;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component.GUI
{
	// Token: 0x02000025 RID: 37
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class NodeContainerSurrogate : WidgetSurrogate
	{
		// Token: 0x060001F7 RID: 503 RVA: 0x000071DC File Offset: 0x000053DC
		protected NodeContainerSurrogate()
		{
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x000071E4 File Offset: 0x000053E4
		public override void SetValue(object obj)
		{
			base.SetValue(obj);
		}
	}
}
