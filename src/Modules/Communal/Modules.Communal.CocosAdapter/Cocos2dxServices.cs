using System;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Core.Events;
using CocoStudio.Projects;
using Gtk;
using Modules.Communal.CocosAdapter.Platform;

namespace Modules.Communal.CocosAdapter
{
	public class Cocos2dxServices
	{
		public static CocosSolutionCreator CreateServices { get; private set; } = new CocosSolutionCreator();

		public static PackageServices PackageServices { get; private set; }

		public static Cocos2dxSupplymentServices SupplymentServices { get; private set; } = new Cocos2dxSupplymentServices();

		public static CocosProperties CocosProperties
		{
			get
			{
				Solution currentSolution = Services.ProjectsService.CurrentSolution;
				if (currentSolution == null)
				{
					if (Cocos2dxServices.tempCocosProperties == null)
					{
						Cocos2dxServices.tempCocosProperties = new CocosProperties();
					}
					return Cocos2dxServices.tempCocosProperties;
				}
				CocosProperties cocosProperties = null;
				if (currentSolution.Config.CustomProperties.ContainsKey("CCS_CocosPropertis"))
				{
					cocosProperties = (currentSolution.Config.CustomProperties["CCS_CocosPropertis"] as CocosProperties);
				}
				if (cocosProperties == null)
				{
					cocosProperties = new CocosProperties();
					currentSolution.Config.CustomProperties["CCS_CocosPropertis"] = cocosProperties;
				}
				return cocosProperties;
			}
		}

		public static CocosRecentServices RecentServices { get; private set; }

		public static PlatformServices PlatformServices { get; private set; } = new PlatformServices();

		public static InstallerServices InstallerServices { get; private set; } = new InstallerServices();

		static Cocos2dxServices()
		{
			Cocos2dxServices.PackageServices = PackageServices.Instance;
			Cocos2dxServices.RecentServices = CocosRecentServices.Instance;
			Services.ProjectOperations.CurrentSelectedSolutionChanged += Cocos2dxServices.SolutionChangedHandler;
		}

		private static void SolutionChangedHandler(object sender, SolutionEventArgs e)
		{
			Cocos2dxServices.tempCocosProperties = null;
		}

		internal static bool CopyFolder(string srcPath, string dstPath, bool overwrite, CocosMonitor monitor)
		{
			bool result;
			try
			{
				if (monitor.IsCancelled)
				{
					result = false;
				}
				else
				{
					if (!Directory.Exists(dstPath))
					{
						Directory.CreateDirectory(dstPath);
					}
					string[] files = Directory.GetFiles(srcPath);
					if (files.Length > 0)
					{
						foreach (string text in files)
						{
							string text2 = Path.Combine(dstPath, Path.GetFileName(text));
							if (!File.Exists(text2) || overwrite)
							{
								File.Copy(text, text2, true);
							}
						}
					}
					string[] directories = Directory.GetDirectories(srcPath);
					foreach (string text3 in directories)
					{
						if (monitor.IsCancelled)
						{
							return false;
						}
						string dstPath2 = Path.Combine(dstPath, Path.GetFileName(text3));
						if (!Cocos2dxServices.CopyFolder(text3, dstPath2, overwrite, monitor))
						{
							return false;
						}
					}
					result = true;
				}
			}
			catch (Exception ex)
			{
				if (monitor != null)
				{
					monitor.SendInfo("Failed to copy file");
				}
				LogConfig.Logger.Error("复制文件失败：\r\n" + ex.ToString());
				result = false;
			}
			return result;
		}

		private static CocosProperties tempCocosProperties;
	}
}
