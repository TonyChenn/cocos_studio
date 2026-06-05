using System;
using Mono.Addins;

namespace MonoDevelop.Core.AddIns
{
	// Token: 0x02000045 RID: 69
	[NodeAttribute("description", typeof(string), false, Description = "Description of the tool")]
	[NodeAttribute("class", typeof(Type), true, Description = "Name of the class")]
	internal class ApplicationExtensionNode : ExtensionNode, IApplicationInfo
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000237 RID: 567 RVA: 0x00008F13 File Offset: 0x00007113
		public string TypeName
		{
			get
			{
				return this.typeName;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000238 RID: 568 RVA: 0x00008F1B File Offset: 0x0000711B
		public string Description
		{
			get
			{
				return this.description;
			}
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00008F24 File Offset: 0x00007124
		protected override void Read(NodeElement elem)
		{
			this.typeName = elem.GetAttribute("class");
			if (this.typeName.Length == 0)
			{
				this.typeName = elem.GetAttribute("type");
			}
			if (this.typeName.Length == 0)
			{
				throw new InvalidOperationException("Application type not provided");
			}
			this.description = elem.GetAttribute("description");
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00008F89 File Offset: 0x00007189
		public object CreateInstance()
		{
			return Activator.CreateInstance(base.Addin.GetType(this.typeName, true));
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00008FAA File Offset: 0x000071AA
		string IApplicationInfo.get_Id()
		{
			return base.Id;
		}

		// Token: 0x040000CA RID: 202
		private string typeName;

		// Token: 0x040000CB RID: 203
		private string description;
	}
}
