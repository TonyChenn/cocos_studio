using System;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Projects.ExtensionModel.Upgrade;
using Gtk;
using Modules.Communal.CocosAdapter;
using Xwt.Drawing;

namespace Modules.Communal.NewSolution
{
	public abstract class BaseSolutionTemplate : ISolutionTemplate, IComparable
	{
		public abstract EnumTemplateGroup Group { get; }

		public virtual bool Enable
		{
			get
			{
				return true;
			}
		}

		public SolutionTypeInfo Info { get; protected set; }

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

		protected abstract bool OnCreateNewSolution(CreateParams prms, CocosMonitor monitor);

		protected virtual string OnGetDefaultScenePath(CreateParams prms)
		{
			return Path.Combine(prms.Directory, prms.ProjName, "CocosStudio".ToLower(), "MainScene.csd");
		}

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

		public int CompareTo(object obj)
		{
			BaseSolutionTemplate baseSolutionTemplate = obj as BaseSolutionTemplate;
			if (baseSolutionTemplate == null)
			{
				return this.Info.SolutionType.CompareTo(-1);
			}
			return this.Info.SolutionType.CompareTo(baseSolutionTemplate.Info.SolutionType);
		}

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

		private const string ConfigFilename = "config.json";

		private const string DefaultConfig = "{ 'init_cfg':{ 'isLandscape': false, 'width': 960, 'height': 640 } }";
	}
}
