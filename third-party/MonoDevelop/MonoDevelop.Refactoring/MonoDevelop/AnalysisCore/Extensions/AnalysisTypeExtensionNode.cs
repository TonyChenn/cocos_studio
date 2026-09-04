using Mono.Addins;

namespace MonoDevelop.AnalysisCore.Extensions
{
	public class AnalysisTypeExtensionNode : TypeExtensionNode
	{
		[NodeAttribute(Required = true)]
		private string name;

		public string Name => name;
	}
}
