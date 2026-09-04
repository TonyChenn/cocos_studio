using Mono.Debugging.Client;

namespace MonoDevelop.Debugger
{
	public abstract class InlineVisualizer
	{
		public abstract bool CanInlineVisualize(ObjectValue val);

		public abstract string InlineVisualize(ObjectValue val);
	}
}
