using System;
using System.Text;

namespace Modules.Communal.CocosAdapter
{
	internal class StepCocosV3 : CreateStep
	{
		protected override bool OnRun(CreateParams prms)
		{
			string cmd = this.CreateParams(prms);
			CocosPythonTool cocosPythonTool = new CocosPythonTool(this.Monitor);
			return cocosPythonTool.RunPython(prms.EngineInfo, cmd, false);
		}

		protected override bool OnCanCreate(CreateParams prms)
		{
			return prms.EngineInfo != null && prms.EngineInfo.MainVersion == 3;
		}

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
