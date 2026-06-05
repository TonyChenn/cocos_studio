using System;
using Mono.Addins;

namespace MonoDevelop.Core.AddIns
{
	// Token: 0x02000219 RID: 537
	internal class UserDataMigrationNode : ExtensionNode
	{
		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x0600142E RID: 5166 RVA: 0x00053AA9 File Offset: 0x00051CA9
		public string SourceVersion
		{
			get
			{
				return this.sourceVersion;
			}
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x0600142F RID: 5167 RVA: 0x00053AB1 File Offset: 0x00051CB1
		public FilePath SourcePath
		{
			get
			{
				return FileService.MakePathSeparatorsNative(this.path);
			}
		}

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x06001430 RID: 5168 RVA: 0x00053AC3 File Offset: 0x00051CC3
		public FilePath TargetPath
		{
			get
			{
				return FileService.MakePathSeparatorsNative(string.IsNullOrEmpty(this.targetPath) ? this.path : this.targetPath);
			}
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x06001431 RID: 5169 RVA: 0x00053AEA File Offset: 0x00051CEA
		public UserDataKind SourceKind
		{
			get
			{
				return this.kind;
			}
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x06001432 RID: 5170 RVA: 0x00053AF2 File Offset: 0x00051CF2
		public UserDataKind TargetKind
		{
			get
			{
				if (!string.IsNullOrEmpty(this.targetKind))
				{
					return (UserDataKind)Enum.Parse(typeof(UserDataKind), this.targetKind);
				}
				return this.kind;
			}
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x00053B22 File Offset: 0x00051D22
		public IUserDataMigrationHandler GetHandler()
		{
			if (string.IsNullOrEmpty(this.handlerTypeName))
			{
				return null;
			}
			return (IUserDataMigrationHandler)Activator.CreateInstance(base.Addin.GetType(this.handlerTypeName));
		}

		// Token: 0x0400060C RID: 1548
		[NodeAttribute(Required = true, Description = "The version of the source profile for this migration applies.")]
		private string sourceVersion;

		// Token: 0x0400060D RID: 1549
		[NodeAttribute(Required = true, Description = "The relative path of the data")]
		private string path;

		// Token: 0x0400060E RID: 1550
		[NodeAttribute(Description = "The relative path of the target, if it differs from the source")]
		private string targetPath;

		// Token: 0x0400060F RID: 1551
		[NodeAttribute(Required = true, Description = "The kind of the data to be migrated")]
		private UserDataKind kind;

		// Token: 0x04000610 RID: 1552
		[NodeAttribute(Description = "The kind of the target, if it differs from the source")]
		private string targetKind;

		// Token: 0x04000611 RID: 1553
		[NodeAttribute(Name = "handler")]
		private string handlerTypeName;
	}
}
