using System;
using System.Collections.Generic;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	[Serializable]
	public class MSBuildEvaluatedItem
	{
		public Dictionary<string, string> Metadata { get; private set; }

		public string ItemSpec { get; private set; }

		public string Name { get; private set; }

		public MSBuildEvaluatedItem(string name, string itemSpec)
		{
			Name = name;
			ItemSpec = itemSpec;
			Metadata = new Dictionary<string, string>();
		}
	}
}
