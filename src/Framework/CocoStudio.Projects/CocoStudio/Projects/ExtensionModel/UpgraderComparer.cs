using System;
using System.Collections.Generic;

namespace CocoStudio.Projects.ExtensionModel
{
	internal class UpgraderComparer : IComparer<IUpgrader>
	{
		public static UpgraderComparer Instance { get; private set; } = new UpgraderComparer();

		private UpgraderComparer()
		{
		}

		public int Compare(IUpgrader x, IUpgrader y)
		{
			return x.Version.CompareTo(y.Version);
		}
	}
}
