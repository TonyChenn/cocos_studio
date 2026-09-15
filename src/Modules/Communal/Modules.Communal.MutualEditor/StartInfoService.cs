using System;
using System.Diagnostics;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Projects;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.StartAutoRecover;
using MonoDevelop.Core;

namespace Modules.Communal.MutualEditor
{
	public class StartInfoService
	{
		protected StartInfoService()
		{
			MutualCore.Init();
			this.argsProjFilePath = (this.argsSlnFilePath = string.Empty);
		}

		public static StartInfoService Instance
		{
			get
			{
				if (StartInfoService.startInfoService == null)
				{
					StartInfoService.startInfoService = new StartInfoService();
				}
				return StartInfoService.startInfoService;
			}
		}

		private void OpenSolution(string slnFilePath)
		{
			string text = this.PreCheckFilePathLegal(slnFilePath);
			slnFilePath = Path.Combine(new string[]
			{
				slnFilePath
			});
			if (string.IsNullOrEmpty(text))
			{
				Solution currentSelectedSolution = Services.ProjectOperations.CurrentSelectedSolution;
				if (currentSelectedSolution == null)
				{
					Services.Workspace.OpenWorkspaceItem(slnFilePath);
				}
				else if (Path.GetFullPath(currentSelectedSolution.FileName) == slnFilePath)
				{
					LogConfig.Output.Error(LanguageInfo.Output_SameProjAlreadyOpened);
				}
				else
				{
					if (!Services.ProjectOperations.CloseSolution())
					{
						return;
					}
					GLib.Timeout.Add(100U, delegate
					{
						Services.Workspace.OpenWorkspaceItem(slnFilePath);
						return false;
					});
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				LogConfig.Output.Error(text);
			}
		}

		private string PreCheckFilePathLegal(string prjSlnPath)
		{
			string result = string.Empty;
			if (!File.Exists(prjSlnPath))
			{
				result = string.Format("{0} {1}", prjSlnPath, LanguageInfo.MessageBox_Content100);
			}
			else if (RegexModel.HasChinese(prjSlnPath))
			{
				result = LanguageInfo.MessageBox212_NotChinese;
			}
			return result;
		}

		public void OpenArgsProjSln()
		{
			string text = string.Empty;
			bool flag = !string.IsNullOrEmpty(this.argsSlnFilePath);
			bool flag2 = !string.IsNullOrEmpty(this.argsProjFilePath);
			if (flag)
			{
				text = this.PreCheckFilePathLegal(this.argsSlnFilePath);
				if (string.IsNullOrEmpty(text))
				{
					Services.Workspace.OpenWorkspaceItem(this.argsSlnFilePath);
					if (flag2)
					{
						StartRecoverService.CocosFilePathByDoubleClick = this.argsProjFilePath;
					}
				}
			}
			else
			{
				this.HandleLatestOpenSolution();
			}
			if (!string.IsNullOrEmpty(text))
			{
				LogConfig.Output.Error(text);
			}
		}

		public bool PreCheckArgs(string[] args)
		{
			bool result;
			if (args == null || args.Length < 1)
			{
				result = true;
			}
			else
			{
				bool flag = false;
				string text = "";
				string text2 = "";
				foreach (string text3 in args)
				{
					string text4 = text3.Trim();
					if (Path.HasExtension(text4))
					{
						text2 = Path.GetExtension(text4).ToLower();
						if (text2.Equals(".csd") || text2.Equals(".ccs"))
						{
							text = text4;
							flag = true;
						}
					}
				}
				if (!flag)
				{
					result = true;
				}
				else
				{
					if (text2 == ".csd")
					{
						this.argsProjFilePath = text;
						this.argsSlnFilePath = this.GetCCSFilePath(text);
					}
					else if (text2 == ".ccs")
					{
						this.argsSlnFilePath = text;
					}
					else
					{
						LogConfig.Logger.Error("未知的传入参数：" + text);
					}
					bool flag2 = SolutionLockHandler.Instance.IsSolutionLocked(this.argsSlnFilePath);
					if (flag2)
					{
						MutualCore.Instance.SendMessage(this.argsSlnFilePath, Modules.Communal.MutualEditor.Action.Show);
						MutualCore.Instance.Dispose();
						result = false;
					}
					else
					{
						result = true;
					}
				}
			}
			return result;
		}

		public bool PreCheckLauncher()
		{
			string filePath = "";
			return SolutionLockHandler.Instance.IsFileLocked(filePath);
		}

		public void HandleOpenSolution(string solutionFilePath)
		{
			solutionFilePath = Path.Combine(new string[]
			{
				solutionFilePath
			});
			Solution currentSelectedSolution = Services.ProjectOperations.CurrentSelectedSolution;
			if (currentSelectedSolution != null && Path.GetFullPath(currentSelectedSolution.FileName) == solutionFilePath)
			{
				LogConfig.Output.Error(LanguageInfo.Output_SameProjAlreadyOpened);
			}
			else
			{
				bool flag = SolutionLockHandler.Instance.IsSolutionLocked(solutionFilePath);
				string empty = string.Empty;
				if (flag)
				{
					MutualCore.Instance.SendMessage(solutionFilePath, Modules.Communal.MutualEditor.Action.Show);
					LogConfig.Logger.Error(LanguageInfo.Output_SameProjAlreadyOpened);
				}
				else
				{
					this.OpenSolution(solutionFilePath);
				}
			}
		}

		private void HandleLatestOpenSolution()
		{
			try
			{
				if (Services.ProjectsService.CurrentSolution == null)
				{
					string text = StartRecoverService.Instance.LoadLastSolutionArgs();
					if (text != null)
					{
						bool flag = SolutionLockHandler.Instance.IsSolutionLocked(text);
						if (!flag)
						{
							this.OpenSolution(text);
						}
					}
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Exception while handle the latest solution! ", exception);
			}
		}

		public void HandleMacStartOpen(string projOrSlnPath)
		{
			if (Platform.IsMac)
			{
				string text = this.PreCheckFilePathLegal(projOrSlnPath);
				if (string.IsNullOrEmpty(text))
				{
					projOrSlnPath = Path.Combine(new string[]
					{
						projOrSlnPath
					});
					string text2 = projOrSlnPath;
					string text3 = projOrSlnPath;
					string a = Path.GetExtension(projOrSlnPath).ToLower();
					bool flag = a == ".csd";
					if (flag)
					{
						text3 = this.GetCCSFilePath(text2);
					}
					bool flag2 = Services.ProjectsService.CurrentSolution == null;
					if (!flag2 && Services.ProjectsService.CurrentSolution.FileName == text3)
					{
						MutualCore.ShowCurrApplication();
						return;
					}
					bool flag3 = SolutionLockHandler.Instance.IsSolutionLocked(text3);
					if (flag3)
					{
						MutualCore.Instance.SendMessage(text3, Modules.Communal.MutualEditor.Action.Show);
						text = LanguageInfo.Output_SameProjAlreadyOpened;
					}
					else if (flag2 && this.IsRuningCocosStudio())
					{
						Services.Workspace.OpenWorkspaceItem(text3);
						if (flag)
						{
							StartRecoverService.CocosFilePathByDoubleClick = text2;
						}
					}
					else
					{
						try
						{
							System.Diagnostics.Process process = new System.Diagnostics.Process();
							process.EnableRaisingEvents = false;
							ProcessStartInfo processStartInfo = new ProcessStartInfo();
							processStartInfo.Arguments = projOrSlnPath;
							string fileName = Path.Combine(Option.AssemblyDir, "CocosStudio");
							processStartInfo.FileName = fileName;
							process.StartInfo = processStartInfo;
							if (!process.Start())
							{
								text = LanguageInfo.OpenProjectFail;
							}
						}
						catch (Exception ex)
						{
							text = LanguageInfo.OpenProjectFail + ex.ToString();
						}
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					LogConfig.Output.Error(text);
				}
			}
		}

		private bool IsRuningCocosStudio()
		{
			System.Diagnostics.Process[] processesByName = System.Diagnostics.Process.GetProcessesByName("CocosStudio");
			return processesByName != null && processesByName.Length > 0;
		}

		private string GetCCSFilePath(string csdFilePath)
		{
			string directoryName = Path.GetDirectoryName(csdFilePath);
			DirectoryInfo panrentDirInfo = new DirectoryInfo(directoryName);
			return this.DirRecursion(panrentDirInfo);
		}

		private string DirRecursion(DirectoryInfo panrentDirInfo)
		{
			string result = string.Empty;
			if (panrentDirInfo.Name.Equals("CocosStudio", StringComparison.OrdinalIgnoreCase))
			{
				DirectoryInfo parent = panrentDirInfo.Parent;
				FileInfo[] files = parent.GetFiles("*.ccs");
				if (files.Length > 0)
				{
					return files[0].FullName;
				}
			}
			else
			{
				DirectoryInfo parent = panrentDirInfo.Parent;
				result = this.DirRecursion(parent);
			}
			return result;
		}

		private string argsProjFilePath = string.Empty;

		private string argsSlnFilePath = string.Empty;

		private static StartInfoService startInfoService = null;
	}
}
