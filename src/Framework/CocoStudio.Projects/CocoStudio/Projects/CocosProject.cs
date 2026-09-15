using System;
using System.Collections.Generic;
using MonoDevelop.Projects;

namespace CocoStudio.Projects
{
	public class CocosProject : Project
	{
		public static CocosProject Instance { get; private set; } = new CocosProject();

		private CocosProject()
		{
		}

		public override IEnumerable<string> GetProjectTypes()
		{
			return new string[]
			{
				".ccs"
			};
		}
	}
}
