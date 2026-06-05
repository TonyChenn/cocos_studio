using System;
using System.Collections.Generic;
using Mono.Addins;
using MonoDevelop.Core.Execution;

namespace MonoDevelop.Core.AddIns
{
	// Token: 0x020000C3 RID: 195
	[ExtensionNodeChild(typeof(ExecutionModeNode), "Mode")]
	internal class ExecutionModeSetNode : ExtensionNode, IExecutionModeSet
	{
		// Token: 0x17000176 RID: 374
		// (get) Token: 0x060006AC RID: 1708 RVA: 0x0001A919 File Offset: 0x00018B19
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x060006AD RID: 1709 RVA: 0x0001AAC4 File Offset: 0x00018CC4
		public IEnumerable<IExecutionMode> ExecutionModes
		{
			get
			{
				foreach (object obj in base.ChildNodes)
				{
					ExecutionModeNode node = (ExecutionModeNode)obj;
					yield return node;
				}
				yield break;
			}
		}

		// Token: 0x04000231 RID: 561
		[NodeAttribute("_name", Localizable = true)]
		private string name;
	}
}
