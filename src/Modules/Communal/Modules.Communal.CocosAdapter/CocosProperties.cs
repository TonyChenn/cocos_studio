using System;
using CocoStudio.Projects;
using Mono.Addins;
using MonoDevelop.Core.Serialization;

namespace Modules.Communal.CocosAdapter
{
	[Extension(typeof(IUserData))]
	public class CocosProperties : IUserData
	{
		[ItemProperty("SolutionCodeType/Value")]
		public EnumSolutionCodeType SolutionCodeType { get; set; }

		[ItemProperty("ProgramLanguage/Value")]
		public EnumProgramLanguage ProgramLanguage { get; set; }

		[ItemProperty("CreateFrameworkVersion/Value")]
		public string CreateFrameworkVersion { get; set; }

		[ItemProperty("CurrentFrameworkVersion/Value")]
		public string CurrentFrameworkVersion { get; set; }

		public CocosProperties()
		{
			this.InitDefaultValue();
		}

		private void InitDefaultValue()
		{
			this.SolutionCodeType = EnumSolutionCodeType.Resource;
			this.ProgramLanguage = EnumProgramLanguage.none;
			this.CreateFrameworkVersion = (this.CurrentFrameworkVersion = string.Empty);
		}

		public const string dictionaryKey = "CCS_CocosPropertis";
	}
}
