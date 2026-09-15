using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CocoStudio.Projects.Formates
{
	internal class MaterialContainer
	{
		public ConcurrentDictionary<string, MaterialItem> MaterialList { get; set; }

		public ParallelLoopResult ParallelScanResult { get; private set; }

		public MaterialContainer()
		{
			this.MaterialList = new ConcurrentDictionary<string, MaterialItem>();
		}

		public MaterialItem FindResource(string name)
		{
			return this.MaterialList[name];
		}

		private void AddItem(string name, MaterialItem item)
		{
			this.MaterialList.AddOrUpdate(name, item, (string k, MaterialItem v) => item);
		}

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

		public const string MaterialListPattern = "*.material";

		private const string RegexPatternMaterialpart = "(?<=\\bmaterial)[\\S\\s]*?(\\n\\})";

		private const string RegexPatternTexturepart = "(\\btexture\\s.*)";

		private const string TextureFieldName = "texture";

		private readonly string _materialFolderName = "materials" + Path.DirectorySeparatorChar;

		private readonly string _textureFieldFolder = "textures" + Path.DirectorySeparatorChar;

		private string _textureFolder = "";
	}
}
