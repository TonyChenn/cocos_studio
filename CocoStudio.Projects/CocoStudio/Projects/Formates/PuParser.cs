using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace CocoStudio.Projects.Formates
{
	// Token: 0x02000019 RID: 25
	internal class PuParser
	{
		// Token: 0x06000084 RID: 132 RVA: 0x00003388 File Offset: 0x00001588
		public bool LoadPuFileAndGetAllResource(string fullpath)
		{
			bool result = false;
			string text = Path.GetDirectoryName(fullpath);
			text = Path.Combine(text, Path.GetFileName(fullpath));
			string text2 = File.ReadAllText(text);
			if (text2.Length > 0)
			{
				this._resourcePath = PuParser.FindResourcePath(text);
				if (this._resourcePath.Length > 0)
				{
					MaterialManager instance = MaterialManager.GetInstance();
					result = true;
					List<string> resourceNames = PuParser.GetResourceNames(text2, "(material.*)", "material");
					List<string> resourceNames2 = PuParser.GetResourceNames(text2, "(mesh_name.*)", "mesh_name");
					this._allRelatedFileList = new Dictionary<string, List<string>>
					{
						{
							"material",
							new List<string>()
						},
						{
							"texture",
							new List<string>()
						},
						{
							"model",
							new List<string>()
						}
					};
					if (resourceNames != null && resourceNames.Count > 0)
					{
						foreach (string materialName in resourceNames)
						{
							MaterialItem materialResource = instance.GetMaterialResource(this._resourcePath, materialName);
							if (materialResource != null)
							{
								if (materialResource.ContainerFile.Length > 0)
								{
									this.AddContainerFile(materialResource.ContainerFile);
								}
								if (materialResource.ResourceFileList != null && materialResource.ResourceFileList.Count > 0)
								{
									foreach (string filename in materialResource.ResourceFileList)
									{
										this.AddResourcesFile(filename);
									}
								}
							}
						}
					}
					if (resourceNames2 != null && resourceNames2.Count > 0)
					{
						string text3 = Path.GetDirectoryName(text);
						text3 = Path.GetDirectoryName(text3) + Path.DirectorySeparatorChar + this._meshFolderName;
						foreach (string text4 in resourceNames2)
						{
							if (!string.IsNullOrEmpty(text4))
							{
								string item = text3 + text4;
								List<string> list = this._allRelatedFileList["model"];
								if (!list.Contains(item))
								{
									list.Add(item);
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000035DC File Offset: 0x000017DC
		public List<string> GetMaterialFiles()
		{
			List<string> result = null;
			if (this._allRelatedFileList != null)
			{
				result = this._allRelatedFileList["material"];
			}
			return result;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003608 File Offset: 0x00001808
		public List<string> GetTextureFiles()
		{
			List<string> result = null;
			if (this._allRelatedFileList != null)
			{
				result = this._allRelatedFileList["texture"];
			}
			return result;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003634 File Offset: 0x00001834
		public List<string> GetAllMeshFiles()
		{
			List<string> result = null;
			if (this._allRelatedFileList != null)
			{
				result = this._allRelatedFileList["model"];
			}
			return result;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003660 File Offset: 0x00001860
		private void AddContainerFile(string filename)
		{
			if (!string.IsNullOrEmpty(filename))
			{
				List<string> list = this._allRelatedFileList["material"];
				if (!list.Contains(filename))
				{
					list.Add(filename);
				}
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003698 File Offset: 0x00001898
		private void AddResourcesFile(string filename)
		{
			if (!string.IsNullOrEmpty(filename))
			{
				List<string> list = this._allRelatedFileList["texture"];
				if (!list.Contains(filename))
				{
					list.Add(filename);
				}
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000036D0 File Offset: 0x000018D0
		private static string FindResourcePath(string fullpath)
		{
			string result = "";
			try
			{
				result = Path.GetDirectoryName(Path.GetDirectoryName(fullpath));
			}
			catch (Exception)
			{
				result = "";
			}
			return result;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000370C File Offset: 0x0000190C
		private static List<string> GetResourceNames(string puContents, string regexPattern, string fieldName)
		{
			List<string> list = null;
			Regex regex = new Regex(regexPattern, RegexOptions.Multiline | RegexOptions.ExplicitCapture);
			MatchCollection matchCollection = regex.Matches(puContents);
			if (matchCollection.Count > 0)
			{
				list = new List<string>();
				for (int i = 0; i < matchCollection.Count; i++)
				{
					Match match = matchCollection[i];
					string item = match.Value.Replace(fieldName, "").Trim();
					list.Add(item);
				}
			}
			return list;
		}

		// Token: 0x04000022 RID: 34
		private const string MaterialFieldKey = "material";

		// Token: 0x04000023 RID: 35
		private const string RegexPatternMaterial = "(material.*)";

		// Token: 0x04000024 RID: 36
		private const string MeshFieldKey = "mesh_name";

		// Token: 0x04000025 RID: 37
		private const string RegexPatternMesh = "(mesh_name.*)";

		// Token: 0x04000026 RID: 38
		private const string ListFieldMaterial = "material";

		// Token: 0x04000027 RID: 39
		private const string ListFieldTexture = "texture";

		// Token: 0x04000028 RID: 40
		private const string ListField3Dmodel = "model";

		// Token: 0x04000029 RID: 41
		private readonly string _meshFolderName = "models" + Path.DirectorySeparatorChar;

		// Token: 0x0400002A RID: 42
		private string _resourcePath = "";

		// Token: 0x0400002B RID: 43
		private Dictionary<string, List<string>> _allRelatedFileList;
	}
}
