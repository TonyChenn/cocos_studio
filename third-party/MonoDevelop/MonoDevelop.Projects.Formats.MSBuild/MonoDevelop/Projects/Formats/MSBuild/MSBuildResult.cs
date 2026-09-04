using System;
using System.Collections.Generic;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	[Serializable]
	public class MSBuildResult
	{
		private readonly MSBuildTargetResult[] errors;

		private readonly Dictionary<string, string> properties;

		private readonly Dictionary<string, List<MSBuildEvaluatedItem>> items;

		public MSBuildTargetResult[] Errors => errors;

		public Dictionary<string, List<MSBuildEvaluatedItem>> Items => items;

		public Dictionary<string, string> Properties => properties;

		public MSBuildResult(MSBuildTargetResult[] errors)
		{
			this.errors = errors;
			properties = new Dictionary<string, string>();
			items = new Dictionary<string, List<MSBuildEvaluatedItem>>();
		}
	}
}
