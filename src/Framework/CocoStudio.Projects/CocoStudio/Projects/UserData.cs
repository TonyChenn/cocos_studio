using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	// Token: 0x0200007D RID: 125
	[DataItem("UserData")]
	public class UserData : IExtendedDataItem
	{
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060003AF RID: 943 RVA: 0x0000CF96 File Offset: 0x0000B196
		// (set) Token: 0x060003B0 RID: 944 RVA: 0x0000CF9E File Offset: 0x0000B19E
		public string FilePath { get; private set; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x0000CFA7 File Offset: 0x0000B1A7
		// (set) Token: 0x060003B2 RID: 946 RVA: 0x0000CFAF File Offset: 0x0000B1AF
		[ItemProperty("Version")]
		public string Version { get; set; }

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x0000CFB8 File Offset: 0x0000B1B8
		// (set) Token: 0x060003B4 RID: 948 RVA: 0x0000CFC0 File Offset: 0x0000B1C0
		[ItemProperty("OpenedDocuments")]
		[Obsolete]
		public List<FilePathData> OpenedDocuments { get; set; }

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x0000CFC9 File Offset: 0x0000B1C9
		// (set) Token: 0x060003B6 RID: 950 RVA: 0x0000CFD1 File Offset: 0x0000B1D1
		[Obsolete]
		[ItemProperty("ActiveDocument")]
		public FilePathData ActiveDocument { get; set; }

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x0000CFDA File Offset: 0x0000B1DA
		// (set) Token: 0x060003B8 RID: 952 RVA: 0x0000CFE2 File Offset: 0x0000B1E2
		[ItemProperty("Properties")]
		public Dictionary<string, IUserData> Properties { get; set; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x0000CFEB File Offset: 0x0000B1EB
		public IDictionary ExtendedProperties
		{
			get
			{
				if (this.extendedProperties == null)
				{
					this.extendedProperties = new Hashtable();
				}
				return this.extendedProperties;
			}
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000D006 File Offset: 0x0000B206
		public UserData()
		{
			this.Version = string.Empty;
			this.FilePath = string.Empty;
			this.Properties = new Dictionary<string, IUserData>();
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000D02F File Offset: 0x0000B22F
		public UserData(string filePath) : this()
		{
			this.FilePath = filePath;
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000D03E File Offset: 0x0000B23E
		public static string GetFilePath(FilePath path)
		{
			return path.ChangeExtension(".udf");
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000D054 File Offset: 0x0000B254
		public static UserData Load(string filePath)
		{
			if (!File.Exists(filePath))
			{
				return null;
			}
			UserData userData = null;
			try
			{
				IProgressMonitor defaultMonitor = ProjectsService.Instance.DefaultMonitor;
				userData = (ProjectsService.Instance.ReadFile(defaultMonitor, filePath, typeof(UserData)) as UserData);
			}
			catch (Exception ex)
			{
				userData = null;
				LoggingService.LogError("Exception while loading user solution userdatas." + filePath, ex);
			}
			if (userData != null)
			{
				userData.FilePath = filePath;
			}
			return userData;
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000D0C8 File Offset: 0x0000B2C8
		public void Save()
		{
			if (string.IsNullOrEmpty(this.FilePath))
			{
				return;
			}
			this.Version = "2.3.3.0";
			try
			{
				IProgressMonitor defaultMonitor = ProjectsService.Instance.DefaultMonitor;
				ProjectsService.Instance.WriteFile(defaultMonitor, this.FilePath, this);
			}
			catch (Exception ex)
			{
				LoggingService.LogWarning("Could not save solution userdatas: " + this.FilePath, ex);
			}
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000D13C File Offset: 0x0000B33C
		public void ChangeFilePath(string newFilePath)
		{
			this.FilePath = newFilePath;
		}

		// Token: 0x040000F1 RID: 241
		public const string FileSuffix = ".udf";

		// Token: 0x040000F2 RID: 242
		public const string GuidesFileSuffix = ".csd.udf";

		// Token: 0x040000F3 RID: 243
		private Hashtable extendedProperties;
	}
}
