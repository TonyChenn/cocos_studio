using Mono.Debugging.Client;
using MonoDevelop.Components;

namespace MonoDevelop.Debugger
{
	public abstract class PreviewVisualizer
	{
		public abstract bool CanVisualize(ObjectValue val);

		public abstract Control GetVisualizerWidget(ObjectValue val);
	}
}
