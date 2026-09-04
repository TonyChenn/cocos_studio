using Mono.Addins;

namespace MonoDevelop.AnalysisCore.Extensions
{
	internal class AdaptorAnalysisRuleAddinNode : AnalysisRuleAddinNode
	{
		[NodeAttribute(Required = true, Description = "The ID of the output type.")]
		private string output;

		public override string Output => output;
	}
}
