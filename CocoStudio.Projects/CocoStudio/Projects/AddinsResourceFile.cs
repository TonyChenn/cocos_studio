using System;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Model;
using MonoDevelop.Core;

namespace CocoStudio.Projects
{
	// Token: 0x02000040 RID: 64
	internal class AddinsResourceFile : ResourceFile
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060001BB RID: 443 RVA: 0x00007006 File Offset: 0x00005206
		internal override string PreviewImagePath
		{
			get
			{
				return this.FileName;
			}
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00007013 File Offset: 0x00005213
		protected AddinsResourceFile() : base(true)
		{
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000701C File Offset: 0x0000521C
		public AddinsResourceFile(ResourceData resourceData) : this()
		{
			this.resourceData = resourceData;
			if (Path.IsPathRooted(resourceData.Path))
			{
				this.FileName = resourceData.Path;
				return;
			}
			this.FileName = Path.Combine(Option.LuaScriptFolder, resourceData.Path);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00007070 File Offset: 0x00005270
		protected override ResourceData CreateDefaultResourceData(FilePath filePath)
		{
			return this.resourceData;
		}

		// Token: 0x04000075 RID: 117
		private ResourceData resourceData;
	}
}
