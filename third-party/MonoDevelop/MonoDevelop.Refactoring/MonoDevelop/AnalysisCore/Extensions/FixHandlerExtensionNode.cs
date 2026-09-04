using Mono.Addins;

namespace MonoDevelop.AnalysisCore.Extensions
{
	public class FixHandlerExtensionNode : TypeExtensionNode
	{
		[NodeAttribute(Required = true)]
		private string fixName;

		public string FixName => fixName;

		public IFixHandler FixHandler => (IFixHandler)CreateInstance();
	}
}
