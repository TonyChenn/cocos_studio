using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component.GUI
{
	// Token: 0x0200002E RID: 46
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class WidgetTreeSurrogate : BaseEntitySurrogate
	{
		// Token: 0x1700015E RID: 350
		// (get) Token: 0x0600034C RID: 844 RVA: 0x00008687 File Offset: 0x00006887
		// (set) Token: 0x0600034D RID: 845 RVA: 0x0000868F File Offset: 0x0000688F
		[DataMember]
		public WidgetSurrogate options { get; set; }

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x0600034E RID: 846 RVA: 0x00008698 File Offset: 0x00006898
		// (set) Token: 0x0600034F RID: 847 RVA: 0x000086A0 File Offset: 0x000068A0
		[DataMember]
		public List<WidgetTreeSurrogate> children { get; set; }

		// Token: 0x06000350 RID: 848 RVA: 0x000086A9 File Offset: 0x000068A9
		protected WidgetTreeSurrogate()
		{
		}

		// Token: 0x06000351 RID: 849 RVA: 0x000086B1 File Offset: 0x000068B1
		public WidgetTreeSurrogate(WidgetSurrogate guiControlSurrogate)
		{
			this.classname = guiControlSurrogate.classname;
			this.options = guiControlSurrogate;
			this.ConvertChildren();
		}

		// Token: 0x06000352 RID: 850 RVA: 0x000086D2 File Offset: 0x000068D2
		private void ConvertChildren()
		{
		}

		// Token: 0x06000353 RID: 851 RVA: 0x000086D4 File Offset: 0x000068D4
		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			this.options.ConvertToObject();
		}
	}
}
