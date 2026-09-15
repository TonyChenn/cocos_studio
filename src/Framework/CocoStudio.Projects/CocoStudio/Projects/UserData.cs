using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	[DataItem("UserData")]
	public class UserData : IExtendedDataItem
	{
		public string FilePath { get; private set; }

		[ItemProperty("Version")]
		public string Version { get; set; }

		[ItemProperty("OpenedDocuments")]
		[Obsolete]
		public List<FilePathData> OpenedDocuments { get; set; }

		[Obsolete]
		[ItemProperty("ActiveDocument")]
		public FilePathData ActiveDocument { get; set; }

		[ItemProperty("Properties")]
		public Dictionary<string, IUserData> Properties { get; set; }

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

		public UserData()
		{
			this.Version = string.Empty;
			this.FilePath = string.Empty;
			this.Properties = new Dictionary<string, IUserData>();
		}

		public UserData(string filePath) : this()
		{
			this.FilePath = filePath;
		}

		public static string GetFilePath(FilePath path)
		{
			return path.ChangeExtension(".udf");
		}

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

		public void ChangeFilePath(string newFilePath)
		{
			this.FilePath = newFilePath;
		}

		public const string FileSuffix = ".udf";

		public const string GuidesFileSuffix = ".csd.udf";

		private Hashtable extendedProperties;
	}
}
