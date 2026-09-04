using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using CocoStudio.Basic;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	// Token: 0x0200007F RID: 127
	[DataItem("SolutionConfig")]
	public class SolutionConfig : IExtendedDataItem
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060003C5 RID: 965 RVA: 0x0000D16F File Offset: 0x0000B36F
		// (set) Token: 0x060003C6 RID: 966 RVA: 0x0000D177 File Offset: 0x0000B377
		public string FilePath { get; private set; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060003C7 RID: 967 RVA: 0x0000D180 File Offset: 0x0000B380
		// (set) Token: 0x060003C8 RID: 968 RVA: 0x0000D188 File Offset: 0x0000B388
		[ItemProperty("Version")]
		public string Version { get; set; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x0000D191 File Offset: 0x0000B391
		// (set) Token: 0x060003CA RID: 970 RVA: 0x0000D199 File Offset: 0x0000B399
		[ItemProperty("PublishDirectory/Value")]
		public string PublishDirectory { get; set; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060003CB RID: 971 RVA: 0x0000D1A2 File Offset: 0x0000B3A2
		// (set) Token: 0x060003CC RID: 972 RVA: 0x0000D1AA File Offset: 0x0000B3AA
		[ItemProperty("PackageDirectory/Value")]
		public string PackageDirectory { get; set; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060003CD RID: 973 RVA: 0x0000D1B3 File Offset: 0x0000B3B3
		// (set) Token: 0x060003CE RID: 974 RVA: 0x0000D1BB File Offset: 0x0000B3BB
		[ItemProperty("PublishType/Value")]
		public PublishType PublishType { get; set; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060003CF RID: 975 RVA: 0x0000D1C4 File Offset: 0x0000B3C4
		// (set) Token: 0x060003D0 RID: 976 RVA: 0x0000D1CC File Offset: 0x0000B3CC
		[ItemProperty("SolutionSize/Value")]
		public string SolutionSize { get; set; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060003D1 RID: 977 RVA: 0x0000D1D5 File Offset: 0x0000B3D5
		// (set) Token: 0x060003D2 RID: 978 RVA: 0x0000D1DD File Offset: 0x0000B3DD
		[ItemProperty("ResolutionName/Value")]
		public string ResolutionName { get; set; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060003D3 RID: 979 RVA: 0x0000D1E6 File Offset: 0x0000B3E6
		// (set) Token: 0x060003D4 RID: 980 RVA: 0x0000D1EE File Offset: 0x0000B3EE
		[ItemProperty("DefaultSerializer/Value")]
		public string DefaultSerializer { get; set; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x0000D1F7 File Offset: 0x0000B3F7
		// (set) Token: 0x060003D6 RID: 982 RVA: 0x0000D1FF File Offset: 0x0000B3FF
		[ItemProperty("CustomSerializer/Value")]
		public string CustomSerializer { get; set; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x0000D208 File Offset: 0x0000B408
		// (set) Token: 0x060003D8 RID: 984 RVA: 0x0000D210 File Offset: 0x0000B410
		[ItemProperty("IsNameStandardized/Value")]
		public bool IsNameStandardized { get; set; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060003D9 RID: 985 RVA: 0x0000D219 File Offset: 0x0000B419
		// (set) Token: 0x060003DA RID: 986 RVA: 0x0000D221 File Offset: 0x0000B421
		[ItemProperty("CustomProperties")]
		public Dictionary<string, IUserData> CustomProperties { get; private set; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060003DB RID: 987 RVA: 0x0000D22A File Offset: 0x0000B42A
		public IDictionary ExtendedProperties
		{
			get
			{
				return this._extendedProperties;
			}
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0000D232 File Offset: 0x0000B432
		public SolutionConfig()
		{
			this.Version = string.Empty;
			this.FilePath = string.Empty;
			this.InitDefaultValue();
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0000D261 File Offset: 0x0000B461
		public SolutionConfig(string filePath) : this()
		{
			this.FilePath = filePath;
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0000D270 File Offset: 0x0000B470
		private void InitDefaultValue()
		{
			this.CustomProperties = new Dictionary<string, IUserData>();
			this.PublishDirectory = Solution.DefaultPublishDirectoryName;
			this.PackageDirectory = Solution.DefaultPackageDirectoryName;
			this.PublishType = PublishType.Reference;
			this.SolutionSize = "960 * 640";
			this.ResolutionName = "iPhone 4/4S";
			this.DefaultSerializer = (this.CustomSerializer = "Serializer_FlatBuffers");
			this.IsNameStandardized = false;
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0000D2D8 File Offset: 0x0000B4D8
		public static string GetFilePath(WorkspaceItem sln)
		{
			return sln.FileName.ChangeExtension(".cfg");
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000D300 File Offset: 0x0000B500
		internal static SolutionConfig Load(string filePath)
		{
			if (!File.Exists(filePath))
			{
				return null;
			}
			SolutionConfig solutionConfig = null;
			try
			{
				IProgressMonitor defaultMonitor = ProjectsService.Instance.DefaultMonitor;
				solutionConfig = (ProjectsService.Instance.ReadFile(defaultMonitor, filePath, typeof(SolutionConfig)) as SolutionConfig);
			}
			catch (Exception ex)
			{
				solutionConfig = null;
				LoggingService.LogError("读取项目配置时出错", ex);
			}
			if (solutionConfig != null)
			{
				solutionConfig.FilePath = filePath;
			}
			return solutionConfig;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000D370 File Offset: 0x0000B570
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
			catch (Exception exception)
			{
				LogConfig.Logger.Error("保存项目用户属性时出错", exception);
			}
		}

		// Token: 0x040000FC RID: 252
		public const string FileSuffix = ".cfg";

		// Token: 0x040000FD RID: 253
		private Hashtable _extendedProperties = new Hashtable();
	}
}
