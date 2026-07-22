using System;
using System.IO;
using CocoStudio.Basic;

namespace Modules.Communal.CocosAdapter
{
	// Token: 0x02000020 RID: 32
	internal class StepX86 : CreateStep
	{
		// Token: 0x060000FC RID: 252 RVA: 0x00005A90 File Offset: 0x00003C90
		protected override bool OnRun(CreateParams prms)
		{
			base.SendOutputInfo("Optimize x86 compiler");
			string path = "";
			string path2 = "";
			string path3 = Path.Combine(Option.AssemblyDir, "Publish", "AppMKFile");
			switch (prms.X86Type)
			{
			case ECompilerType.Null:
				path2 = Path.Combine(path3, "x86.mk");
				break;
			case ECompilerType.GCC:
				path2 = Path.Combine(path3, "x86GCC.mk");
				break;
			case ECompilerType.ICC:
				path2 = Path.Combine(path3, "x86ICC.mk");
				break;
			}
			if (prms.EngineInfo.MainVersion == 2)
			{
				path = Path.Combine(new string[]
				{
					prms.Directory,
					prms.ProjName,
					"projects",
					prms.ProjName,
					"proj.android",
					"jni",
					"Application.mk"
				});
			}
			else if (prms.EngineInfo.MainVersion == 3)
			{
				if (prms.Language == EnumProgramLanguage.cpp)
				{
					path = Path.Combine(new string[]
					{
						prms.Directory,
						prms.ProjName,
						"proj.android",
						"jni",
						"Application.mk"
					});
				}
				else
				{
					path = Path.Combine(new string[]
					{
						prms.Directory,
						prms.ProjName,
						"frameworks",
						"runtime-src",
						"proj.android",
						"jni",
						"Application.mk"
					});
				}
			}
			StreamReader streamReader = File.OpenText(path2);
			string str = streamReader.ReadToEnd();
			streamReader.Close();
			StreamWriter streamWriter = new StreamWriter(path, true);
			streamWriter.Write("\r\n" + str);
			streamWriter.Close();
			return true;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00005C57 File Offset: 0x00003E57
		protected override bool OnCanCreate(CreateParams prms)
		{
			return prms.EngineInfo != null && prms.UseX86;
		}
	}
}
