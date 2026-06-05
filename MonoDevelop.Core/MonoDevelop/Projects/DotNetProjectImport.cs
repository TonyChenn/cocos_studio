using System;

namespace MonoDevelop.Projects
{
	// Token: 0x0200026B RID: 619
	internal class DotNetProjectImport
	{
		// Token: 0x0600165C RID: 5724 RVA: 0x0005A506 File Offset: 0x00058706
		internal DotNetProjectImport(string name, string condition = null)
		{
			this.Name = name;
			this.Condition = condition;
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x0600165D RID: 5725 RVA: 0x0005A51C File Offset: 0x0005871C
		// (set) Token: 0x0600165E RID: 5726 RVA: 0x0005A524 File Offset: 0x00058724
		public string Name { get; set; }

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x0600165F RID: 5727 RVA: 0x0005A52D File Offset: 0x0005872D
		// (set) Token: 0x06001660 RID: 5728 RVA: 0x0005A535 File Offset: 0x00058735
		public string Condition { get; set; }

		// Token: 0x06001661 RID: 5729 RVA: 0x0005A53E File Offset: 0x0005873E
		public bool HasCondition()
		{
			return !string.IsNullOrEmpty(this.Condition);
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x0005A550 File Offset: 0x00058750
		public override bool Equals(object obj)
		{
			DotNetProjectImport dotNetProjectImport = obj as DotNetProjectImport;
			return dotNetProjectImport != null && dotNetProjectImport.Name == this.Name;
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x0005A57A File Offset: 0x0005877A
		public override int GetHashCode()
		{
			return this.Name.GetHashCode();
		}
	}
}
