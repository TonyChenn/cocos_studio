using System;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Core;
using Gtk;
using Modules.Communal.CocosAdapter;
using MonoDevelop.Core;
using Xwt.Drawing;

namespace Modules.Communal.NewSolution
{
	// Token: 0x02000008 RID: 8
	[SolutionTemplate(true)]
	internal class SampleSolutionTemplate : BaseSolutionTemplate
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000038 RID: 56 RVA: 0x0000297B File Offset: 0x00000B7B
		public override EnumTemplateGroup Group
		{
			get
			{
				return EnumTemplateGroup.Sample;
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002980 File Offset: 0x00000B80
		public SampleSolutionTemplate(SampleInfo sampleInfo)
		{
			string displayName = sampleInfo.DisplayName;
			string description = sampleInfo.Description;
			EnumSolutionType type = EnumSolutionType.Sample;
			Xwt.Drawing.Image image = sampleInfo.Image;
			if (image == null)
			{
				image = base.GetIconImage(EnumSolutionType.Sample);
			}
			bool isCompleteSln = sampleInfo.IsCompleteSln;
			base.Info = new SolutionTypeInfo(displayName, description, type, image, isCompleteSln, isCompleteSln, isCompleteSln, isCompleteSln, sampleInfo);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000029D8 File Offset: 0x00000BD8
		protected override bool OnCreateNewSolution(CreateParams prms, CocosMonitor monitor)
		{
			bool result;
			try
			{
				string srcPath = Path.Combine(base.Info.SampleInfo.SamplePath, base.Info.SampleInfo.SampleName);
				string text = Path.Combine(prms.Directory, prms.ProjName);
				FileService.CopyDirectory(srcPath, text);
				string text2 = Path.Combine(text, prms.ProjName) + ".ccs";
				if (!base.Info.SampleInfo.SampleName.Equals(prms.ProjName))
				{
					string oldName = Path.Combine(text, base.Info.SampleInfo.SampleName) + ".ccs";
					FileService.RenameFile(oldName, text2);
					Services.ProjectsService.ChangeSolutionName(Services.ProjectsService.DefaultMonitor, text2, base.Info.SampleInfo.SampleName, prms.ProjName);
				}
				result = true;
			}
			catch (Exception ex)
			{
				monitor.SendInfo(ex.ToString());
				LogConfig.Logger.Error("新建项目时出错", ex);
				result = false;
			}
			return result;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002AEC File Offset: 0x00000CEC
		protected override string OnGetDefaultScenePath(CreateParams prms)
		{
			return Path.Combine(prms.Directory, prms.ProjName, "CocosStudio".ToLower(), base.Info.SampleInfo.DefaultScene);
		}
	}
}
