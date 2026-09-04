using System;
using CocoStudio.Projects;
using Mono.Addins;
using MonoDevelop.Core.Serialization;

namespace Modules.Communal.CocosAdapter
{
	// Token: 0x0200000C RID: 12
	[Extension(typeof(IUserData))]
	public class CocosProperties : IUserData
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00002F6C File Offset: 0x0000116C
		// (set) Token: 0x0600004F RID: 79 RVA: 0x00002F74 File Offset: 0x00001174
		[ItemProperty("SolutionCodeType/Value")]
		public EnumSolutionCodeType SolutionCodeType { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00002F7D File Offset: 0x0000117D
		// (set) Token: 0x06000051 RID: 81 RVA: 0x00002F85 File Offset: 0x00001185
		[ItemProperty("ProgramLanguage/Value")]
		public EnumProgramLanguage ProgramLanguage { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00002F8E File Offset: 0x0000118E
		// (set) Token: 0x06000053 RID: 83 RVA: 0x00002F96 File Offset: 0x00001196
		[ItemProperty("CreateFrameworkVersion/Value")]
		public string CreateFrameworkVersion { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00002F9F File Offset: 0x0000119F
		// (set) Token: 0x06000055 RID: 85 RVA: 0x00002FA7 File Offset: 0x000011A7
		[ItemProperty("CurrentFrameworkVersion/Value")]
		public string CurrentFrameworkVersion { get; set; }

		// Token: 0x06000056 RID: 86 RVA: 0x00002FB0 File Offset: 0x000011B0
		public CocosProperties()
		{
			this.InitDefaultValue();
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002FC0 File Offset: 0x000011C0
		private void InitDefaultValue()
		{
			this.SolutionCodeType = EnumSolutionCodeType.Resource;
			this.ProgramLanguage = EnumProgramLanguage.none;
			this.CreateFrameworkVersion = (this.CurrentFrameworkVersion = string.Empty);
		}

		// Token: 0x04000015 RID: 21
		public const string dictionaryKey = "CCS_CocosPropertis";
	}
}
