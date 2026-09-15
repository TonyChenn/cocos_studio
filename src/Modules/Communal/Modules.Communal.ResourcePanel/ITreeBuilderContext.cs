using System;

namespace Modules.Communal.ResourcePanel
{
	public interface ITreeBuilderContext
	{
		ITreeBuild GetTreeBuilder();

		ITreeBuild GetTreeBuilder(object dataObject);

		ResourceTreeView Tree { get; }
	}
}
