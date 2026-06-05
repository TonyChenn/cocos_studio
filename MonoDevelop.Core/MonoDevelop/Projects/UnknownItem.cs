using System;
using System.Collections;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects
{
	// Token: 0x02000150 RID: 336
	internal class UnknownItem : IBuildTarget, IWorkspaceObject, IExtendedDataItem, IFolderItem, IDisposable
	{
		// Token: 0x06000C6F RID: 3183 RVA: 0x0002DEC4 File Offset: 0x0002C0C4
		public BuildResult RunTarget(IProgressMonitor monitor, string target, ConfigurationSelector configuration)
		{
			return new BuildResult();
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x0002DECB File Offset: 0x0002C0CB
		public void Execute(IProgressMonitor monitor, ExecutionContext context, ConfigurationSelector configuration)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x0002DED2 File Offset: 0x0002C0D2
		public bool CanExecute(ExecutionContext context, ConfigurationSelector configuration)
		{
			return false;
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x0002DED5 File Offset: 0x0002C0D5
		public bool SupportsTarget(string target)
		{
			return false;
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x0002DED8 File Offset: 0x0002C0D8
		public bool NeedsBuilding(ConfigurationSelector configuration)
		{
			return false;
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x0002DEDB File Offset: 0x0002C0DB
		public void SetNeedsBuilding(bool needsBuilding, ConfigurationSelector configuration)
		{
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x0002DEDD File Offset: 0x0002C0DD
		public void Save(IProgressMonitor monitor)
		{
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000C76 RID: 3190 RVA: 0x0002DEDF File Offset: 0x0002C0DF
		// (set) Token: 0x06000C77 RID: 3191 RVA: 0x0002DEE6 File Offset: 0x0002C0E6
		public string Name
		{
			get
			{
				return "Unknown";
			}
			set
			{
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000C78 RID: 3192 RVA: 0x0002DEE8 File Offset: 0x0002C0E8
		public FilePath ItemDirectory
		{
			get
			{
				return FilePath.Empty;
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000C79 RID: 3193 RVA: 0x0002DEEF File Offset: 0x0002C0EF
		// (set) Token: 0x06000C7A RID: 3194 RVA: 0x0002DEF6 File Offset: 0x0002C0F6
		public FilePath BaseDirectory
		{
			get
			{
				return FilePath.Empty;
			}
			set
			{
			}
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x0002DEF8 File Offset: 0x0002C0F8
		public void Dispose()
		{
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000C7C RID: 3196 RVA: 0x0002DEFA File Offset: 0x0002C0FA
		public IDictionary ExtendedProperties
		{
			get
			{
				return null;
			}
		}

		// Token: 0x040003BA RID: 954
		public static UnknownItem Instance = new UnknownItem();
	}
}
