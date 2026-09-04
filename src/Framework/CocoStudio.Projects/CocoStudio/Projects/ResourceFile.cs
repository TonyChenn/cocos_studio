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
	// Token: 0x0200003F RID: 63
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
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x00006C35 File Offset: 0x00004E35
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x00006C3D File Offset: 0x00004E3D
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

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x00006C48 File Offset: 0x00004E48
		// (set) Token: 0x060001A8 RID: 424 RVA: 0x00006C78 File Offset: 0x00004E78
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

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x00006CB4 File Offset: 0x00004EB4
		// (set) Token: 0x060001AA RID: 426 RVA: 0x00006CBC File Offset: 0x00004EBC
		public bool IsDefault { get; private set; }

		// Token: 0x060001AC RID: 428 RVA: 0x00006CDC File Offset: 0x00004EDC
		protected ResourceFile()
		{
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00006CE4 File Offset: 0x00004EE4
		protected ResourceFile(bool isDefault)
		{
			this.IsDefault = isDefault;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00006CF3 File Offset: 0x00004EF3
		public ResourceFile(FilePath fileName)
		{
			if (!fileName.IsAbsolute)
			{
				throw new ArgumentException("Must be absolute path.");
			}
			this.FileName = fileName;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00006D16 File Offset: 0x00004F16
		public ResourceFile(ResourceData resourceData)
		{
			if (resourceData.Type == EnumResourceType.Default)
			{
				this.IsDefault = true;
			}
			this.FileName = ProjectsService.Instance.GetFullPath(resourceData);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00006D40 File Offset: 0x00004F40
		protected virtual ResourceData CreateDefaultResourceData(FilePath filePath)
		{
			string text = filePath.ToRelative(Option.EditorDefaultResourcePath);
			text = Option.ConvertToMacPath(text);
			return new ResourceData(EnumResourceType.Default, text);
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00006D72 File Offset: 0x00004F72
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

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x00006DB0 File Offset: 0x00004FB0
		public override string FullPath
		{
			get
			{
				return this.FileName.FullPath;
			}
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00006DD0 File Offset: 0x00004FD0
		protected override void OnSetLocation(FilePath newFilePath, bool isRename = true)
		{
			if (this.IsDefault)
			{
				throw new InvalidOperationException("Default resource can not be moved.");
			}
			this.FileName = newFilePath;
			base.OnSetLocation(newFilePath, isRename);
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00006DF4 File Offset: 0x00004FF4
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

		// Token: 0x060001B5 RID: 437 RVA: 0x00006E94 File Offset: 0x00005094
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

		// Token: 0x060001B6 RID: 438 RVA: 0x00006EE0 File Offset: 0x000050E0
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

		// Token: 0x060001B7 RID: 439 RVA: 0x00006F98 File Offset: 0x00005198
		protected override void OnRefresh()
		{
			if (this.IsDefault)
			{
				return;
			}
			base.OnRefresh();
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00006FA9 File Offset: 0x000051A9
		protected internal override bool IsNeedRefresh()
		{
			return !this.IsDefault && base.IsNeedRefresh();
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00006FBB File Offset: 0x000051BB
		protected virtual ICompositeResourceProcesser GetCompositeResourceProcesser()
		{
			return null;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00006FC0 File Offset: 0x000051C0
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

		// Token: 0x04000070 RID: 112
		public static readonly ResourceFile DefaultMarker = new ResourceFile(true);

		// Token: 0x04000071 RID: 113
		public static readonly ResourceFile Empty = new ResourceFile();

		// Token: 0x04000072 RID: 114
		protected FilePath fileName;

		// Token: 0x04000073 RID: 115
		protected string name;
	}
}
