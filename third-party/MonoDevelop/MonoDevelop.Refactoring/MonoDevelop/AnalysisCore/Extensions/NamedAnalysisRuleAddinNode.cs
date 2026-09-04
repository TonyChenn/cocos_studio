using Mono.Addins;

namespace MonoDevelop.AnalysisCore.Extensions
{
	internal class NamedAnalysisRuleAddinNode : AnalysisRuleAddinNode
	{
		[NodeAttribute("_name", Required = true, Localizable = true, Description = "User-visible name of the rule")]
		private string name;

		public string Name => name;

		public override string Output => "Results";
	}
}
