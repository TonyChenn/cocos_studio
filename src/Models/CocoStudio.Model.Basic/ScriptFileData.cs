using System;
using System.IO;
using CocoStudio.Basic;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Model
{
	// Token: 0x02000018 RID: 24
	public class ScriptFileData
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000AB RID: 171 RVA: 0x000034BC File Offset: 0x000016BC
		// (set) Token: 0x060000AC RID: 172 RVA: 0x000034D3 File Offset: 0x000016D3
		[ItemProperty(DefaultValue = ScriptType.None)]
		public ScriptType FileType { get; set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000AD RID: 173 RVA: 0x000034DC File Offset: 0x000016DC
		// (set) Token: 0x060000AE RID: 174 RVA: 0x000034F4 File Offset: 0x000016F4
		[ItemProperty(DefaultValue = "")]
		public string RelativeScriptFile
		{
			get
			{
				return this._relativeFile;
			}
			set
			{
				this._relativeFile = value;
				if (!string.IsNullOrEmpty(this._relativeFile) && !Path.IsPathRooted(this._relativeFile))
				{
					this._scriptFile = Path.Combine(Option.LuaScriptFolder, this._relativeFile);
				}
				else
				{
					this._scriptFile = string.Empty;
				}
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000AF RID: 175 RVA: 0x0000354C File Offset: 0x0000174C
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x00003564 File Offset: 0x00001764
		public string ScriptFile
		{
			get
			{
				return this._scriptFile;
			}
			set
			{
				this._scriptFile = value;
				if (!string.IsNullOrEmpty(this._scriptFile) && Path.IsPathRooted(this._scriptFile))
				{
					this._relativeFile = this.GetRelativeFilePath(this._scriptFile);
				}
				else
				{
					this._relativeFile = string.Empty;
				}
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000035BC File Offset: 0x000017BC
		private string GetRelativeFilePath(string absoluateScriptFilePath)
		{
			FilePath filePath = new FilePath(absoluateScriptFilePath);
			return filePath.ToRelative(new FilePath(Option.LuaScriptFolder));
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000035ED File Offset: 0x000017ED
		public ScriptFileData()
		{
			this.ScriptFile = string.Empty;
			this.FileType = ScriptType.None;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00003622 File Offset: 0x00001822
		public ScriptFileData(string scriptFile, ScriptType fileType)
		{
			this.ScriptFile = scriptFile;
			this.FileType = fileType;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003654 File Offset: 0x00001854
		public bool IsEqual(ScriptFileData item)
		{
			return item != null && item.ScriptFile == this.ScriptFile && item.FileType == this.FileType;
		}

		// Token: 0x0400005B RID: 91
		private string _relativeFile = string.Empty;

		// Token: 0x0400005C RID: 92
		private string _scriptFile = string.Empty;
	}
}
