using System;
using CocoStudio.Projects;

namespace Modules.Communal.CocosAdapter
{
	// Token: 0x02000021 RID: 33
	public class CreateParams
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00005C71 File Offset: 0x00003E71
		// (set) Token: 0x06000100 RID: 256 RVA: 0x00005C79 File Offset: 0x00003E79
		public string ProjName { get; private set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00005C82 File Offset: 0x00003E82
		// (set) Token: 0x06000102 RID: 258 RVA: 0x00005C8A File Offset: 0x00003E8A
		public string Directory { get; private set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00005C93 File Offset: 0x00003E93
		// (set) Token: 0x06000104 RID: 260 RVA: 0x00005C9B File Offset: 0x00003E9B
		public bool IsHorizonScreen { get; private set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00005CA4 File Offset: 0x00003EA4
		// (set) Token: 0x06000106 RID: 262 RVA: 0x00005CAC File Offset: 0x00003EAC
		public string PkgName { get; private set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00005CB5 File Offset: 0x00003EB5
		// (set) Token: 0x06000108 RID: 264 RVA: 0x00005CBD File Offset: 0x00003EBD
		public Cocos2dxInfo EngineInfo { get; private set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00005CC6 File Offset: 0x00003EC6
		// (set) Token: 0x0600010A RID: 266 RVA: 0x00005CCE File Offset: 0x00003ECE
		public EnumProgramLanguage Language { get; private set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00005CD7 File Offset: 0x00003ED7
		// (set) Token: 0x0600010C RID: 268 RVA: 0x00005CDF File Offset: 0x00003EDF
		public bool UseX86 { get; set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00005CE8 File Offset: 0x00003EE8
		// (set) Token: 0x0600010E RID: 270 RVA: 0x00005CF0 File Offset: 0x00003EF0
		public ECompilerType X86Type { get; set; }

		// Token: 0x0600010F RID: 271 RVA: 0x00005CFC File Offset: 0x00003EFC
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

		// Token: 0x06000110 RID: 272 RVA: 0x00005D4C File Offset: 0x00003F4C
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
