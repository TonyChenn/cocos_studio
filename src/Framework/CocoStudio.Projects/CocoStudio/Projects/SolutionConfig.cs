using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using CocoStudio.Basic;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	[DataItem("SolutionConfig")]
	public class SolutionConfig : IExtendedDataItem
	{
		public string FilePath { get; private set; }

		[ItemProperty("Version")]
		public string Version { get; set; }

		[ItemProperty("PublishDirectory/Value")]
		public string PublishDirectory { get; set; }

		[ItemProperty("PackageDirectory/Value")]
		public string PackageDirectory { get; set; }

		[ItemProperty("PublishType/Value")]
		public PublishType PublishType { get; set; }

		[ItemProperty("SolutionSize/Value")]
		public string SolutionSize { get; set; }

		[ItemProperty("ResolutionName/Value")]
		public string ResolutionName { get; set; }

		[ItemProperty("DefaultSerializer/Value")]
		public string DefaultSerializer { get; set; }

		[ItemProperty("CustomSerializer/Value")]
		public string CustomSerializer { get; set; }

		[ItemProperty("IsNameStandardized/Value")]
		public bool IsNameStandardized { get; set; }

		[ItemProperty("CustomProperties")]
		public Dictionary<string, IUserData> CustomProperties { get; private set; }

		public IDictionary ExtendedProperties
		{
			get
			{
				return this._extendedProperties;
			}
		}

		public SolutionConfig()
		{
			this.Version = string.Empty;
			this.FilePath = string.Empty;
			this.InitDefaultValue();
		}

		public SolutionConfig(string filePath) : this()
		{
			this.FilePath = filePath;
		}

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

		public static string GetFilePath(WorkspaceItem sln)
		{
			return sln.FileName.ChangeExtension(".cfg");
		}

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

		public const string FileSuffix = ".cfg";

		private Hashtable _extendedProperties = new Hashtable();
	}
}
