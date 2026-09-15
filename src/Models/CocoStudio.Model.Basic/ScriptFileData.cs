using System;
using System.IO;
using CocoStudio.Basic;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Model
{
	public class ScriptFileData
	{
		[ItemProperty(DefaultValue = ScriptType.None)]
		public ScriptType FileType { get; set; }

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

		private string GetRelativeFilePath(string absoluateScriptFilePath)
		{
			FilePath filePath = new FilePath(absoluateScriptFilePath);
			return filePath.ToRelative(new FilePath(Option.LuaScriptFolder));
		}

		public ScriptFileData()
		{
			this.ScriptFile = string.Empty;
			this.FileType = ScriptType.None;
		}

		public ScriptFileData(string scriptFile, ScriptType fileType)
		{
			this.ScriptFile = scriptFile;
			this.FileType = fileType;
		}

		public bool IsEqual(ScriptFileData item)
		{
			return item != null && item.ScriptFile == this.ScriptFile && item.FileType == this.FileType;
		}

		private string _relativeFile = string.Empty;

		private string _scriptFile = string.Empty;
	}
}
