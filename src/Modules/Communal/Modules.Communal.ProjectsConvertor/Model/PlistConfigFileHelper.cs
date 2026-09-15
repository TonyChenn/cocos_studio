using System;
using System.Collections.Generic;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Core;
using Modules.Communal.Packer;
using MonoDevelop.Core;

namespace Modules.Communal.ProjectsConvertor.Model
{
	public class PlistConfigFileHelper
	{
		public void Clear()
		{
			this.configfileList.Clear();
			this.resPlistfileMap.Clear();
		}

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

		public string FindPlistFile(string imgNameResKey)
		{
			if (!this.resPlistfileMap.ContainsKey(imgNameResKey))
			{
				return "";
			}
			return this.resPlistfileMap[imgNameResKey];
		}

		public bool isEmpty()
		{
			return this.resPlistfileMap.Count == 0;
		}

		private List<string> configfileList = new List<string>();

		private Dictionary<string, string> resPlistfileMap = new Dictionary<string, string>();
	}
}
