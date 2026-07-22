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
	// Token: 0x0200001C RID: 28
	public class Cocos2dxServices
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000DF RID: 223 RVA: 0x000056D7 File Offset: 0x000038D7
		// (set) Token: 0x060000E0 RID: 224 RVA: 0x000056DE File Offset: 0x000038DE
		public static CocosSolutionCreator CreateServices { get; private set; } = new CocosSolutionCreator();

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x000056E6 File Offset: 0x000038E6
		// (set) Token: 0x060000E2 RID: 226 RVA: 0x000056ED File Offset: 0x000038ED
		public static PackageServices PackageServices { get; private set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x000056F5 File Offset: 0x000038F5
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x000056FC File Offset: 0x000038FC
		public static Cocos2dxSupplymentServices SupplymentServices { get; private set; } = new Cocos2dxSupplymentServices();

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00005704 File Offset: 0x00003904
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

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x0000578A File Offset: 0x0000398A
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x00005791 File Offset: 0x00003991
		public static CocosRecentServices RecentServices { get; private set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00005799 File Offset: 0x00003999
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x000057A0 File Offset: 0x000039A0
		public static PlatformServices PlatformServices { get; private set; } = new PlatformServices();

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000EA RID: 234 RVA: 0x000057A8 File Offset: 0x000039A8
		// (set) Token: 0x060000EB RID: 235 RVA: 0x000057AF File Offset: 0x000039AF
		public static InstallerServices InstallerServices { get; private set; } = new InstallerServices();

		// Token: 0x060000EC RID: 236 RVA: 0x000057B8 File Offset: 0x000039B8
		static Cocos2dxServices()
		{
			Cocos2dxServices.PackageServices = PackageServices.Instance;
			Cocos2dxServices.RecentServices = CocosRecentServices.Instance;
			Services.ProjectOperations.CurrentSelectedSolutionChanged += Cocos2dxServices.SolutionChangedHandler;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00005817 File Offset: 0x00003A17
		private static void SolutionChangedHandler(object sender, SolutionEventArgs e)
		{
			Cocos2dxServices.tempCocosProperties = null;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00005820 File Offset: 0x00003A20
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

		// Token: 0x04000042 RID: 66
		private static CocosProperties tempCocosProperties;
	}
}
