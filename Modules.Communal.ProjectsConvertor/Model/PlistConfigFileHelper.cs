using System;
using System.Collections.Generic;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Core;
using Modules.Communal.Packer;
using MonoDevelop.Core;

namespace Modules.Communal.ProjectsConvertor.Model
{
	// Token: 0x02000040 RID: 64
	public class PlistConfigFileHelper
	{
		// Token: 0x060003F6 RID: 1014 RVA: 0x0000A465 File Offset: 0x00008665
		public void Clear()
		{
			this.configfileList.Clear();
			this.resPlistfileMap.Clear();
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000A480 File Offset: 0x00008680
		public void AddPlistConfigFile(string plistfileabspath)
		{
			if (!File.Exists(plistfileabspath))
			{
				return;
			}
			PListImageReader plistImageReader = new PListImageReader(plistfileabspath);
			string text = FileService.AbsoluteToRelativePath(Services.ProjectOperations.CurrentResourceGroup.RootFolder.FullPath, plistfileabspath);
			text = Option.ConvertToMacPath(text);
			foreach (ImageInfo imageInfo in plistImageReader.ImageList)
			{
				string name = imageInfo.Name;
				if (this.resPlistfileMap.ContainsKey(name))
				{
					this.resPlistfileMap[name] = text;
				}
				else
				{
					this.resPlistfileMap.Add(name, text);
				}
			}
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000A538 File Offset: 0x00008738
		public string FindPlistFile(string imgNameResKey)
		{
			if (!this.resPlistfileMap.ContainsKey(imgNameResKey))
			{
				return "";
			}
			return this.resPlistfileMap[imgNameResKey];
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000A55A File Offset: 0x0000875A
		public bool isEmpty()
		{
			return this.resPlistfileMap.Count == 0;
		}

		// Token: 0x040001D0 RID: 464
		private List<string> configfileList = new List<string>();

		// Token: 0x040001D1 RID: 465
		private Dictionary<string, string> resPlistfileMap = new Dictionary<string, string>();
	}
}
