using System;
using Mono.Addins;
using MonoDevelop.Core.Execution;

namespace MonoDevelop.Core.AddIns
{
	// Token: 0x0200005B RID: 91
	internal class ExecutionModeNode : TypeExtensionNode, IExecutionMode
	{
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x0000B34D File Offset: 0x0000954D
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x0000B355 File Offset: 0x00009555
		public IExecutionHandler ExecutionHandler
		{
			get
			{
				return (IExecutionHandler)base.GetInstance(typeof(IExecutionHandler));
			}
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000B374 File Offset: 0x00009574
		string IExecutionMode.get_Id()
		{
			return base.Id;
		}

		// Token: 0x04000115 RID: 277
		[NodeAttribute("_name", Localizable = true)]
		private string name;
	}
}
