using System;
using Mono.Addins;

namespace CocoStudio.Model.ExtensionModel
{
	// Token: 0x02000086 RID: 134
	internal class FrameExtensionNode : TypeExtensionNode<FrameExtensionAttribute>
	{
		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000490 RID: 1168 RVA: 0x00013E38 File Offset: 0x00012038
		// (set) Token: 0x06000491 RID: 1169 RVA: 0x00013E4F File Offset: 0x0001204F
		public new FrameExtensionAttribute Data { get; private set; }

		// Token: 0x06000492 RID: 1170 RVA: 0x00013E58 File Offset: 0x00012058
		protected override void Read(NodeElement elem)
		{
			base.Read(elem);
			object[] customAttributes = base.Type.GetCustomAttributes(typeof(FrameExtensionAttribute), false);
			if (customAttributes.Length > 0)
			{
				this.Data = (customAttributes[0] as FrameExtensionAttribute);
			}
			else
			{
				this.Data = base.Data;
			}
		}
	}
}
