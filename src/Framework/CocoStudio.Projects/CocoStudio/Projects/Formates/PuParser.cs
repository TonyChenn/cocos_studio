using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace CocoStudio.Projects.Formates
{
	internal class PuParser
	{
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

		public List<string> GetMaterialFiles()
		{
			List<string> result = null;
			if (this._allRelatedFileList != null)
			{
				result = this._allRelatedFileList["material"];
			}
			return result;
		}

		public List<string> GetTextureFiles()
		{
			List<string> result = null;
			if (this._allRelatedFileList != null)
			{
				result = this._allRelatedFileList["texture"];
			}
			return result;
		}

		public List<string> GetAllMeshFiles()
		{
			List<string> result = null;
			if (this._allRelatedFileList != null)
			{
				result = this._allRelatedFileList["model"];
			}
			return result;
		}

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

		private const string MaterialFieldKey = "material";

		private const string RegexPatternMaterial = "(material.*)";

		private const string MeshFieldKey = "mesh_name";

		private const string RegexPatternMesh = "(mesh_name.*)";

		private const string ListFieldMaterial = "material";

		private const string ListFieldTexture = "texture";

		private const string ListField3Dmodel = "model";

		private readonly string _meshFolderName = "models" + Path.DirectorySeparatorChar;

		private string _resourcePath = "";

		private Dictionary<string, List<string>> _allRelatedFileList;
	}
}
