using System;
using CocoStudio.Model;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Projects
{
	// Token: 0x0200005A RID: 90
	public class PlistImageFile : ResourceFile
	{
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000279 RID: 633 RVA: 0x00009840 File Offset: 0x00007A40
		// (set) Token: 0x0600027A RID: 634 RVA: 0x00009848 File Offset: 0x00007A48
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

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600027B RID: 635 RVA: 0x0000986F File Offset: 0x00007A6F
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

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600027C RID: 636 RVA: 0x0000988F File Offset: 0x00007A8F
		// (set) Token: 0x0600027D RID: 637 RVA: 0x00009897 File Offset: 0x00007A97
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

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600027E RID: 638 RVA: 0x000098A3 File Offset: 0x00007AA3
		internal override string PreviewImagePath
		{
			get
			{
				return this.FileName;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600027F RID: 639 RVA: 0x000098B0 File Offset: 0x00007AB0
		public override string FullPath
		{
			get
			{
				return this.Parent.FullPath;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000280 RID: 640 RVA: 0x000098BD File Offset: 0x00007ABD
		// (set) Token: 0x06000281 RID: 641 RVA: 0x000098C5 File Offset: 0x00007AC5
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

		// Token: 0x06000282 RID: 642 RVA: 0x000098CE File Offset: 0x00007ACE
		private PlistImageFile()
		{
		}

		// Token: 0x06000283 RID: 643 RVA: 0x000098D6 File Offset: 0x00007AD6
		public PlistImageFile(FilePath info, string subImageName, PlistImageFolder plistFileFolder)
		{
			this.FileName = info;
			this.Parent = plistFileFolder;
			this.key = subImageName;
		}

		// Token: 0x06000284 RID: 644 RVA: 0x000098F4 File Offset: 0x00007AF4
		public override ResourceData GetResourceData()
		{
			ResourceData resourceData = this.Parent.GetResourceData();
			return new ResourceData(EnumResourceType.PlistSubImage, this.key, resourceData.Path);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00009920 File Offset: 0x00007B20
		protected override void OnMove(FilePath newMovePath)
		{
			int startIndex = newMovePath.ToString().LastIndexOf(this.Name);
			newMovePath = ((FilePath)newMovePath.ToString().Remove(startIndex)).Combine(new string[]
			{
				this.FileName.FileName
			});
			base.OnMove(newMovePath);
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00009988 File Offset: 0x00007B88
		protected override void OnSetLocation(FilePath newFilePath, bool isRename = true)
		{
			int startIndex = newFilePath.ToString().LastIndexOf(this.Name);
			newFilePath = ((FilePath)newFilePath.ToString().Remove(startIndex)).Combine(new string[]
			{
				this.FileName.FileName
			});
			base.OnSetLocation(newFilePath, isRename);
		}

		// Token: 0x0400009E RID: 158
		[JsonProperty(PropertyName = "Key")]
		[ItemProperty(Name = "Key")]
		private string key;
	}
}
