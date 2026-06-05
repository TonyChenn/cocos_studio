using System;
using System.Collections.Generic;
using MonoDevelop.Core;
using MonoDevelop.Core.Assemblies;
using MonoDevelop.Projects.Extensions;

namespace MonoDevelop.Projects
{
	// Token: 0x02000172 RID: 370
	public class FileFormat
	{
		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000E7B RID: 3707 RVA: 0x000359F5 File Offset: 0x00033BF5
		public string Id
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000E7C RID: 3708 RVA: 0x000359FD File Offset: 0x00033BFD
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000E7D RID: 3709 RVA: 0x00035A05 File Offset: 0x00033C05
		// (set) Token: 0x06000E7E RID: 3710 RVA: 0x00035A0D File Offset: 0x00033C0D
		public bool CanDefault { get; private set; }

		// Token: 0x06000E7F RID: 3711 RVA: 0x00035A16 File Offset: 0x00033C16
		public string GetValidFileName(object obj, string fileName)
		{
			return this.format.GetValidFormatName(obj, fileName);
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x00035A30 File Offset: 0x00033C30
		public IEnumerable<string> GetCompatibilityWarnings(object obj)
		{
			IWorkspaceFileObject workspaceFileObject = obj as IWorkspaceFileObject;
			if (workspaceFileObject != null && !workspaceFileObject.SupportsFormat(this))
			{
				return new string[]
				{
					GettextCatalog.GetString("The project '{0}' is not supported by {1}", workspaceFileObject.Name, this.Name)
				};
			}
			IEnumerable<string> compatibilityWarnings = this.format.GetCompatibilityWarnings(obj);
			return compatibilityWarnings ?? ((IEnumerable<string>)new string[0]);
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x00035A90 File Offset: 0x00033C90
		public bool CanWrite(object obj)
		{
			IWorkspaceFileObject workspaceFileObject = obj as IWorkspaceFileObject;
			return (workspaceFileObject == null || workspaceFileObject.SupportsFormat(this)) && this.format.CanWriteFile(obj);
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000E82 RID: 3714 RVA: 0x00035ABE File Offset: 0x00033CBE
		public bool SupportsMixedFormats
		{
			get
			{
				return this.format.SupportsMixedFormats;
			}
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x00035ACB File Offset: 0x00033CCB
		public bool SupportsFramework(TargetFramework framework)
		{
			return this.format.SupportsFramework(framework);
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000E84 RID: 3716 RVA: 0x00035AD9 File Offset: 0x00033CD9
		internal IFileFormat Format
		{
			get
			{
				return this.format;
			}
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x00035AE1 File Offset: 0x00033CE1
		internal FileFormat(IFileFormat format, string id, string name) : this(format, id, name, false)
		{
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x00035AED File Offset: 0x00033CED
		internal FileFormat(IFileFormat format, string id, string name, bool canDefault)
		{
			this.id = id;
			this.name = (name ?? id);
			this.format = format;
			this.CanDefault = canDefault;
		}

		// Token: 0x04000426 RID: 1062
		private string id;

		// Token: 0x04000427 RID: 1063
		private string name;

		// Token: 0x04000428 RID: 1064
		private IFileFormat format;
	}
}
