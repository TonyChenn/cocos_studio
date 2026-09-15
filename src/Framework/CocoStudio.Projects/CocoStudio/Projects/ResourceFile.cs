using System;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Model;
using CocoStudio.Projects.Formates;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Ide;

namespace CocoStudio.Projects
{
	[DataInclude(typeof(MeshFile))]
	[DataInclude(typeof(PuFile))]
	[DataInclude(typeof(LuaFile))]
	[DataInclude(typeof(ImageFile))]
	[DataInclude(typeof(AudioFile))]
	[DataInclude(typeof(CocosItem))]
	[DataInclude(typeof(PlistImageFile))]
	[DataInclude(typeof(PlistParticleFile))]
	[DataInclude(typeof(TmxFile))]
	[DataInclude(typeof(FntFile))]
	[DataInclude(typeof(TTFFile))]
	[DataItem(Name = "File")]
	[DataInclude(typeof(Sprite3DFile))]
	[DataInclude(typeof(AddinsResourceFile))]
	public class ResourceFile : ResourceItem, IFileItem
	{
		[ResourcePathItemProperty("Name")]
		public virtual FilePath FileName
		{
			get
			{
				return this.fileName;
			}
			internal set
			{
				this.fileName = value;
			}
		}

		public override string Name
		{
			get
			{
				if (!string.IsNullOrEmpty(this.name))
				{
					return this.name;
				}
				return this.FileName.FileName;
			}
			protected set
			{
				this.FileName = this.FileName.ParentDirectory.Combine(new string[]
				{
					value
				});
				this.name = value;
			}
		}

		public bool IsDefault { get; private set; }

		protected ResourceFile()
		{
		}

		protected ResourceFile(bool isDefault)
		{
			this.IsDefault = isDefault;
		}

		public ResourceFile(FilePath fileName)
		{
			if (!fileName.IsAbsolute)
			{
				throw new ArgumentException("Must be absolute path.");
			}
			this.FileName = fileName;
		}

		public ResourceFile(ResourceData resourceData)
		{
			if (resourceData.Type == EnumResourceType.Default)
			{
				this.IsDefault = true;
			}
			this.FileName = ProjectsService.Instance.GetFullPath(resourceData);
		}

		protected virtual ResourceData CreateDefaultResourceData(FilePath filePath)
		{
			string text = filePath.ToRelative(Option.EditorDefaultResourcePath);
			text = Option.ConvertToMacPath(text);
			return new ResourceData(EnumResourceType.Default, text);
		}

		public override ResourceData GetResourceData()
		{
			if (this.IsDefault)
			{
				return this.CreateDefaultResourceData(this.FileName);
			}
			if (string.IsNullOrEmpty(this.FileName))
			{
				return ResourceData.Empty;
			}
			return this.CreateResourceData(this.FileName);
		}

		public override string FullPath
		{
			get
			{
				return this.FileName.FullPath;
			}
		}

		protected override void OnSetLocation(FilePath newFilePath, bool isRename = true)
		{
			if (this.IsDefault)
			{
				throw new InvalidOperationException("Default resource can not be moved.");
			}
			this.FileName = newFilePath;
			base.OnSetLocation(newFilePath, isRename);
		}

		protected override void OnMove(FilePath newMovePath)
		{
			if (this.IsDefault)
			{
				throw new InvalidOperationException("Default resource can not be moved.");
			}
			try
			{
				if (!Directory.Exists(newMovePath.ParentDirectory))
				{
					Directory.CreateDirectory(newMovePath.ParentDirectory);
				}
				if (File.Exists(this.fileName))
				{
					FileService.MoveFile(this.FileName, newMovePath);
				}
				this.FileName = newMovePath;
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("File move failure", exception);
			}
			base.OnMove(newMovePath);
		}

		protected override DataError OnCheckDataError()
		{
			if (this.IsDefault)
			{
				return null;
			}
			if (!File.Exists(this.FileName))
			{
				string message = string.Format(LanguageInfo.DataError7_NotExist, this.FileName.FileName);
				return new DataError(message);
			}
			return null;
		}

		protected override void OnDelete(IProgressMonitor monitor)
		{
			if (this.IsDefault)
			{
				throw new InvalidOperationException("Default resource can not be deleted.");
			}
			try
			{
				DesktopService.PlatformService.DeleteToTrash(this.fileName);
				if (File.Exists(this.fileName))
				{
					throw new IOException("Failed to delete file.");
				}
				base.OnDelete(monitor);
			}
			catch (IOException exception)
			{
				monitor.ReportError(LanguageInfo.MessageBox259_ProcessCannotAccess, exception);
				LogConfig.Output.Error(LanguageInfo.MessageBox259_ProcessCannotAccess, exception);
			}
			catch (Exception ex)
			{
				monitor.ReportError(ex.ToString(), ex);
				LogConfig.Logger.Error(ex.ToString());
			}
		}

		protected override void OnRefresh()
		{
			if (this.IsDefault)
			{
				return;
			}
			base.OnRefresh();
		}

		protected internal override bool IsNeedRefresh()
		{
			return !this.IsDefault && base.IsNeedRefresh();
		}

		protected virtual ICompositeResourceProcesser GetCompositeResourceProcesser()
		{
			return null;
		}

		public static ResourceFile PreprocessToEngine(ref ResourceFile file, ResourceFile defaultFile, bool supportEmptyResource = false)
		{
			ResourceFile result;
			if (file == null)
			{
				if (supportEmptyResource)
				{
					result = ResourceFile.Empty;
				}
				else
				{
					file = defaultFile;
					result = defaultFile;
				}
			}
			else if (file.DataError != null)
			{
				result = defaultFile;
			}
			else if (file == ResourceFile.DefaultMarker)
			{
				file = defaultFile;
				result = defaultFile;
			}
			else
			{
				result = file;
			}
			return result;
		}

		public static readonly ResourceFile DefaultMarker = new ResourceFile(true);

		public static readonly ResourceFile Empty = new ResourceFile();

		protected FilePath fileName;

		protected string name;
	}
}
