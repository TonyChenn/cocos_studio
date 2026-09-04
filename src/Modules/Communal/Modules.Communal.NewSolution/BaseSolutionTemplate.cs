using System;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Projects.ExtensionModel.Upgrade;
using Gtk;
using Modules.Communal.CocosAdapter;
using Xwt.Drawing;

namespace Modules.Communal.NewSolution
{
	// Token: 0x02000004 RID: 4
	public abstract class BaseSolutionTemplate : ISolutionTemplate, IComparable
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000005 RID: 5
		public abstract EnumTemplateGroup Group { get; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000006 RID: 6 RVA: 0x00002050 File Offset: 0x00000250
		public virtual bool Enable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002053 File Offset: 0x00000253
		// (set) Token: 0x06000008 RID: 8 RVA: 0x0000205B File Offset: 0x0000025B
		public SolutionTypeInfo Info { get; protected set; }

		// Token: 0x06000009 RID: 9 RVA: 0x00002064 File Offset: 0x00000264
		public void CreateNewSolution(CreateParams prms, CocosMonitor monitor, out string defaultScenePath)
		{
			monitor.Start();
			bool isSuccess = this.OnCreateNewSolution(prms, monitor);
			this.FixConfigJson(prms);
			defaultScenePath = this.OnGetDefaultScenePath(prms);
			if (monitor.IsProcessing)
			{
				monitor.Finish(isSuccess);
			}
		}

		// Token: 0x0600000A RID: 10
		protected abstract bool OnCreateNewSolution(CreateParams prms, CocosMonitor monitor);

		// Token: 0x0600000B RID: 11 RVA: 0x0000209F File Offset: 0x0000029F
		protected virtual string OnGetDefaultScenePath(CreateParams prms)
		{
			return Path.Combine(prms.Directory, prms.ProjName, "CocosStudio".ToLower(), "MainScene.csd");
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000020C4 File Offset: 0x000002C4
		protected Xwt.Drawing.Image GetIconImage(EnumSolutionType slnType)
		{
			string str = "Modules.Communal.NewSolution.Resource.";
			string text = "";
			switch (slnType)
			{
			case EnumSolutionType.Complete:
				text = "ItemIcon_Cocos.png";
				break;
			case EnumSolutionType.Resource:
				text = "ItemIcon_Res.png";
				break;
			case EnumSolutionType.Sample:
				text = "ItemIcon_Sample.png";
				break;
			}
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			if (Option.CurrentApp == EnumApp.Launcher)
			{
				text = "Launcher_" + text;
			}
			return ImageIcon.GetIcon(str + text);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002134 File Offset: 0x00000334
		public int CompareTo(object obj)
		{
			BaseSolutionTemplate baseSolutionTemplate = obj as BaseSolutionTemplate;
			if (baseSolutionTemplate == null)
			{
				return this.Info.SolutionType.CompareTo(-1);
			}
			return this.Info.SolutionType.CompareTo(baseSolutionTemplate.Info.SolutionType);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000218C File Offset: 0x0000038C
		private void FixConfigJson(CreateParams prms)
		{
			try
			{
				string path = Path.Combine(prms.Directory, prms.ProjName);
				if (Directory.Exists(path))
				{
					SolutionUpgraderHelper.UpdateConfigJson(path, prms.IsHorizonScreen);
				}
			}
			catch (Exception ex)
			{
				LogConfig.OutputWithoutTip.Error(ex.Data);
			}
		}

		// Token: 0x04000005 RID: 5
		private const string ConfigFilename = "config.json";

		// Token: 0x04000006 RID: 6
		private const string DefaultConfig = "{ 'init_cfg':{ 'isLandscape': false, 'width': 960, 'height': 640 } }";
	}
}
