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
	internal class FileCopyInfo : IComparable<FileCopyInfo>
	{
		public bool IsComposite { get; private set; }

		public bool IsPretreatment { get; private set; }

		public bool IsAfter { get; private set; }

		public bool IsProjectFile { get; private set; }

		public bool IsHiddenCompositeFile { get; private set; }

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

		public override string ToString()
		{
			return this.SourcePath;
		}

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

		public override int GetHashCode()
		{
			return this.SourcePath.GetHashCode();
		}

		public FilePath SourcePath;

		public FilePath TargetPath;

		public bool Exists;

		public EFileOperate Operate;

		public List<string> PairResources;
	}
}
