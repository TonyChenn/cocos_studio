using Mono.Addins;

namespace MonoDevelop.Debugger
{
	internal class DebuggerEngineExtensionNode : TypeExtensionNode
	{
		[NodeAttribute("name")]
		public string Name;

		[NodeAttribute("features")]
		public string[] SupportedFeatures;
	}
}
