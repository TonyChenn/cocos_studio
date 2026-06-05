using System;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects
{
	// Token: 0x02000171 RID: 369
	public class SolutionItemReference
	{
		// Token: 0x06000E70 RID: 3696 RVA: 0x000358AB File Offset: 0x00033AAB
		internal SolutionItemReference()
		{
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x000358B4 File Offset: 0x00033AB4
		public SolutionItemReference(SolutionItem item)
		{
			if (item is SolutionEntityItem)
			{
				this.path = ((SolutionEntityItem)item).FileName;
				return;
			}
			this.path = item.ParentSolution.FileName;
			if (item is SolutionFolder && ((SolutionFolder)item).IsRoot)
			{
				this.id = ":root:";
				return;
			}
			this.id = item.ItemId;
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x0003591F File Offset: 0x00033B1F
		public SolutionItemReference(FilePath path)
		{
			this.path = path;
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x0003592E File Offset: 0x00033B2E
		public SolutionItemReference(FilePath path, string id)
		{
			this.path = path;
			this.id = id;
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000E74 RID: 3700 RVA: 0x00035944 File Offset: 0x00033B44
		internal FilePath Path
		{
			get
			{
				return this.path;
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000E75 RID: 3701 RVA: 0x0003594C File Offset: 0x00033B4C
		internal string Id
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x06000E76 RID: 3702 RVA: 0x00035954 File Offset: 0x00033B54
		public override bool Equals(object o)
		{
			SolutionItemReference solutionItemReference = o as SolutionItemReference;
			return o != null && this.path == solutionItemReference.path && this.id == solutionItemReference.id;
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x00035993 File Offset: 0x00033B93
		public override int GetHashCode()
		{
			return (this.Path + this.id).GetHashCode();
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x000359B0 File Offset: 0x00033BB0
		public static bool operator ==(SolutionItemReference r1, SolutionItemReference r2)
		{
			return object.ReferenceEquals(r1, r2) || (r1 != null && r2 != null && r1.Equals(r2));
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x000359CC File Offset: 0x00033BCC
		public static bool operator !=(SolutionItemReference r1, SolutionItemReference r2)
		{
			return !(r1 == r2);
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x000359D8 File Offset: 0x00033BD8
		public override string ToString()
		{
			return string.Format("[SolutionItemReference: path={0}, id={1}]", this.path, this.id);
		}

		// Token: 0x04000424 RID: 1060
		[ProjectPathItemProperty]
		private FilePath path;

		// Token: 0x04000425 RID: 1061
		[ItemProperty]
		private string id;
	}
}
