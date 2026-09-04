using System;
using System.Text;

namespace Modules.Communal.CocosAdapter
{
	// Token: 0x0200001F RID: 31
	internal class StepCocosV3 : CreateStep
	{
		// Token: 0x060000F8 RID: 248 RVA: 0x000059BC File Offset: 0x00003BBC
		protected override bool OnRun(CreateParams prms)
		{
			string cmd = this.CreateParams(prms);
			CocosPythonTool cocosPythonTool = new CocosPythonTool(this.Monitor);
			return cocosPythonTool.RunPython(prms.EngineInfo, cmd, false);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000059EB File Offset: 0x00003BEB
		protected override bool OnCanCreate(CreateParams prms)
		{
			return prms.EngineInfo != null && prms.EngineInfo.MainVersion == 3;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00005A08 File Offset: 0x00003C08
		private string CreateParams(CreateParams prms)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Format(" new -p {0} -l {1} -d {2}", prms.PkgName, prms.Language, prms.Directory));
			if (prms.Language != EnumProgramLanguage.cpp)
			{
				stringBuilder.Append(" -t runtime");
			}
			stringBuilder.Append(" " + prms.ProjName);
			if (!prms.IsHorizonScreen)
			{
				stringBuilder.Append(" --portrait");
			}
			return stringBuilder.ToString();
		}
	}
}
