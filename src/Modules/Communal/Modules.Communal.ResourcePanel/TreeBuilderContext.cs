using System;

namespace Modules.Communal.ResourcePanel
{
	public class TreeBuilderContext : ITreeBuilderContext
	{
		internal TreeBuilderContext(ResourceTreeView pad)
		{
			this.pad = pad;
		}

		public ITreeBuild GetTreeBuilder()
		{
			return this.pad.Builder;
		}

		public ITreeBuild GetTreeBuilder(object dataObject)
		{
			return this.pad.Builder;
		}

		public ResourceTreeView Tree
		{
			get
			{
				return this.pad;
			}
		}

		private ResourceTreeView pad;
	}
}
