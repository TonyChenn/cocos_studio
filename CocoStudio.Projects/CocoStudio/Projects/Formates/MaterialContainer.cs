using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CocoStudio.Projects.Formates
{
	// Token: 0x02000014 RID: 20
	internal class MaterialContainer
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00002D25 File Offset: 0x00000F25
		// (set) Token: 0x06000060 RID: 96 RVA: 0x00002D2D File Offset: 0x00000F2D
		public ConcurrentDictionary<string, MaterialItem> MaterialList { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00002D36 File Offset: 0x00000F36
		// (set) Token: 0x06000062 RID: 98 RVA: 0x00002D3E File Offset: 0x00000F3E
		public ParallelLoopResult ParallelScanResult { get; private set; }

		// Token: 0x06000063 RID: 99 RVA: 0x00002D48 File Offset: 0x00000F48
		public MaterialContainer()
		{
			this.MaterialList = new ConcurrentDictionary<string, MaterialItem>();
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00002DA5 File Offset: 0x00000FA5
		public MaterialItem FindResource(string name)
		{
			return this.MaterialList[name];
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00002DC4 File Offset: 0x00000FC4
		private void AddItem(string name, MaterialItem item)
		{
			this.MaterialList.AddOrUpdate(name, item, (string k, MaterialItem v) => item);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002E00 File Offset: 0x00001000
		public void ScanResourceFolder(string path)
		{
			if (!string.IsNullOrEmpty(path))
			{
				this._textureFolder = Path.Combine(path, this._textureFieldFolder);
				string path2 = Path.Combine(path, this._materialFolderName);
				try
				{
					string[] files = Directory.GetFiles(path2, "*.material");
					this.ParallelScanResult = Parallel.ForEach<string>(files, new ParallelOptions
					{
						MaxDegreeOfParallelism = Environment.ProcessorCount + 2
					}, new Action<string>(this.LoadMaterialFile));
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002E8C File Offset: 0x0000108C
		private void LoadMaterialFile(string filename)
		{
			string text = File.ReadAllText(filename);
			if (text.Length > 0)
			{
				Regex regex = new Regex("(?<=\\bmaterial)[\\S\\s]*?(\\n\\})", RegexOptions.ExplicitCapture);
				MatchCollection matchCollection = regex.Matches(text);
				foreach (object obj in matchCollection)
				{
					Match match = (Match)obj;
					string value = match.Value;
					string[] array = value.Split(new char[]
					{
						'\n'
					});
					if (array.Length > 0)
					{
						List<string> list = new List<string>();
						string name = array[0].Trim();
						Regex regex2 = new Regex("(\\btexture\\s.*)", RegexOptions.ExplicitCapture);
						MatchCollection matchCollection2 = regex2.Matches(value);
						foreach (object obj2 in matchCollection2)
						{
							Match match2 = (Match)obj2;
							string text2 = this.CleanupTextureFilename(match2.Value.Replace("texture", "").Trim());
							if (text2.Length > 0)
							{
								list.Add(this._textureFolder + text2);
							}
						}
						MaterialItem item = new MaterialItem(filename, list);
						this.AddItem(name, item);
					}
				}
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003014 File Offset: 0x00001214
		public string CleanupTextureFilename(string filename)
		{
			string text = filename;
			int i = text.LastIndexOf(" ", StringComparison.Ordinal);
			if (i > 0)
			{
				int num = text.LastIndexOf(".", StringComparison.Ordinal);
				if (num > 0)
				{
					while (i > num)
					{
						text = text.Substring(0, i).Trim();
						i = text.LastIndexOf(" ", StringComparison.Ordinal);
					}
				}
			}
			return text;
		}

		// Token: 0x04000012 RID: 18
		public const string MaterialListPattern = "*.material";

		// Token: 0x04000013 RID: 19
		private const string RegexPatternMaterialpart = "(?<=\\bmaterial)[\\S\\s]*?(\\n\\})";

		// Token: 0x04000014 RID: 20
		private const string RegexPatternTexturepart = "(\\btexture\\s.*)";

		// Token: 0x04000015 RID: 21
		private const string TextureFieldName = "texture";

		// Token: 0x04000016 RID: 22
		private readonly string _materialFolderName = "materials" + Path.DirectorySeparatorChar;

		// Token: 0x04000017 RID: 23
		private readonly string _textureFieldFolder = "textures" + Path.DirectorySeparatorChar;

		// Token: 0x04000018 RID: 24
		private string _textureFolder = "";
	}
}
