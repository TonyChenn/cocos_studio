using System;
using CocoStudio.Basic;
using Mono.Addins;
using MonoDevelop.Core.Serialization;

namespace Modules.Communal.MultiLanguage
{
	[Extension(typeof(IUserConfig))]
	internal class LanguageConfig : IUserConfig
	{
		[ItemProperty("LanguageType/Value")]
		public LanguageType LanguageType { get; set; }

		public const string conifgKey = "CCS_LanguageConfig";
	}
}
