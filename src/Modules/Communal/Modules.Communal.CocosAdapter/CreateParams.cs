using System;
using CocoStudio.Projects;

namespace Modules.Communal.CocosAdapter
{
	public class CreateParams
	{
		public string ProjName { get; private set; }

		public string Directory { get; private set; }

		public bool IsHorizonScreen { get; private set; }

		public string PkgName { get; private set; }

		public Cocos2dxInfo EngineInfo { get; private set; }

		public EnumProgramLanguage Language { get; private set; }

		public bool UseX86 { get; set; }

		public ECompilerType X86Type { get; set; }

		public CreateParams(string projectName, string directory, bool isHorizon)
		{
			this.ProjName = projectName;
			this.Directory = directory;
			this.IsHorizonScreen = isHorizon;
			this.PkgName = "";
			this.EngineInfo = null;
			this.Language = EnumProgramLanguage.none;
			this.UseX86 = false;
			this.X86Type = ECompilerType.Null;
		}

		public CreateParams(string projectName, string directory, bool isHorizon, string packageName, Cocos2dxInfo engineInfo, EnumProgramLanguage language)
		{
			this.ProjName = projectName;
			this.Directory = directory;
			this.IsHorizonScreen = isHorizon;
			this.PkgName = packageName;
			this.EngineInfo = engineInfo;
			this.Language = language;
			this.UseX86 = false;
			this.X86Type = ECompilerType.Null;
		}
	}
}
