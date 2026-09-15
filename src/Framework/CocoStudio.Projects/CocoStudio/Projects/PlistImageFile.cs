using System;
using CocoStudio.Model;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Projects
{
	public class PlistImageFile : ResourceFile
	{
		public override ResourceItem Parent
		{
			get
			{
				return base.Parent;
			}
			internal set
			{
				if (value == null)
				{
					base.Parent = value;
					return;
				}
				if (!(value is PlistImageFolder))
				{
					throw new ArgumentException("PlistImageFile的父类必须是PlistImageFolder类型");
				}
				base.Parent = value;
			}
		}

		public override DataError DataError
		{
			get
			{
				if (this.Parent != null)
				{
					return this.Parent.DataError;
				}
				return new DataError(LanguageInfo.DataError4_ParentNotExist);
			}
		}

		public override string Name
		{
			get
			{
				return this.key;
			}
			protected set
			{
				throw new InvalidOperationException("Can not set name of plistImageFile.");
			}
		}

		internal override string PreviewImagePath
		{
			get
			{
				return this.FileName;
			}
		}

		public override string FullPath
		{
			get
			{
				return this.Parent.FullPath;
			}
		}

		public override FilePath FileName
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

		private PlistImageFile()
		{
		}

		public PlistImageFile(FilePath info, string subImageName, PlistImageFolder plistFileFolder)
		{
			this.FileName = info;
			this.Parent = plistFileFolder;
			this.key = subImageName;
		}

		public override ResourceData GetResourceData()
		{
			ResourceData resourceData = this.Parent.GetResourceData();
			return new ResourceData(EnumResourceType.PlistSubImage, this.key, resourceData.Path);
		}

		protected override void OnMove(FilePath newMovePath)
		{
			int startIndex = newMovePath.ToString().LastIndexOf(this.Name);
			newMovePath = ((FilePath)newMovePath.ToString().Remove(startIndex)).Combine(new string[]
			{
				this.FileName.FileName
			});
			base.OnMove(newMovePath);
		}

		protected override void OnSetLocation(FilePath newFilePath, bool isRename = true)
		{
			int startIndex = newFilePath.ToString().LastIndexOf(this.Name);
			newFilePath = ((FilePath)newFilePath.ToString().Remove(startIndex)).Combine(new string[]
			{
				this.FileName.FileName
			});
			base.OnSetLocation(newFilePath, isRename);
		}

		[JsonProperty(PropertyName = "Key")]
		[ItemProperty(Name = "Key")]
		private string key;
	}
}
