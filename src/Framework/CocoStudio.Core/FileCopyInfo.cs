using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CocoStudio.Basic;
using CocoStudio.ControlLib;
using CocoStudio.Projects.Formates;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;

namespace CocoStudio.Core
{
	// Token: 0x02000029 RID: 41
	internal class FileCopyInfo : IComparable<FileCopyInfo>
	{
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000173 RID: 371 RVA: 0x000067B0 File Offset: 0x000049B0
		// (set) Token: 0x06000174 RID: 372 RVA: 0x000067C7 File Offset: 0x000049C7
		public bool IsComposite { get; private set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000175 RID: 373 RVA: 0x000067D0 File Offset: 0x000049D0
		// (set) Token: 0x06000176 RID: 374 RVA: 0x000067E7 File Offset: 0x000049E7
		public bool IsPretreatment { get; private set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000177 RID: 375 RVA: 0x000067F0 File Offset: 0x000049F0
		// (set) Token: 0x06000178 RID: 376 RVA: 0x00006807 File Offset: 0x00004A07
		public bool IsAfter { get; private set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00006810 File Offset: 0x00004A10
		// (set) Token: 0x0600017A RID: 378 RVA: 0x00006827 File Offset: 0x00004A27
		public bool IsProjectFile { get; private set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00006830 File Offset: 0x00004A30
		// (set) Token: 0x0600017C RID: 380 RVA: 0x00006847 File Offset: 0x00004A47
		public bool IsHiddenCompositeFile { get; private set; }

		// Token: 0x0600017D RID: 381 RVA: 0x00006850 File Offset: 0x00004A50
		public FileCopyInfo(FilePath parentDir, FilePath file)
		{
			this.SourcePath = file;
			if (file.IsChildPathOf(parentDir))
			{
				this.TargetPath = this.SourcePath;
			}
			else
			{
				this.TargetPath = parentDir.Combine(new string[]
				{
					file.FileName
				});
			}
			this.IsHiddenCompositeFile = true;
			if (CompositeProcesserManager.Instance.PretreatmentTypes.Contains(file.Extension))
			{
				ICompositeResourceProcesser compositeResourceProcesser = Services.ProjectsService.GetCompositeResourceProcesser(this.SourcePath);
				this.IsComposite = true;
				this.IsPretreatment = true;
				this.PairResources = FileCopyInfo.ProcessPairResources(this.SourcePath, compositeResourceProcesser);
				if (compositeResourceProcesser != null)
				{
					this.IsHiddenCompositeFile = compositeResourceProcesser.IsHiddenCompositeFile;
				}
			}
			if (CompositeProcesserManager.Instance.AfterTypes.Contains(file.Extension))
			{
				this.IsAfter = true;
			}
			this.IsProjectFile = Services.ProjectsService.IsCocosFile(file);
			this.Exists = File.Exists(this.TargetPath);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00006974 File Offset: 0x00004B74
		public override string ToString()
		{
			return this.SourcePath;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00006994 File Offset: 0x00004B94
		internal bool Copy(IProgressMonitor monitor)
		{
			bool result;
			try
			{
				if (this.SourcePath.IsDirectory)
				{
					if (!Directory.Exists(this.TargetPath))
					{
						Directory.CreateDirectory(this.TargetPath);
					}
					result = true;
				}
				else
				{
					switch (this.Operate)
					{
					case EFileOperate.KeepBoth:
						if (!this.IsComposite)
						{
							this.TargetPath = this.GetNewFileName(this.TargetPath);
						}
						break;
					case EFileOperate.Skip:
						return true;
					}
					if (!Directory.Exists(this.TargetPath.ParentDirectory))
					{
						Directory.CreateDirectory(this.TargetPath.ParentDirectory);
					}
					if (this.IsComposite)
					{
						result = this.CopyCompositeFiles(monitor);
					}
					else
					{
						File.Copy(this.SourcePath, this.TargetPath, true);
						File.SetAttributes(this.TargetPath, FileAttributes.Normal);
						result = true;
					}
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("文件拷贝失败", exception);
				monitor.ReportError(string.Format(LanguageInfo.Output_ImportFailed, this.SourcePath), exception);
				result = false;
			}
			return result;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00006B04 File Offset: 0x00004D04
		private bool CopyCompositeFiles(IProgressMonitor monitor)
		{
			if (this.IsComposite && this.PairResources != null)
			{
				string text = this.CheckFiles(this.PairResources);
				if (!string.IsNullOrWhiteSpace(text))
				{
					string message = string.Format(LanguageInfo.MessageBox195_NoMatchPng, this.SourcePath, text);
					monitor.ReportWarning(message);
					return false;
				}
				foreach (string name in this.PairResources)
				{
					FilePath filePath = name;
					FilePath filePath2 = filePath;
					FilePath filePath3 = filePath2.ToRelative(this.SourcePath.ParentDirectory).ToAbsolute(this.TargetPath.ParentDirectory);
					string path = filePath3.ParentDirectory;
					if (filePath.IsDirectory)
					{
						if (!Directory.Exists(filePath3))
						{
							Directory.CreateDirectory(filePath3);
						}
					}
					else
					{
						if (!Directory.Exists(path))
						{
							Directory.CreateDirectory(path);
						}
						FileService.CopyFile(filePath, filePath3);
						File.SetAttributes(filePath3, FileAttributes.Normal);
					}
				}
			}
			File.Copy(this.SourcePath, this.TargetPath, true);
			return true;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00006C94 File Offset: 0x00004E94
		private string CheckFiles(IEnumerable<string> files)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (files != null)
			{
				foreach (string name in files)
				{
					FilePath filePath = name;
					if (!filePath.IsDirectory)
					{
						if (!File.Exists(filePath))
						{
							stringBuilder.AppendLine(Environment.NewLine + filePath);
						}
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00006D3C File Offset: 0x00004F3C
		private static List<string> ProcessPairResources(string filePath, ICompositeResourceProcesser process)
		{
			List<string> result;
			if (process != null)
			{
				result = process.GetFiles(filePath);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00006D64 File Offset: 0x00004F64
		private string GetNewFileName(string file)
		{
			string fileName = Path.GetFileName(file);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
			string extension = Path.GetExtension(fileName);
			string directoryName = Path.GetDirectoryName(file);
			int num = 1;
			do
			{
				string str = fileNameWithoutExtension + num;
				file = Path.Combine(directoryName, str + extension);
				num++;
			}
			while (File.Exists(file));
			return file;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00006DCC File Offset: 0x00004FCC
		public bool VerifyPath(IProgressMonitor monitor)
		{
			monitor.Step(1);
			bool result;
			if (!this.SourcePath.IsValidPath())
			{
				result = false;
			}
			else if (!this.CheckImportDirPath(this.SourcePath))
			{
				result = false;
			}
			else if (this.SourcePath.Extension.Equals(".ccs", StringComparison.OrdinalIgnoreCase))
			{
				result = false;
			}
			else
			{
				string input = this.TargetPath.ToString().Replace("\\", "").Replace("/", "").Replace("(", "").Replace(")", "").Replace(":", "").Replace("@", "");
				string pattern = "^[A-Za-z0-9, ._@-]+$";
				if (!Regex.IsMatch(input, pattern))
				{
					monitor.ReportError(string.Format(LanguageInfo.MessageBox199_PathContainsChinese, this.SourcePath), null);
					result = false;
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00006ED8 File Offset: 0x000050D8
		private bool CheckImportDirPath(FilePath path)
		{
			List<string> compositeFilterTypes = CompositeProcesserManager.Instance.CompositeFilterTypes;
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			bool result;
			if (directoryInfo.Attributes.HasFlag(FileAttributes.Hidden))
			{
				result = false;
			}
			else
			{
				if (compositeFilterTypes != null)
				{
					foreach (string text in compositeFilterTypes)
					{
						int num = directoryInfo.Name.Length - text.Length;
						if (num >= 0)
						{
							bool flag = directoryInfo.Name.Substring(num) != text;
							if (!flag)
							{
								return flag;
							}
						}
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00006FC0 File Offset: 0x000051C0
		public int CompareTo(FileCopyInfo other)
		{
			int result;
			if (this.IsPretreatment)
			{
				if (this.IsAfter)
				{
					result = 1;
				}
				else
				{
					result = -1;
				}
			}
			else if (other.IsComposite)
			{
				if (other.IsAfter)
				{
					result = -1;
				}
				else
				{
					result = 1;
				}
			}
			else if (this.IsProjectFile && !other.IsProjectFile)
			{
				result = 1;
			}
			else if (other.IsProjectFile && !this.IsProjectFile)
			{
				result = -1;
			}
			else
			{
				result = this.SourcePath.CompareTo(other.SourcePath);
			}
			return result;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00007060 File Offset: 0x00005260
		public override int GetHashCode()
		{
			return this.SourcePath.GetHashCode();
		}

		// Token: 0x040000DA RID: 218
		public FilePath SourcePath;

		// Token: 0x040000DB RID: 219
		public FilePath TargetPath;

		// Token: 0x040000DC RID: 220
		public bool Exists;

		// Token: 0x040000DD RID: 221
		public EFileOperate Operate;

		// Token: 0x040000DE RID: 222
		public List<string> PairResources;
	}
}
