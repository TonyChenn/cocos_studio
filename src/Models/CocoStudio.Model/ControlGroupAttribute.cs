using System;

namespace CocoStudio.Model
{
	// Token: 0x020000D9 RID: 217
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public class ControlGroupAttribute : Attribute
	{
		// Token: 0x170001DC RID: 476
		// (get) Token: 0x060006B9 RID: 1721 RVA: 0x0001AD94 File Offset: 0x00018F94
		// (set) Token: 0x060006BA RID: 1722 RVA: 0x0001ADAB File Offset: 0x00018FAB
		public string GroupName { get; set; }

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x060006BB RID: 1723 RVA: 0x0001ADB4 File Offset: 0x00018FB4
		// (set) Token: 0x060006BC RID: 1724 RVA: 0x0001ADCB File Offset: 0x00018FCB
		public int Order { get; set; }

		// Token: 0x060006BD RID: 1725 RVA: 0x0001ADD4 File Offset: 0x00018FD4
		public ControlGroupAttribute(string groupName, int order)
		{
			this.GroupName = groupName;
			this.Order = order;
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0001ADF0 File Offset: 0x00018FF0
		public override bool Equals(object obj)
		{
			ControlGroupAttribute controlGroupAttribute = obj as ControlGroupAttribute;
			return this.GroupName.Equals(controlGroupAttribute.GroupName);
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x0001AE1C File Offset: 0x0001901C
		public override int GetHashCode()
		{
			return this.GroupName.GetHashCode();
		}
	}
}
